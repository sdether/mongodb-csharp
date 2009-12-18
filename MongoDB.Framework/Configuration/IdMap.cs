using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Reflection;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration
{
    public class IdMap : IMapVisitable
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

        /// <summary>
        /// Gets the transient value.
        /// </summary>
        /// <value>The transient value.</value>
        public string[] TransientValues
        {
            get { return new [] { null, string.Empty }; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedPropertiesMap"/> class.
        /// </summary>
        /// <param name="memberInfo">The member info.</param>
        public IdMap(MemberInfo memberInfo)
        {
            if (memberInfo == null)
                throw new ArgumentNullException("memberInfo");
            if (typeof(string) != LateBoundReflection.GetMemberValueType(memberInfo))
                throw new ArgumentException("Id member must be of type string.");

            this.MemberName = memberInfo.Name;
            this.Getter = LateBoundReflection.GetGetter(memberInfo);
            this.Setter = LateBoundReflection.GetSetter(memberInfo);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public void Accept(IMapVisitor visitor)
        {
            visitor.VisitIdMap(this);
        }

        /// <summary>
        /// Gets the document value from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public object GetDocumentValueFromEntity(object entity)
        {
            string id = (string)this.Getter(entity);
            if (id != null)
                return new Oid(id);
            return null;
        }

        /// <summary>
        /// Sets the document value on entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="documentValue">The document value.</param>
        public void SetDocumentValueOnEntity(object entity, object documentValue)
        {
            Oid oid = documentValue as Oid;
            if (oid != null)
            {
                string id = BitConverter.ToString(oid.Value).Replace("-", "").ToLower();
                this.Setter(entity, id);
            }
        }

        #endregion
    }
}