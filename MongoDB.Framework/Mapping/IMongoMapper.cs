using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.Framework.Mapping
{
    public interface IMongoMapper
    {
        Document MapToDocument(object entity);

        object MapToEntity(Type entityType, Document document);
    }

    public static class IMongoMapperExtensions
    {
        public static T MapToEntity<T>(this IMongoMapper mongoMapper, Document document)
        {
            if (mongoMapper == null)
                throw new ArgumentNullException("mongoMapper");

            return (T)mongoMapper.MapToEntity(typeof(T), document);
        }
    }
}