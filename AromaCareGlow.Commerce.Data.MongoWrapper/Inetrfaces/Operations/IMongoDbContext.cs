using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using AromaCareGlow.Commerce.Data.MongoWrapper.Helpers;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Collection;

namespace AromaCareGlow.Commerce.Data.MongoWrapper.Interfaces.Operations
{
    public interface IMongoDbContext
    {

        #region Create/Add Document(s)
        StoreResult<T> InsertSingle<T>(T @object, IMongoDBStateContext stateContext = null) where T : class;
        Task<StoreResult<T>> InsertSingleAsync<T>(T @object, IMongoDBStateContext stateContext = null) where T : class;
        StoreResult<T> InsertMany<T>(IEnumerable<T> objects, IMongoDBStateContext stateContext = null) where T : class;
        Task<StoreResult<T>> InsertManyAsync<T>(IEnumerable<T> objects, IMongoDBStateContext stateContext = null) where T : class;
        #endregion

        #region Read/Get Document(s)
        T FindOne<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        Task<T> FindOneAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        IEnumerable<T> FindAll<T>(Expression<Func<T, bool>> expression, FindOptions<T> options, IMongoDBStateContext stateContext = null) where T : class;
        Task<IEnumerable<T>> FindAllAysnc<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        Task<IEnumerable<T>> FindAllAysnc<T>(Expression<Func<T, bool>> expression, FindOptions<T> options, IMongoDBStateContext stateContext = null) where T : class;
        P FindAndProjectOne<T, P>(Expression<Func<T, P>> projectionParameter, Expression<Func<T, bool>> searchParameters, IMongoDBStateContext stateContext = null) where T : class;
        List<P> FindAndProjectMany<T, P>(Expression<Func<T, P>> projectionParameter, Expression<Func<T, bool>> searchParameters, IMongoDBStateContext stateContext = null) where T : class;
        IEnumerable<T> FindUsingRegex<T>(string fieldName, string searchKey, RegexOptions options, IMongoDBStateContext stateContext = null) where T : class;
        List<P> FindAndProjectEmbeddedDocuments<T, P>(BsonDocument[] pipeline, IMongoDBStateContext stateContext = null) where T : class;

        #endregion

        #region Update Document(s)
        T FindOneAndUpdate<T, TField>(Expression<Func<T, TField>> expression, TField value, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class;
        T FindOneAndUpdate<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class;
        T FindOneAndUpdate<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, FindOneAndUpdateOptions<T> options, IMongoDBStateContext stateContext = null) where T : class;
        UpdateResult UpdateMany<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class;
        Task<UpdateResult> UpdateManyAsync<T>(Expression<Func<T, bool>> expression, UpdateDefinition<T> updateDef, IMongoDBStateContext stateContext = null) where T : class;
        ReplaceOneResult ReplaceOne<T>(T @object, Expression<Func<T, bool>> expression, UpdateOptions options, IMongoDBStateContext stateContext = null) where T : class;
        Task<ReplaceOneResult> ReplaceOneAsync<T>(T @object, Expression<Func<T, bool>> expression, UpdateOptions options, IMongoDBStateContext stateContext = null) where T : class;
        #endregion

        #region Remove/Delete Documents
        DeleteResult DeleteOne<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        Task<DeleteResult> DeleteOneAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        DeleteResult DeleteMany<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        Task<DeleteResult> DeleteManyAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        T FindOneAndDelete<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        Task<T> FindOneAndDeleteAsync<T>(Expression<Func<T, bool>> expression, IMongoDBStateContext stateContext = null) where T : class;
        T FindOneAndDelete<T>(Expression<Func<T, bool>> expression, FindOneAndDeleteOptions<T> options, IMongoDBStateContext stateContext = null) where T : class;
        Task<T> FindOneAndDeleteAsync<T>(Expression<Func<T, bool>> expression, FindOneAndDeleteOptions<T> options, IMongoDBStateContext stateContext = null) where T : class;

        #endregion

    }
}
