using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration.Mapping.Visitors
{
    public class MapModelConventionRunner : DefaultMapModelVisitor
    {
        private AutoMapModel autoMapModel;
        private HashSet<Type> rootClassTypes;

        public MapModelConventionRunner(IEnumerable<Type> rootClassTypes)
        {
            if (rootClassTypes == null)
                throw new ArgumentNullException("rootClassTypes");

            this.rootClassTypes = new HashSet<Type>(rootClassTypes);
        }

        public void ApplyConventions(ClassMapModel classMapModel)
        {
            this.autoMapModel = classMapModel.AutoMap;
            this.Visit(classMapModel);
        }

        protected override RootClassMapModel VisitRootClass(RootClassMapModel model)
        {
            this.ProcessSuperClass(model);
            this.ProcessClass(model);

            if (string.IsNullOrEmpty(model.CollectionName))
            {
                model.CollectionName = this.autoMapModel
                        .GetCollectionNameConvention(model.Type)
                        .GetCollectionName(model.Type);
            }

            return base.VisitRootClass(model);
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
            var collectionConvention = this.autoMapModel.GetCollectionConvention(memberType);

            if(model.CollectionType == null)
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
            var collectionConvention = this.autoMapModel.GetCollectionConvention(memberType);
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
            if (model.ExtendedPropertiesMap == null)
            {
                var convention = this.autoMapModel.GetExtendedPropertiesConvention(model.Type);

                if(convention.HasExtendedProperties(model.Type))
                    model.ExtendedPropertiesMap = convention.GetExtendedPropertiesMapModel(model.Type);
            }

            if(string.IsNullOrEmpty(model.DiscriminatorKey))
            {
                var convention = this.autoMapModel.GetDiscriminatorConvention(model.Type);
                
                if(convention.HasDiscriminator(model.Type))    
                    model.DiscriminatorKey = convention.GetDiscriminatorKey(model.Type);
            }

            if (model.IdMap == null)
            {
                var convention = this.autoMapModel.GetIdConvention(model.Type);

                if (convention.HasId(model.Type))
                    model.IdMap = convention.GetIdMapModel(model.Type);
            }
        }

        private void ProcessClass(ClassMapModel model)
        {
            if (model.Discriminator == null)
            {
                var convention = this.autoMapModel.GetDiscriminatorConvention(model.Type);

                if (convention.HasDiscriminator(model.Type))
                    model.Discriminator = convention.GetDiscriminator(model.Type);
            }

            //find collection members?
            //find nested class members?
            //find value members?
            //find parent map?
        }

        private void ProcessMember(PersistentMemberMapModel model)
        {
            if (string.IsNullOrEmpty(model.Key))
            {
                model.Key = this.autoMapModel
                    .GetMemberKeyConvention(model.Getter)
                    .GetKey(model.Getter);
            }
        }

        private void ProcessConvertibleMember(ConvertibleMemberMapModel model)
        {
            if (model.ValueConverter == null)
            {
                model.ValueConverter = this.autoMapModel
                    .GetValueConverterConvention(model.Getter)
                    .GetValueConverter(model.Getter);
            }
        }

        #endregion
    }
}