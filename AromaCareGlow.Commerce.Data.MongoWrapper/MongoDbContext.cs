using MongoDB.Driver;
using AromaCareGlow.Commerce.Data.MongoWrapper.Helpers;
using AromaCareGlow.Commerce.Data.MongoWrapper.Configuration;
using MongoDB.Bson.Serialization;
using System.Linq.Expressions;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Collection;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Operations;
using System.Linq;

namespace AromaCareGlow.Commerce.Data.MongoWrapper
{
    public class MongoDbContext : MongoClient, IMongoDbContext
    {
        
        private readonly MongoClientSettings _settings;
        private readonly IMongoDatabase _mongoDatabase;        

        public MongoDbContext()
        {
            var helper = new MongoDbSettingsHelper();
            _settings = helper.client.Settings;
            _mongoDatabase = helper.client.GetDatabase(helper.DefaultDatabase);            
            CreateCollection(helper.MongoCollections);
        }

        public MongoDbContext(MongoDbSettingsHelper helper)            
        {
            _settings = helper.client.Settings;
            _mongoDatabase = helper.client.GetDatabase(helper.DefaultDatabase);            
            CreateCollection(helper.MongoCollections);
        }

        public MongoDbContext(string connectionString, string defaultDatabase)
        {
            var helper = new MongoDbSettingsHelper(connectionString, defaultDatabase);
            _settings = helper.client.Settings;
            _mongoDatabase = helper.client.GetDatabase(helper.DefaultDatabase);             
        }

        #region Support Methods for Managing Collections

        public IMongoCollection<T> GetCollection<T>(IMongoDBStateContext stateContext) where T : class
        { 
            return GetCollectionObject<T>(stateContext);
        }

        //public IMongoCollection<T> GetCollectionWithClassMap<T>() where T : class, IMongoCollectionSerializationMap<T>, new()
        //{
        //    if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
        //    {
        //        Action<BsonClassMap<T>> classMap = (new T()).SerializationClassMap();
        //        if (classMap != null) BsonClassMap.RegisterClassMap<T>(classMap);
        //        else BsonClassMap.RegisterClassMap<T>();
        //    }

        //    return GetCollectionObject<T>();
        //}

        private void CreateCollection(MongoCollections mongoCollections)
        {
            foreach (var collection in mongoCollections)
            {
                _mongoDatabase.GetCollection<BsonDocument>(((MongoCollection)collection).Name);
            }
        }

        private IMongoCollection<T> GetCollectionObject<T>(IMongoDBStateContext stateContext)
        {
            IMongoCollection<T> collectionObject = null;

            if (stateContext!= null && !string.IsNullOrEmpty(stateContext.CollectionName))
            {
                collectionObject = _mongoDatabase.GetCollection<T>(stateContext.CollectionName);
            }
            else
            {
                collectionObject = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            }

            CreateCollectionTTLIndex(collectionObject, stateContext);

            return collectionObject;
        }


        private void CreateCollectionTTLIndex<T>(IMongoCollection<T> collectionObject, IMongoDBStateContext stateContext)
        {
            if (stateContext != null && stateContext.TTLExpiration != null && stateContext.TTLExpiration.TotalSeconds > 0 && !string.IsNullOrEmpty(stateContext.ExpirationFieldName))
            {
                try
                {
                    var tsk = collectionObject.Indexes.CreateOneAsync(Builders<T>.IndexKeys.Ascending(stateContext.ExpirationFieldName),
                            new CreateIndexOptions() { ExpireAfter = stateContext.TTLExpiration });

                    tsk.Wait();
                }
                catch (AggregateException ex)
                {

                }
            }
        }

        #endregion

        #region CRUD Operations

        #region Create/Add Documents

        public StoreResult<T> InsertSingle<T>(T @object, IMongoDBStateContext stateContext = null) where T : class
        {                       
           GetCollection<T>(stateContext).InsertOne(@object);           
           return new StoreResult<T>(true, @object, null);
        }

        public async Task<StoreResult<T>> InsertSingleAsync<T>(T @object, IMongoDBStateContext stateContext = null) where T : class
        {
            await GetCollection<T>(stateContext).InsertOneAsync(@object);
            return new StoreResult<T>(true, @object, null);
        }

        public StoreResult<T> InsertMany<T>(IEnumerable<T> objects, IMongoDBStateContext stateContext = null) where T : class
        {
            GetCollection<T>(stateContext).InsertMany(objects);
            return new StoreResult<T>(true, null, objects);
        }

        public async Task<StoreResult<T>> InsertManyAsync<T>(IEnumerable<T> objects, IMongoDBStateContext stateContext = null) where T : class
        {
            await GetCollection<T>(stateContext).InsertManyAsync(objects);
            return new StoreResult<T>(true, null, objects);
        }       

        #endregion

        #region Read/Get Documents
        public T FindOne<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {            
            var filter = Builders<T>.Filter.And(expression);
            var result = GetCollection<T>(stateContext).Find<T>(filter).FirstOrDefault();
            return result;       
        }

        public async Task<T> FindOneAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            var result = await GetCollection<T>(stateContext).FindAsync<T>(filter);
            return result.FirstOrDefault();
        }

        public IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            return GetCollection<T>(stateContext).Find<T>(filter).ToList();
        }

        public IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> expression, FindOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            return GetCollection<T>(stateContext).FindSync<T>(filter, options).ToList();
        }

        public async Task<IEnumerable<T>> FindAllAysnc<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            var result =await  GetCollection<T>(stateContext).FindAsync<T>(filter).ConfigureAwait(false);
            //Task.WaitAny(result);
            return result.ToList();
        }

        public async Task<IEnumerable<T>> FindAllAysnc<T>(Expression<Func<T, bool>> expression, FindOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            var result =await  GetCollection<T>(stateContext).FindAsync<T>(filter, options).ConfigureAwait(true);
         
            // await Task.WhenAll(result);
            return result.ToList();
            //Task<IEnumerable<T>> t =await result;
            //return Task.FromResult(result);
        }
       

        public P FindAndProjectOne<T, P>(Expression<Func<T, P>> projectionParameter, Expression<Func<T, bool>> searchParameters, IMongoDBStateContext stateContext = null) where T : class
        {
            var projection = Builders<T>.Projection.Expression(projectionParameter);
            var filter = Builders<T>.Filter.And(searchParameters);
            return GetCollection<T>(stateContext).Find(filter).Project(projection).FirstOrDefault();
        }

        public List<P> FindAndProjectMany<T, P>(Expression<Func<T, P>> projectionParameter, Expression<Func<T, bool>> searchParameters, IMongoDBStateContext stateContext = null) where T : class
        {
            var projection = Builders<T>.Projection.Expression(projectionParameter);
            var filter = Builders<T>.Filter.And(searchParameters);
            return GetCollection<T>(stateContext).Find(filter).Project(projection).ToList();
        }

        public IEnumerable<T> FindUsingRegex<T>(string fieldName, string searchKey, RegexOptions options, IMongoDBStateContext stateContext = null) where T : class
        {  
            var regex = new BsonRegularExpression(new Regex(searchKey, options));
            var filter = Builders<T>.Filter.Regex(fieldName, regex);
            var result = GetCollection<T>(stateContext).Find<T>(filter).ToList();
            return result;
        }

        public List<P> FindAndProjectEmbeddedDocuments<T, P>(BsonDocument[] pipeline, IMongoDBStateContext stateContext = null) where T : class
        {
            var result = GetCollection<T>(stateContext).Aggregate<P>(pipeline).ToList();
            return result;
        }

        #endregion

        #region Update Documents

        public T FindOneAndUpdate<T, TField>(Expression<Func<T, TField>> expression, TField value, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.Eq(expression, value);            
            var result = GetCollection<T>(stateContext).FindOneAndUpdate<T>(filter, updateDef);
            return result;
        }

        public T FindOneAndUpdate<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);  
            var result = GetCollection<T>(stateContext).FindOneAndUpdate<T>(filter, updateDef);
            return result;
        }

        public T FindOneAndUpdate<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, FindOneAndUpdateOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);           
            var result = GetCollection<T>(stateContext).FindOneAndUpdate<T>(filter, updateDef, options);
            return result;
        }

        public UpdateResult UpdateMany<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class
        {   
            var result = GetCollection<T>(stateContext).UpdateMany<T>(expression, updateDef);
            return result;
        }    
        

        public async Task<UpdateResult> UpdateManyAsync<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class
        {                       
            var result = await GetCollection<T>(stateContext).UpdateManyAsync<T>(expression, updateDef);
            return result;
        }

        public ReplaceOneResult ReplaceOne<T>(T @object, Expression<Func<T, bool>> key, UpdateOptions options, IMongoDBStateContext stateContext = null) where T : class
        {

            var filter = Builders<T>.Filter.And(key);
            var result = GetCollection<T>(stateContext).ReplaceOne(filter, @object, options);
            return result;
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync<T>(T @object, Expression<Func<T, bool>> key, UpdateOptions options, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(key);
            var result = await GetCollection<T>(stateContext).ReplaceOneAsync(filter, @object, options);
            return result;
        }

        #endregion

        #region Remove/Delete Documents

        public DeleteResult DeleteOne<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {             
            var result = GetCollection<T>(stateContext).DeleteOne<T>(expression);            
            return result;
        }

        public async Task<DeleteResult> DeleteOneAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            var result =  await GetCollection<T>(stateContext).DeleteOneAsync<T>(expression);
            return result;
        }

        public DeleteResult DeleteMany<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            var result = GetCollection<T>(stateContext).DeleteMany<T>(expression);
            return result;
        }

        public async Task<DeleteResult> DeleteManyAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {            
            var result = await GetCollection<T>(stateContext).DeleteManyAsync<T>(expression);
            return result;
        }

        public T FindOneAndDelete<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            var result = GetCollection<T>(stateContext).FindOneAndDelete<T>(filter);
            return result;
        }

        public async Task<T> FindOneAndDeleteAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            var result = await GetCollection<T>(stateContext).FindOneAndDeleteAsync<T>(filter);
            return result;
        }
        public T FindOneAndDelete<T>(Expression<Func<T, bool>> expression, FindOneAndDeleteOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            var result = GetCollection<T>(stateContext).FindOneAndDelete<T>(filter, options);
            return result;
        }

        public async Task<T> FindOneAndDeleteAsync<T>(Expression<Func<T, bool>> expression, FindOneAndDeleteOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            var filter = Builders<T>.Filter.And(expression);
            var result = await GetCollection<T>(stateContext).FindOneAndDeleteAsync<T>(filter, options);
            return result;
        }

        #endregion       

        #endregion
    }
}
