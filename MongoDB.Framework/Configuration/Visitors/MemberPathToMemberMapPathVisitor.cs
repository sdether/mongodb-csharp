using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Visitors
{
    public class MemberPathToMemberMapPathVisitor : IMapVisitor
    {
        private Stack<MemberInfo> memberPathParts;
        private List<MemberMap> memberMapPath;

        private string documentKey;

        private MemberInfo CurrentMemberInfo
        {
            get { return this.memberPathParts.Peek(); }
        }

        private bool IsFinished
        {
            get { return this.memberPathParts.Count == 0; }
        }

        public string DocumentKey
        {
            get
            {
                return string.Join(".", this.memberMapPath.Select(mm => mm.DocumentKey).ToArray());
            }
        }

        public IList<MemberMap> MemberMapPath
        {
            get { return this.memberMapPath; }
        }

        public MemberPathToMemberMapPathVisitor(IEnumerable<MemberInfo> memberPathParts)
        {
            if (memberPathParts == null)
                throw new ArgumentNullException("memberPathParts");

            this.documentKey = "";
            this.memberMapPath = new List<MemberMap>();
            this.memberPathParts = new Stack<MemberInfo>(memberPathParts.Reverse());
        }

        public void VisitRootEntityMap(RootEntityMap rootEntityMap)
        {
            rootEntityMap.IdMap.Accept(this);
            this.VisitEntityMap(rootEntityMap);
        }

        public void VisitEntityMap(EntityMap entityMap)
        {
            if (this.IsFinished)
                return;

            var memberMap = entityMap.GetMemberMap(this.CurrentMemberInfo.DeclaringType, this.CurrentMemberInfo.Name);
            memberMap.Accept(this);
        }

        public void VisitDiscriminatedEntityMap(DiscriminatedEntityMap discriminatedEntityMap)
        {
            throw new NotSupportedException();
        }

        public void VisitPrimitiveMemberMap(PrimitiveMemberMap primitiveMemberMap)
        {
            this.memberPathParts.Pop();
            this.memberMapPath.Add(primitiveMemberMap);
        }

        public void VisitComponentMemberMap(ComponentMemberMap componentMemberMap)
        {
            this.memberPathParts.Pop();
            this.memberMapPath.Add(componentMemberMap);
            if(!this.IsFinished)
                componentMemberMap.EntityMap.Accept(this);
        }

        public void VisitIdMap(IdMap idMap)
        {
            if (this.CurrentMemberInfo.Name == idMap.MemberName)
            {
                this.memberMapPath.Add(idMap);
                this.memberPathParts.Pop();
            }
        }
    }
}