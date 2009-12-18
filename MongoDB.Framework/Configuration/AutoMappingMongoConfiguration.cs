using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration
{
    public class AutoMappingMongoConfiguration : MongoConfiguration
    {
        /// <summary>
        /// Called when an entity map could not be found.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="rootEntityMap">The root entity map.</param>
        /// <returns></returns>
        protected override bool OnMissingEntityMap(Type type, out RootEntityMap rootEntityMap)
        {
            rootEntityMap = new RootEntityMap(type);
            this.AutoMap(rootEntityMap);
            return true;
        }

        /// <summary>
        /// Automatically creates the member mappings.
        /// </summary>
        /// <param name="rootEntityMap">The root entity map.</param>
        private void AutoMap(RootEntityMap rootEntityMap)
        {
            var props = rootEntityMap.Type.GetProperties()
                .Where(p => p.CanRead && p.CanWrite);

            //foreach(var prop in props)
            //    rootEntityMap.AddMemberMap(new PrimitiveMemberMap(prop));
        }
    }
}