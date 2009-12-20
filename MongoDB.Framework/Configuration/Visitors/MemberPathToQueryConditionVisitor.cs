using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MongoDB.Driver;

namespace MongoDB.Framework.Configuration.Visitors
{
    public class MemberPathToQueryConditionVisitor : MapVisitor
    {
        private Stack<MemberInfo> memberPathParts;
        private List<string> documentKeyParts;

        private string documentKey;
        private object documentValue;
        private object comparisonValue;

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
                return string.Join(".", this.documentKeyParts.ToArray());
            }
        }

        public object DocumentValue
        {
            get { return this.documentValue; }
        }

        public MemberPathToQueryConditionVisitor(IEnumerable<MemberInfo> memberPathParts, object comparisonValue)
        {
            if (memberPathParts == null)
                throw new ArgumentNullException("memberPathParts");

            this.comparisonValue = comparisonValue;
            this.documentKey = "";
            this.documentKeyParts = new List<string>();
            this.memberPathParts = new Stack<MemberInfo>(memberPathParts.Reverse());
        }

        public override void VisitEntityMap(EntityMap entityMap)
        {
            if (this.IsFinished)
                return;

            var memberMap = entityMap.GetMemberMap(this.CurrentMemberInfo.DeclaringType, this.CurrentMemberInfo.Name);
            memberMap.Accept(this);
        }

        public override void VisitPrimitiveMemberMap(PrimitiveMemberMap primitiveMemberMap)
        {
            this.memberPathParts.Pop();
            this.documentKeyParts.Add(primitiveMemberMap.DocumentKey);
            if (this.IsFinished)
            {
                this.documentValue = primitiveMemberMap.Converter.ConvertToDocumentValue(this.comparisonValue);
            }
        }

        public override void VisitComponentMemberMap(ComponentMemberMap componentMemberMap)
        {
            this.memberPathParts.Pop();
            this.documentKeyParts.Add(componentMemberMap.DocumentKey);
            if(this.IsFinished)
            {
                var translator = new EntityToDocumentTranslator(this.comparisonValue);
                componentMemberMap.EntityMap.Accept(translator);
                this.documentValue = translator.Document;
            }
            else
                componentMemberMap.EntityMap.Accept(this);
        }

        public override void VisitIdMap(IdMap idMap)
        {
            if (this.CurrentMemberInfo.Name == idMap.MemberName)
            {
                this.documentKeyParts.Add(idMap.DocumentKey);
                this.documentValue = new Oid((string)this.comparisonValue);
                this.memberPathParts.Pop();
            }
        }
    }
}