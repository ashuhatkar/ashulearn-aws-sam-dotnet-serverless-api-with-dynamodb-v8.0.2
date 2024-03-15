/*--****************************************************************************
 --* Project Name    : aws-sam-dotnet-serverless-api-with-dynamodb
 --* Reference       : Amazon.DynamoDBv2.DataModel
 --* Description     : Base class for domain entities
 --* Configuration Record
 --* Review            Ver  Author           Date      Cr       Comments
 --* 001               001  A HATKAR         15/11/24  CR-XXXXX Original
 --****************************************************************************/
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