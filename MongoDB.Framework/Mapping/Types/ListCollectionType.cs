using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Types
{
    public class ListCollectionType : ICollectionType
    {
        private readonly MethodInfo addMethod;

        /// <summary>
        /// Gets the type of the collection.
        /// </summary>
        /// <value>The type of the collection.</value>
        public Type CollectionType { get; private set; }

        /// <summary>
        /// Gets the type of the element value.
        /// </summary>
        /// <value>The type of the element value.</value>
        public IValueType ElementValueType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCollectionType"/> class.
        /// </summary>
        /// <param name="elementValueType">Type of the element value.</param>
        public ListCollectionType(IValueType elementValueType)
        {
            if (elementValueType == null)
                throw new ArgumentNullException("elementValueType");

            this.ElementValueType = elementValueType;

            this.CollectionType = typeof(List<>).MakeGenericType(this.ElementValueType.Type);
            this.addMethod = this.CollectionType.GetMethod("Add", new [] { this.ElementValueType.Type });
        }

        public object CreateCollection(IEnumerable<object> elements)
        {
            var list = Activator.CreateInstance(this.CollectionType);
            foreach (var element in elements)
                addMethod.Invoke(list, new [] { element });
            return list;
        }
    }
}