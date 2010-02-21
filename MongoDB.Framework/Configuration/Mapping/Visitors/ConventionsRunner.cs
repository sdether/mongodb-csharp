using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Visitors
{
    public class ConventionsRunner : DefaultMapModelVisitor
    {
        private MappingConventions conventions;
        private HashSet<Type> rootClassTypes;

        public ConventionsRunner(IEnumerable<Type> rootClassTypes)
        {
            this.rootClassTypes = new HashSet<Type>(rootClassTypes ?? Type.EmptyTypes);
        }

        public void ApplyConventions(ClassMapModel classMapModel)
        {
            this.conventions = classMapModel.Conventions;
            this.Visit(classMapModel);
        }

        protected override RootClassMapModel VisitRootClass(RootClassMapModel model)
        {
            if (string.IsNullOrEmpty(model.CollectionName))
            {
                model.CollectionName = this.conventions
                        .GetCollectionNameConvention(model.Type)
                        .GetCollectionName(model.Type);
            }

            if (model.SubClassMaps.Count == 0)
                model.DiscriminatorKey = null;

            this.ProcessSuperClass(model);

            return base.VisitRootClass(model);
        }

        protected override NestedClassMapModel VisitNestedClass(NestedClassMapModel model)
        {
            this.ProcessSuperClass(model);
            return base.VisitNestedClass(model);
        }

        protected override SubClassMapModel VisitSubClass(SubClassMapModel model)
        {
            this.ProcessClass(model);
            return base.VisitSubClass(model);
        }

        protected override IdMapModel VisitId(IdMapModel model)
        {
            this.ProcessMember(model);
            this.ProcessConvertibleMember(model);

            return base.VisitId(model);
        }

        protected override ConvertibleMemberMapModel VisitConvertibleMember(ConvertibleMemberMapModel model)
        {
            this.ProcessMember(model);
            this.ProcessConvertibleMember(model);

            return base.VisitConvertibleMember(model);
        }

        protected override CollectionMemberMapModel VisitCollectionMember(CollectionMemberMapModel model)
        {
            this.ProcessMember(model);

            var memberType = ReflectionUtil.GetMemberValueType(model.Getter);
            var collectionConvention = this.conventions.GetCollectionConvention(memberType);

            if (model.CollectionType == null)
                model.CollectionType = collectionConvention.GetCollectionType(memberType);

            if (model.ElementType == null)
                model.ElementType = collectionConvention.GetElementType(memberType);

            return base.VisitCollectionMember(model);
        }

        protected override ManyToOneMapModel VisitManyToOneMember(ManyToOneMapModel model)
        {
            this.ProcessMember(model);

            if (!model.IsLazy.HasValue)
            {
                //TODO: Make this a convention...
                var memberType = ReflectionUtil.GetMemberValueType(model.Getter);
                model.IsLazy = memberType.IsSealed;
            }

            return base.VisitManyToOneMember(model);
        }

        protected override PersistentMemberMapModel VisitPersistentMember(PersistentMemberMapModel model)
        {
            var memberType = ReflectionUtil.GetMemberValueType(model.Getter);
            var collectionConvention = this.conventions.GetCollectionConvention(memberType);
            if (collectionConvention.IsCollection(memberType))
            {
                var newModel = new CollectionMemberMapModel()
                {
                    Key = model.Key,
                    Getter = model.Getter,
                    Setter = model.Setter,
                    PersistNull = model.PersistNull
                };

                return (CollectionMemberMapModel)this.Visit(newModel);
            }
            else if (this.rootClassTypes.Contains(memberType))
            {
                var newModel = new ManyToOneMapModel()
                {
                    Key = model.Key,
                    Getter = model.Getter,
                    Setter = model.Setter,
                    PersistNull = model.PersistNull
                };

                return (ManyToOneMapModel)this.Visit(newModel);
            }
            else
            {
                var newModel = new ConvertibleMemberMapModel()
                {
                    Key = model.Key,
                    Getter = model.Getter,
                    Setter = model.Setter,
                    PersistNull = model.PersistNull
                };

                return (ConvertibleMemberMapModel)this.Visit(newModel);
            }

            return base.VisitPersistentMember(model);
        }

        #region Private Methods

        private void ProcessSuperClass(SuperClassMapModel model)
        {
            this.ProcessClass(model);

            if (model.ExtendedPropertiesMap == null)
            {
                var convention = this.conventions.GetExtendedPropertiesConvention(model.Type);

                if (convention.HasExtendedProperties(model.Type))
                    model.ExtendedPropertiesMap = convention.GetExtendedPropertiesMapModel(model.Type);
            }

            if (model.IdMap == null)
            {
                var convention = this.conventions.GetIdConvention(model.Type);

                if (convention.HasId(model.Type))
                    model.IdMap = convention.GetIdMapModel(model.Type);
            }
        }

        private void ProcessClass(ClassMapModel model)
        {
            foreach (var member in this.conventions.GetMemberFinder(model.Type).FindMembers(model.Type))
            {
                var memberMapModel = new PersistentMemberMapModel()
                {
                    Getter = member,
                    Setter = member
                };

                model.PersistentMemberMaps.Add(memberMapModel);
            }

            //find parent map?
        }

        private void ProcessMember(PersistentMemberMapModel model)
        {
            if (string.IsNullOrEmpty(model.Key))
            {
                model.Key = this.conventions
                    .GetMemberKeyConvention(model.Getter)
                    .GetKey(model.Getter);
            }
        }

        private void ProcessConvertibleMember(ConvertibleMemberMapModel model)
        {
            if (model.ValueConverter == null)
            {
                model.ValueConverter = this.conventions
                    .GetValueConverterConvention(model.Getter)
                    .GetValueConverter(model.Getter);
            }
        }

        #endregion
    }
}