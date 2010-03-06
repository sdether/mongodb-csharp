using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Proxy
{
    /// <remarks>
    /// This was wholesale ripped out of NHibernate.
    /// </remarks>
    public interface ILazyInitializer
    {
        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>The type of the entity.</value>
        Type EntityType { get; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        object Id { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        bool IsInitialized { get; }

        /// <summary>
        /// Gets the mongo session.
        /// </summary>
        /// <value>The mongo session.</value>
        IMongoSession MongoSession { get; }

        /// <summary>
        /// Performs the load.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <returns></returns>
        object GetImplementation();

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <param name="mongoSession">The mongo session.</param>
        /// <returns></returns>
        object GetImplementation(IMongoSessionImplementor mongoSession);

        /// <summary>
        /// Sets the implementation.
        /// </summary>
        /// <param name="target">The target.</param>
        void SetImplementation(object target);
    }
}