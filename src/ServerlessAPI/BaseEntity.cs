using Amazon.DynamoDBv2.DataModel;

namespace ServerlessAPI
{
    /// <summary>
    /// Represents the base class for domain entities
    /// </summary>
    public abstract partial class BaseEntity
    {

        /// <summary>
        /// Map c# types to DynamoDb Columns 
        /// to learn more visit https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/MidLevelAPILimitations.SupportedTypes.html
        /// Gets or sets guid identifier
        /// </summary>
        [DynamoDBHashKey] //Partition key
        public Guid Id { get; set; } = Guid.Empty;
    }
}