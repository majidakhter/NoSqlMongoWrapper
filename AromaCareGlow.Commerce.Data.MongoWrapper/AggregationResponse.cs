using MongoDB.Bson.Serialization.Attributes;

namespace AromaCareGlow.Commerce.Data.MongoWrapper
{
    public class AggregationResponse<TResult> where TResult : class
    {
        [BsonElement("AggregationResponseObject")]
        public TResult AggregationResponseObject { get; set; }
    }
}
