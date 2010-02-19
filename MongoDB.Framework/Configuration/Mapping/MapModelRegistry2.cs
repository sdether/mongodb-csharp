//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using MongoDB.Framework.Configuration.Mapping.Visitors;
//using MongoDB.Framework.Mapping;
//using MongoDB.Framework.Configuration.Fluent.Mapping;

//namespace MongoDB.Framework.Configuration.Mapping
//{
//    public class MapModelRegistry2 : NullMapModelVisitor, IMapModelRegistry
//    {
//        #region Private Fields

//        private Dictionary<Type, Func<Type, Type>> elementTypeFactories = new Dictionary<Type, Func<Type, Type>>()//        {//            { typeof(ICollection<>), mt => mt.GetGenericArguments()[0] },//            { typeof(IList<>), mt => mt.GetGenericArguments()[0] },//            { typeof(List<>), mt => mt.GetGenericArguments()[0] },//            { typeof(HashSet<>), mt => mt.GetGenericArguments()[0] },//            { typeof(IDictionary<,>), mt => mt.GetGenericArguments()[1] },//            { typeof(Dictionary<,>), mt => mt.GetGenericArguments()[1] },//        };

//        private Dictionary<Type, ModelWithAutoMapPair<RootClassMapModel>> rootClassMapModels;
//        private Dictionary<Type, ModelWithAutoMapPair<NestedClassMapModel>> nestedClassMapModels;
//        private Dictionary<Type, ModelWithAutoMapPair<SubClassMapModel>> subClassMapModels;

//        private Dictionary<Type, RootClassMap> rootClassMaps;
//        private Dictionary<Type, NestedClassMap> nestedClassMaps;

//        #endregion

//        #region Public Methods

//        /// <summary>
//        /// Adds the root class map model.
//        /// </summary>
//        /// <param name="rootClassMapModel">The root class map model.</param>
//        public void AddRootClassMapModel(RootClassMapModel rootClassMapModel)
//        {
//            this.AddRootClassMapModel(rootClassMapModel, null);
//        }

//        public void AddRootClassMapModel(RootClassMapModel rootClassMapModel, AutoMapModel autoMapModel)
//        {
//            if (rootClassMapModel == null)
//                throw new ArgumentNullException("rootClassMapModel");

//            this.rootClassMapModels.Add(rootClassMapModel.Type, new ModelWithAutoMapPair<RootClassMapModel>(rootClassMapModel, autoMapModel ?? new AutoMapModel()));
//        }

//        /// <summary>
//        /// Adds the nested class map model.
//        /// </summary>
//        /// <param name="nestedClassMapModel">The nested class map model.</param>
//        public void AddNestedClassMapModel(NestedClassMapModel nestedClassMapModel)
//        {
//            this.AddNestedClassMapModel(nestedClassMapModel, null);
//        }

//        /// <summary>
//        /// Adds the nested class map model.
//        /// </summary>
//        /// <param name="nestedClassMapModel">The nested class map model.</param>
//        public void AddNestedClassMapModel(NestedClassMapModel nestedClassMapModel, AutoMapModel autoMapModel)
//        {
//            if (nestedClassMapModel == null)
//                throw new ArgumentNullException("nestedClassMapModel");

//            this.nestedClassMapModels.Add(nestedClassMapModel.Type, new ModelWithAutoMapPair<NestedClassMapModel>(nestedClassMapModel, autoMapModel ?? new AutoMapModel()));
//        }

//        /// <summary>
//        /// Adds the sub class map model.
//        /// </summary>
//        /// <param name="subClassMapModel">The sub class map model.</param>
//        public void AddSubClassMapModel(SubClassMapModel subClassMapModel)
//        {
//            this.AddSubClassMapModel(subClassMapModel, null);
//        }

//        /// <summary>
//        /// Adds the sub class map model.
//        /// </summary>
//        /// <param name="subClassMapModel">The sub class map model.</param>
//        public void AddSubClassMapModel(SubClassMapModel subClassMapModel, AutoMapModel autoMapModel)
//        {
//            if (subClassMapModel == null)
//                throw new ArgumentNullException("subClassMapModel");

//            this.subClassMapModels.Add(subClassMapModel.Type, new ModelWithAutoMapPair<SubClassMapModel>(subClassMapModel, autoMapModel ?? new AutoMapModel()));
//        }

//        /// <summary>
//        /// Builds the mapping store.
//        /// </summary>
//        /// <returns></returns>
//        public IMappingStore BuildMappingStore()
//        {
//            this.BuildRootClassMaps();
//            return new MappingStore(this.rootClassMaps.Values);
//        }

//        #endregion

//        #region Private Methods

//        private void AssociateFreeSubClassMapsWithSupers()
//        {
//            Func<Type, Func<Type, bool>, Type> getSuperClassType = null;
//            getSuperClassType = (type, containsKey) =>
//            {
//                var baseType = type.BaseType;
//                if (baseType == typeof(object))
//                    return null;

//                if (containsKey(baseType))
//                    return baseType;

//                return getSuperClassType(baseType, containsKey);
//            };

//            //TODO: do the association...
//        }

//        private void BuildRootClassMaps()
//        {
//            this.AssociateFreeSubClassMapsWithSupers();

//            this.rootClassMaps = new Dictionary<Type, RootClassMap>();
//            this.nestedClassMaps = new Dictionary<Type, NestedClassMap>();

//            foreach (var rootClassMapModel in this.rootClassMapModels.Values)
//            {
//                this.currentAutoMapModel = rootClassMapModel.AutoMapModel;
//                this.currentClassMapModel.Accept(this);
//            }
//        }

//        #endregion

//        #region Private Classes

//        private class ModelWithAutoMapPair<T> where T : ClassMapModel
//        {
//            public T ClassMapModel { get; private set; }

//            public AutoMapModel AutoMapModel { get; private set; }

//            public ModelWithAutoMapPair(T classMapModel, AutoMapModel autoMapModel)
//            {
//                this.ClassMapModel = classMapModel;
//                this.AutoMapModel = autoMapModel;
//            }
//        }

//        #endregion
//    }
//}
