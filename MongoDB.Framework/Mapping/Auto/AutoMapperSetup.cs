using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Framework.Mapping.Conventions;

namespace MongoDB.Framework.Mapping.Auto
{
    public class AutoMapperSetup
    {
        private ConventionProfile conventions;
        private Func<Type, bool> isSubClass;
        private IMemberFinder memberFinder;

        public ConventionProfile Conventions
        {
            get { return this.conventions; }
            set
            {
                if(value == null)
                    throw new ArgumentNullException("value");

                this.conventions = value;
            }
        }

        public IMemberFinder MemberFinder
        {
            get { return this.memberFinder; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.memberFinder = memberFinder;
            }
        }

        public Func<Type, bool> IsSubClass
        {
            get { return this.IsSubClass; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.isSubClass = value;
            }
        }

        public AutoMapperSetup()
        {
            this.conventions = new ConventionProfile();
            this.isSubClass = t => false;
            this.memberFinder = DefaultMemberFinder.Instance;
        }
    }
}