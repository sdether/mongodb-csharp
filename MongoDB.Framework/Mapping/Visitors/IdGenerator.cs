using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Visitors
{
    public class IdGenerator : DefaultMapVisitor
    {
        private ClassMap currentClassMap;
        private object currentEntity;
        private IMongoSessionImplementor mongoSession;

        public IdGenerator(IMongoSessionImplementor mongoSession)
        {
            if (mongoSession == null)
                throw new ArgumentNullException("mongoSession");

            this.mongoSession = mongoSession;
        }

        public void GenerateIdsFor(object entity, ClassMap classMap)
        {
            this.currentClassMap = classMap;
            this.currentEntity = entity;

            this.Visit(classMap);
        }

        public override void Visit(ValueTypeMemberMap memberMap)
        {
            if (memberMap.ValueType is NestedClassValueType)
            {
                var vt = (NestedClassValueType)memberMap.ValueType;
                var oldEntity = currentEntity;
                var oldClassMap = currentClassMap;
                currentEntity = memberMap.MemberGetter(this.currentEntity);
                currentClassMap = vt.NestedClassMap;

                this.Visit(currentClassMap);

                this.currentEntity = oldEntity;
                this.currentClassMap = oldClassMap;
            }
        }

        public override void Visit(IdMap idMap)
        {
            var id = idMap.MemberGetter(this.currentEntity);
            if(!Object.Equals(id, idMap.UnsavedValue))
                return;

            id = idMap.Generate(this.currentEntity, this.mongoSession);
            idMap.MemberSetter(this.currentEntity, id);
        }

    }
}