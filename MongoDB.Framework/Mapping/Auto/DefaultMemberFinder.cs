using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MongoDB.Framework.Mapping.Auto
{
    public class DefaultMemberFinder : IMemberFinder
    {
        public static readonly DefaultMemberFinder Instance = new DefaultMemberFinder();

        private DefaultMemberFinder()
        { }

        public IEnumerable<MemberInfo> FindMembers(Type type)
        {
            foreach (var prop in type.GetProperties())
            {
                if (prop.CanRead && prop.CanWrite)
                    yield return prop;
            }

            foreach (var field in type.GetFields())
            {
                if (!field.IsInitOnly && !field.IsLiteral)
                    yield return field;
            }
        }

    }
}