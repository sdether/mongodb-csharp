using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Proxy
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
        private IMongoSessionImplementor mongoSession;

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
        /// Gets the mongo session.
        /// </summary>
        /// <value>The mongo session.</value>
        public IMongoSession MongoSession
        {
            get { return this.mongoSession; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLazyInitializer"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="mongoSession">The mongo session.</param>
        protected AbstractLazyInitializer(Type entityType, object id, IMongoSessionImplementor mongoSession)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");
            if (id == null)
                throw new ArgumentNullException("id");
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");

            this.EntityType = entityType;
            this.Id = id;
            this.IsInitialized = false;
            this.mongoSession = mongoSession;
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
                if (this.MongoSession == null)
                    throw new LazyInitializationException(string.Format("Could not initialize proxy for {0}, {1} - no Mongo Context.", this.EntityType, this.Id));

                //TODO: maybe check for connectivity on mongo session

                this.Target = this.MongoSession.GetById(this.EntityType, this.Id);
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
        /// <param name="mongoSession">The mongo session.</param>
        /// <returns></returns>
        public object GetImplementation(IMongoSessionImplementor mongoSession)
        {
            return mongoSession.GetById(this.EntityType, this.Id);
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