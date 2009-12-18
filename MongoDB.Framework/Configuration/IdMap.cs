using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Framework.Reflection;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration
{
    public class IdMap : MemberMap
    {
        #region Public Properties

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
        public IdMap(string memberName, Func<object, object> getter, Action<object, object> setter)
            : base(memberName, getter, setter, "_id")
        { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.VisitIdMap(this);
        }

        /// <summary>
        /// Gets the document value from entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override object GetDocumentValueFromEntity(object entity)
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
        public override void SetDocumentValueOnEntity(object entity, object documentValue)
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