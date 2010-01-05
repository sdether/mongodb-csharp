using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using MongoDB.Framework.Configuration.Mapping;

namespace MongoDB.Framework.Proxy
{
    /// <remarks>
    /// This was wholesale ripped out of NHibernate.
    /// </remarks>
    public abstract class BasicLazyInitializer : AbstractLazyInitializer
    {
        private string idMemberName;
        private bool overridesEquals;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicLazyInitializer"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="mongoContext">The mongo context.</param>
        protected BasicLazyInitializer(Type entityType, object id, IMongoContextImplementor mongoContext)
            : base(entityType, id, mongoContext)
        {
            this.idMemberName = mongoContext.MappingStore.GetClassMapFor(entityType).IdMap.MemberName;
            this.overridesEquals = entityType.Overrides("Equals", new[] { typeof(object) });
        }

        /// <summary>
        /// Invokes the method if this is something that the LazyInitializer can handle
        /// without the underlying proxied object being instantiated.
        /// </summary>
        /// <param name="method">The name of the method/property to Invoke.</param>
        /// <param name="args">The arguments to pass the method/property.</param>
        /// <param name="proxy">The proxy object that the method is being invoked on.</param>
        /// <returns>
        /// The result of the Invoke if the underlying proxied object is not needed.  If the 
        /// underlying proxied object is needed then it returns the result <see cref="AbstractLazyInitializer.InvokeImplementation"/>
        /// which indicates that the Proxy will need to forward to the real implementation.
        /// </returns>
        public virtual object Invoke(MethodInfo method, object[] args, object proxy)
        {
            string methodName = method.Name;
            int paramCount = method.GetParameters().Length;

            if (paramCount == 0)
            {
                if (!overridesEquals && methodName == "GetHashCode")
                {
                    return IdentityEqualityComparer.GetHashCode(proxy);
                }
                //TODO: can't do this because we don't have the underlying methods, just an action method
                else if (!this.IsInitialized && methodName == "get_" + this.idMemberName)
                {
                    return this.Id;
                }
                else if (methodName == "Dispose")
                {
                    return null;
                }
                else if ("get_LazyInitializer".Equals(methodName))
                {
                    return this;
                }
            }
            else if (paramCount == 1)
            {
                if (!overridesEquals && methodName == "Equals")
                {
                    return IdentityEqualityComparer.Equals(args[0], proxy);
                }
                else if (methodName == "set_" + this.idMemberName)
                {
                    this.Initialize();
                    this.Id = args[0];
                    return invokeImplementation;
                }
            }
            else if (paramCount == 2)
            {
                // if the Proxy Engine delegates the call of GetObjectData to the Initializer
                // then we need to handle it.
                if (methodName == "GetObjectData")
                {
                    SerializationInfo info = (SerializationInfo)args[0];
                    StreamingContext context = (StreamingContext)args[1]; // not used !?!

                    if (this.Target == null & this.MongoContext != null)
                    {
                        object entity = this.MongoContext.GetById(this.EntityType, this.Id);
                        if (entity != null)
                            this.SetImplementation(entity);
                    }

                    this.AddSerializationInfo(info, context);
                    return null;
                }
            }

            return invokeImplementation;
        }

        /// <summary>
        /// Adds all of the information into the SerializationInfo that is needed to
        /// reconstruct the proxy during deserialization or to replace the proxy
        /// with the instantiated target.
        /// </summary>
        /// <remarks>
        /// This will only be called if the Dynamic Proxy generator does not handle serialization
        /// itself or delegates calls to the method GetObjectData to the LazyInitializer.
        /// </remarks>
        protected virtual void AddSerializationInfo(SerializationInfo info, StreamingContext context)
        { }
    }
}