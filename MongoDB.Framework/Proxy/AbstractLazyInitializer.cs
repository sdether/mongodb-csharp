using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Proxy
{
    /// <remarks>
    /// This was wholesale ripped out of NHibernate.
    /// </remarks>
    public abstract class AbstractLazyInitializer : ILazyInitializer
    {
        /// <summary>
        /// This is returned by Invoke when the subclass needs to invoke the
        /// method call against the object that is being proxied.
        /// </summar
        protected static readonly object invokeImplementation = new object();

        #region Private Fields

        [NonSerialized]
        private IMongoContextImplementor mongoContext;

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <value>The target.</value>
        protected object Target { get; private set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the type of the root class.
        /// </summary>
        /// <value>The type of the root class.</value>
        public Type EntityType { get; private set; }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public object Id { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets the mongo context.
        /// </summary>
        /// <value>The mongo context.</value>
        public IMongoContext MongoContext
        {
            get { return this.mongoContext; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLazyInitializer"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="mongoContext">The mongo context.</param>
        protected AbstractLazyInitializer(Type entityType, object id, IMongoContextImplementor mongoContext)
        {
            if (entityType == null)
                throw new ArgumentNullException("rootClassType");
            if (id == null)
                throw new ArgumentNullException("id");
            if (mongoContext == null)
                throw new ArgumentNullException("mongoContext");

            this.EntityType = entityType;
            this.Id = id;
            this.IsInitialized = false;
            this.mongoContext = mongoContext;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs the load.
        /// </summary>
        public void Initialize()
        {
            if (!this.IsInitialized)
            {
                if (this.MongoContext == null)
                    throw new LazyInitializationException(string.Format("Could not initialize proxy for {0}, {1} - no Mongo Context.", this.EntityType, this.Id));

                //TODO: maybe check for connectivity on mongo context

                this.Target = this.MongoContext.GetById(this.EntityType, this.Id);
                this.IsInitialized = true;
            }
        }

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <returns></returns>
        public object GetImplementation()
        {
            this.Initialize();
            return this.Target;
        }

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public object GetImplementation(IMongoContextImplementor mongoContext)
        {
            return mongoContext.GetById(this.EntityType, this.Id);
        }

        /// <summary>
        /// Sets the implementation.
        /// </summary>
        /// <param name="target">The target.</param>
        public void SetImplementation(object target)
        {
            this.Target = target;
            this.IsInitialized = true;
        }

        #endregion
    }

}