using System;
using System.Collections.Generic;
using System.Text;

namespace AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Collection
{
    public interface IMongoDBStateContext
    {
        string CollectionName { get; set; }
        TimeSpan TTLExpiration { get; set; }
        string ExpirationFieldName { get; set; }
    }
}
