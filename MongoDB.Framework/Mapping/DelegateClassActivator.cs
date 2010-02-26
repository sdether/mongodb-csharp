using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class DelegateClassActivator : IClassActivator
    {
        private Func<Type, Document, object> activator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateClassActivator"/> class.
        /// </summary>
        /// <param name="activator">The activator.</param>
        public DelegateClassActivator(Func<Type, Document, object> activator)
        {
            if (activator == null)
                throw new ArgumentNullException("activator");

            this.activator = activator;
        }
        /// <summary>
        /// Activates the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="document">The document.</param>
        /// <returns></returns>
        public object Activate(Type type, Document document)
        {
            return this.activator(type, document);
        }
    }
}
