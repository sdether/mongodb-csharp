using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public class TranslationContext
    {
        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>The document.</value>
        public Document Document { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public object Owner { get;  set; }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public TranslationContext Parent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationContext"/> class.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="owner">The owner.</param>
        public TranslationContext()
        {
        }

        /// <summary>
        /// Creates the child.
        /// </summary>
        /// <returns></returns>
        public virtual TranslationContext CreateChild()
        {
            return new TranslationContext() { Parent = this };
        }

        /// <summary>
        /// Creates the owner.
        /// </summary>
        /// <param name="type">The type.</param>
        public virtual void CreateOwner(Type type)
        {
            this.Owner = Activator.CreateInstance(type);
        }
    }
}