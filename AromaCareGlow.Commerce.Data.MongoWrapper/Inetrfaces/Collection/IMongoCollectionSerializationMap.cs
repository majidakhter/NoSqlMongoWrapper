using MongoDB.Bson.Serialization;
using System;

namespace AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Collection
{
    public interface IMongoCollectionSerializationMap<T>
    {
        Action<BsonClassMap<T>> SerializationClassMap();
    }
}
