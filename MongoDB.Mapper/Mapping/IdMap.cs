﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Mapper.Mapping
{
    public class IdMap : PersistentMemberMap
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
        /// Gets the value converter.
        /// </summary>
        /// <value>The value converter.</value>
        public IValueConverter ValueConverter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdMap"/> class.
        /// </summary>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="memberGetter">The member getter.</param>
        /// <param name="memberSetter">The member setter.</param>
        /// <param name="valueType">Type of the value.</param>
        /// <param name="idGenerator">The id generator.</param>
        public IdMap(string memberName, Func<object, object> memberGetter, Action<object, object> memberSetter, IIdGenerator idGenerator, IValueConverter valueConverter, object unsavedValue)
            : base("_id", memberName, memberGetter, memberSetter, true)
        {
            if (idGenerator == null)
                throw new ArgumentNullException("idGenerator");

            this.IdGenerator = idGenerator;
            this.UnsavedValue = unsavedValue;
            this.ValueConverter = valueConverter;
        }

        /// <summary>
        /// Generates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="mongoSession">The mongo session.</param>
        /// <returns></returns>
        public object Generate(object entity, IMongoSessionImplementor mongoSession)
        {
            return this.IdGenerator.Generate(entity, mongoSession);
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IMapVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}