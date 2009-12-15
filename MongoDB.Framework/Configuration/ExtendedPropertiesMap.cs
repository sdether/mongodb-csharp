using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Reflection;

namespace MongoDB.Framework.Configuration
{
    public class ExtendedPropertiesMap
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the getter.
        /// </summary>
        /// <value>The getter.</value>
        public Func<object, object> Getter { get; private set; }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>The name of the member.</value>
        public string MemberName { get; private set; }

        /// <summary>
        /// Gets or sets the setter.
        /// </summary>
        /// <value>The setter.</value>
        public Action<object, object> Setter { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedPropertiesMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        public ExtendedPropertiesMap(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException("memberInfo");
            if (!typeof(IDictionary<string, object>).IsAssignableFrom(LateBoundReflection.GetMemberValueType(memberInfo)))
                throw new ArgumentException("ExtendedProperties must be of type IDictionary<string, object>.");

            this.MemberName = memberInfo.Name;
            this.Getter = LateBoundReflection.GetGetter(memberInfo);
            this.Setter = LateBoundReflection.GetSetter(memberInfo);
        }

        #endregion
    }
}