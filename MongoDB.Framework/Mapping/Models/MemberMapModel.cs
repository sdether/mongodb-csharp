using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Models
{
    public class MemberMapModel : MapModel
    {
        public string Key { get; set; }

        public MemberInfo Getter { get; set; }

        public MemberInfo Setter { get; set; }

        public EmbeddedCollectionPart CollectionPart { get; set; }

        public EmbeddedValuePart ValuePart { get; set; }

        public override void Accept(IMapModelVisitor visitor)
        {
            visitor.ProcessMember(this);
        }
    }
}