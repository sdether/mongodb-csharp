using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Framework.Mapping.Conventions
{
    public class ConventionSetup
    {
        private IClassActivatorConvention classActivatorConvention;
        private ICollectionNameConvention collectionNameConvention;
        private ICollectionValueTypeConvention collectionValueTypeConvention;
        private IExtendedPropertiesConvention extendedPropertiesConvention;
        private IIdConvention idConvention;
        private IIdGeneratorConvention idGeneratorConvention;
        private IIdUnsavedValueConvention idUnsavedValueConvention;
        private IMemberKeyConvention memberKeyConvention;
        private IValueConverterConvention valueConverterConvention;

        public IClassActivatorConvention ClassActivatorConvention
        {
            get { return this.classActivatorConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.classActivatorConvention = value;
            }
        }

        public ICollectionNameConvention CollectionNameConvention
        {
            get { return this.collectionNameConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.collectionNameConvention = value;
            }
        }

        public ICollectionValueTypeConvention CollectionValueTypeConvention
        {
            get { return this.collectionValueTypeConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.collectionValueTypeConvention = value;
            }
        }

        public IExtendedPropertiesConvention ExtendedPropertiesConvention
        {
            get { return this.extendedPropertiesConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.extendedPropertiesConvention = extendedPropertiesConvention;
            }
        }

        public IIdConvention IdConvention
        {
            get { return this.idConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.idConvention = value;
            }
        }

        public IIdGeneratorConvention IdGeneratorConvention
        {
            get { return this.idGeneratorConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.idGeneratorConvention = value;
            }
        }

        public IIdUnsavedValueConvention IdUnsavedValueConvention
        {
            get { return this.idUnsavedValueConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.idUnsavedValueConvention = value;
            }
        }

        public IMemberKeyConvention MemberKeyConvention
        {
            get { return this.memberKeyConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.memberKeyConvention = value;
            }
        }

        public IValueConverterConvention ValueConverterConvention
        {
            get { return this.valueConverterConvention; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this.valueConverterConvention = value;
            }
        }

        public ConventionSetup()
        {
            this.classActivatorConvention = DefaultClassActivatorConvention.Instance;
            this.collectionNameConvention = new DelegateCollectionNameConvention(t => t.Name);
            this.collectionValueTypeConvention = DefaultCollectionValueTypeConvention.Instance;
            this.extendedPropertiesConvention = new DelegateExtendedPropertiesConvention(m => m.ReflectedType == typeof(IDictionary<string, object>));
            this.idConvention = new DelegateIdConvention(m => m.Name == "Id");
            this.idGeneratorConvention = DefaultIdGeneratorConvention.Instance;
            this.idUnsavedValueConvention = DefaultIdUnsavedValueConvention.Instance;
            this.memberKeyConvention = new DelegateMemberKeyConvention(m => m.Name);
            this.valueConverterConvention = DefaultValueConverterConvention.Instance;
        }
    }
}