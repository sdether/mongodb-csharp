using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Fluent
{
    public class FluentMemberMap
    {
        private PrimitiveMemberMap memberMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentMemberMap"/> class.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        public FluentMemberMap(PrimitiveMemberMap memberMap)
        {
            if (memberMap == null)
                throw new ArgumentNullException("memberMap");

            this.memberMap = memberMap;
        }

        /// <summary>
        /// Converteds the by.
        /// </summary>
        /// <param name="memberConverter">The member converter.</param>
        public FluentMemberMap ConvertedBy(IValueConverter converter)
        {
            this.memberMap.Converter = converter;
            return this;
        }

    }
}
