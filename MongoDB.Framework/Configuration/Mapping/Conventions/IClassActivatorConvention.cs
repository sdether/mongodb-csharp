using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB.Framework.Mapping;

namespace MongoDB.Framework.Configuration.Mapping.Conventions
{
    public interface IClassActivatorConvention : IConvention<Type>
    {
        /// <summary>
        /// Determines whether this instance [can create activator] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can create activator] the specified type; otherwise, <c>false</c>.
        /// </returns>
        bool CanCreateActivator(Type type);

        /// <summary>
        /// Creates the activator.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        IClassActivator CreateActivator(Type type);
    }
}