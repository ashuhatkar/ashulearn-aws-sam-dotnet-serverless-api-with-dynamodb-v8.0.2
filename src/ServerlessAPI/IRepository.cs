/*--****************************************************************************
 --* Project Name    : aws-sam-dotnet-serverless-api-with-dynamodb
 --* Reference       : ServerlessAPI.Domain
 --* Description     : IRepository
 --* Configuration Record
 --* Review            Ver  Author           Date      Cr       Comments
 --* 001               001  A HATKAR         15/11/24  CR-XXXXX Original
 --****************************************************************************/
namespace ServerlessAPI;

/// <summary>
/// Represents an entity repository
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public partial interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// List book from DynamoDb Table with items limit (default=10)
    /// </summary>
    /// <param name="limit">limit (default=10)</param>
    /// <returns>Collection of books</returns>
    Task<IList<TEntity>> GetBooksAsync(int limit = 10);

    /// <summary>
    /// Get book by PK
    /// </summary>
    /// <param name="id">book`s PK</param>
    /// <returns>Book object</returns>
    Task<TEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Include new book to the DynamoDB Table
    /// </summary>
    /// <param name="entity">Entity to include</param>
    /// <returns>success/failure</returns>
    Task<bool> CreateAsync(TEntity entity);

    /// <summary>
    /// Update book content
    /// </summary>
    /// <param name="entity">Entity to be updated</param>
    /// <returns></returns>
    Task<bool> UpdateAsync(TEntity entity);

    /// <summary>
    /// Remove existing book from DynamoDB Table
    /// </summary>
    /// <param name="entity">Entity to remove</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(TEntity entity);
}