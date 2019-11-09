using AromaCareGlow.Commerce.Data.MongoWrapper.Helpers;
using AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Client;
using AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Collection;
using AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Operations;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AromaCareGlow.Commerce.Data.MongoWrapper
{
    public class Document : IDocument
    {
        private readonly IMongoDbContext context;

        public Document(IMongoDbContext contextDefault)
        {
            context = contextDefault;
        }

        #region Create/Add Document(s)

        public StoreResult<T> AddOne<T>(T @object, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.InsertSingle<T>(@object, stateContext);
        }

        public async Task<StoreResult<T>> AddOneAsync<T>(T @object, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.InsertSingleAsync<T>(@object, stateContext);
        }

        public StoreResult<T> AddMany<T>(IEnumerable<T> objects, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.InsertMany(objects, stateContext);
        }

        public async Task<StoreResult<T>> AddManyAsync<T>(IEnumerable<T> objects, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.InsertManyAsync(objects, stateContext);
        }

        #endregion

        #region Read/Get Document(s)
        public T GetOne<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindOne<T>(expression, stateContext);
        }

        public async Task<T> GetOneAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.FindOneAsync<T>(expression, stateContext);
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindAll<T>(expression, stateContext);
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> expression, FindOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindAll(expression, options, stateContext);
        }

        public async Task<IEnumerable<T>> GetAllAysnc<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.FindAllAysnc<T>(expression, stateContext);
        }

        public async Task<IEnumerable<T>> GetAllAysnc<T>(Expression<Func<T, bool>> expression, FindOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.FindAllAysnc<T>(expression, options, stateContext);
        }


        public P GetAndProjectOne<T, P>(Expression<Func<T, P>> projectionParameter, Expression<Func<T, bool>> searchParameters, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindAndProjectOne<T, P>(projectionParameter, searchParameters, stateContext);
        }

        public List<P> GetAndProjectMany<T, P>(Expression<Func<T, P>> projectionParameter, Expression<Func<T, bool>> searchParameters, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindAndProjectMany<T, P>(projectionParameter, searchParameters, stateContext);
        }

        public IEnumerable<T> GetUsingRegex<T>(string fieldName, string searchKey, RegexOptions options, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindUsingRegex<T>(fieldName, searchKey, options, stateContext);
        }

        public List<P> GetAndProjectEmbeddedDocuments<T, P>(BsonDocument[] pipeline, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindAndProjectEmbeddedDocuments<T, P>(pipeline, stateContext);
        }

        #endregion

        #region Update Document(s)

        public T FindOneAndUpdate<T, TField>(Expression<Func<T, TField>> expression, TField value, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindOneAndUpdate<T, TField>(expression, value, updateDef, stateContext);
        }

        public T FindOneAndUpdate<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindOneAndUpdate<T>(expression, updateDef, stateContext);
        }

        public T FindOneAndUpdate<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, FindOneAndUpdateOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindOneAndUpdate<T>(expression, updateDef, options, stateContext);
        }

        public UpdateResult UpdateMany<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.UpdateMany<T>(expression, updateDef, stateContext);
        }


        public async Task<UpdateResult> UpdateManyAsync<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.UpdateManyAsync<T>(expression, updateDef, stateContext);
        }

        public ReplaceOneResult ReplaceOne<T>(T @object, Expression<Func<T, bool>> expression, UpdateOptions options, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.ReplaceOne<T>(@object, expression, options, stateContext);
        }

        public async Task<ReplaceOneResult> ReplaceOneAsync<T>(T @object, Expression<Func<T, bool>> expression, UpdateOptions options, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.ReplaceOneAsync<T>(@object, expression, options, stateContext);
        }

        #endregion

        #region Remove/Delete Document(s)

        public DeleteResult DeleteOne<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.DeleteOne<T>(expression, stateContext);
        }

        public async Task<DeleteResult> DeleteOneAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.DeleteOneAsync<T>(expression, stateContext);
        }

        public DeleteResult DeleteMany<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.DeleteMany<T>(expression, stateContext);
        }

        public async Task<DeleteResult> DeleteManyAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.DeleteManyAsync<T>(expression, stateContext);
        }

        public T FindOneAndDelete<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindOneAndDelete<T>(expression, stateContext);
        }

        public async Task<T> FindOneAndDeleteAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.FindOneAndDeleteAsync<T>(expression, stateContext);
        }
        public T FindOneAndDelete<T>(Expression<Func<T, bool>> expression, FindOneAndDeleteOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            return context.FindOneAndDelete<T>(expression, options, stateContext);
        }

        public async Task<T> FindOneAndDeleteAsync<T>(Expression<Func<T, bool>> expression, FindOneAndDeleteOptions<T> options, IMongoDBStateContext stateContext = null) where T : class
        {
            return await context.FindOneAndDeleteAsync<T>(expression, options, stateContext);
        }

        #endregion     

    }
}
