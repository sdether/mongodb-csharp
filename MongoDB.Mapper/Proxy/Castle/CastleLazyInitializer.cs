using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Interceptor;
using System.Reflection;

namespace MongoDB.Mapper.Proxy.Castle
{
    /// <remarks>
    /// This was wholesale ripped out of NHibernate.
    /// </remarks>
    public class CastleLazyInitializer : BasicLazyInitializer, IInterceptor
    {
        private static readonly MethodInfo Exception_InternalPreserveStackTrace =
            typeof(Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

        public bool constructed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleLazyInitializer"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="mongoSession">The mongo session.</param>
        public CastleLazyInitializer(Type entityType, object id, IMongoSessionImplementor mongoSession)
            : base(entityType, id, mongoSession)
        { }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public virtual void Intercept(IInvocation invocation)
        {
            if(!constructed)
                return;

            try
            {
                invocation.ReturnValue = base.Invoke(invocation.Method, invocation.Arguments, invocation.Proxy);
                if (invocation.ReturnValue == invokeImplementation)
                    invocation.ReturnValue = invocation.Method.Invoke(GetImplementation(), invocation.Arguments);
            }
            catch (TargetInvocationException ex)
            {
                Exception_InternalPreserveStackTrace.Invoke(ex.InnerException, new Object[] { });
                throw ex.InnerException;
            }
        }
    }
}