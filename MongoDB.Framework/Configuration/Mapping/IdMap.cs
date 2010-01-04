using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Configuration.Mapping
{
    public class IdMap : MemberMap
    {
        /// <summary>
        /// Gets the id generator.
        /// </summary>
        /// <value>The id generator.</value>
        public IIdGenerator IdGenerator { get; private set; }

        /// <summary>
        /// Gets or sets the unsaved value.
        /// </summary>
        /// <value>The unsaved value.</value>
        public object UnsavedValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdMap"/> class.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="idGenerator">The id generator.</param>
        public IdMap(string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter, IValueType valueType, IIdGenerator idGenerator, object unsavedValue)
            : base("_id", memberName, memberGetter, memberSetter, valueType)
        {
            if (idGenerator == null)
                throw new ArgumentNullException("idGenerator");

            this.IdGenerator = idGenerator;
            this.UnsavedValue = unsavedValue;
        }

        /// <summary>
        /// Generates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="mongoContext">The mongo context.</param>
        /// <returns></returns>
        public object Generate(object entity, IMongoContext mongoContext)
        {
            return this.IdGenerator.Generate(entity, mongoContext);
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.ProcessId(this);

            base.Accept(visitor);
        }
    }
}