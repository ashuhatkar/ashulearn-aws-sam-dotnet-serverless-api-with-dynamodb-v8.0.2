/*--****************************************************************************
 --* Project Name    : aws-sam-dotnet-serverless-api-with-dynamodb
 --* Reference       : Amazon.DynamoDBv2.DataModel
 --*                   Amazon.DynamoDBv2.DocumentModel
 --*                   ServerlessAPI.Domain
 --* Description     : Represents a entity repository
 --* Configuration Record
 --* Review            Ver  Author           Date      Cr       Comments
 --* 001               001  A HATKAR         15/11/23  CR-XXXXX Original
 --****************************************************************************/
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using ServerlessAPI.Domain;

namespace ServerlessAPI.Repositories;

/// <summary>
/// Represents the entity repository implementation
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public partial class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    #region Fields

    private readonly IDynamoDBContext _dbContext;
    private readonly ILogger<EntityRepository<TEntity>> _logger;

    #endregion

    #region Ctor

    public EntityRepository(IDynamoDBContext dbContext,
        ILogger<EntityRepository<TEntity>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get all entity entries
    /// </summary>
    /// <param name="limit"></param>
    /// <returns>The task result contains the entity entries</returns>
    public virtual async Task<IList<TEntity>> GetBooksAsync(int limit = 10)
    {
        var result = new List<TEntity>();

        try
        {
            if (limit <= 0)
            {
                return result;
            }

            var filter = new ScanFilter();
            filter.AddCondition("Id", ScanOperator.IsNotNull);

            var scanConfig = new ScanOperationConfig()
            {
                Limit = limit,
                Filter = filter
            };

            var queryResult = _dbContext.FromScanAsync<TEntity>(scanConfig);

            do
            {
                result.AddRange(await queryResult.GetNextSetAsync());
            }
            while (!queryResult.IsDone && result.Count < limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to list books from DynamoDb Table");
            return new List<TEntity>();
        }

        return result;
    }

    /// <summary>
    /// Get the entity entry
    /// </summary>
    /// <param name="id">Book identifier</param>
    /// <returns>The task result contains the entity entry</returns>
    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            return await _dbContext.LoadAsync<TEntity>(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to update book from DynamoDb Table");
            return null;
        }
    }

    /// <summary>
    /// Create the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>The task that represents the asynchronous operation</returns>
    public virtual async Task<bool> CreateAsync(TEntity entity)
    {
        try
        {
            entity.Id = Guid.NewGuid();
            await _dbContext.SaveAsync(entity);
            _logger.LogInformation("Book {} is added", entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to persist to DynamoDb Table");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Update the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (entity == null) return false;

        try
        {
            await _dbContext.SaveAsync(entity);
            _logger.LogInformation("Book {Id} is updated", entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to update book from DynamoDb Table");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Delete the entity entry
    /// </summary>
    /// <param name="entity">Entity entry</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        bool result;
        try
        {
            // Delete the entity.
            await _dbContext.DeleteAsync<Book>(entity.Id);

            // Try to retrieve deleted book. It should return null.
            TEntity deletedBook = await _dbContext.LoadAsync<TEntity>(entity.Id, new DynamoDBContextConfig
            {
                ConsistentRead = true
            });

            result = deletedBook == null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to delete book from DynamoDb Table");
            result = false;
        }

        if (result) _logger.LogInformation("Book {Id} is deleted", entity);

        return result;
    }

    #endregion
}