/*--****************************************************************************
--* Project Name    : aws-sam-dotnet-serverless-api-with-dynamodb
--* Reference       : Amazon.DynamoDBv2.DataModel
--* Description     : Book model
--* Configuration Record
--* Review            Ver  Author           Date      Cr       Comments
--* 001               001  A HATKAR         20/06/24  CR-XXXXX Original
--****************************************************************************/
using Amazon.DynamoDBv2.DataModel;

namespace ServerlessAPI.Domain;

// <summary>
/// Map the Book Class to DynamoDb Table
/// To learn more visit https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DeclarativeTagsList.html
/// </summary>
[DynamoDBTable("aws-sam-dotnet-lambdaBookCatalog")]
public partial class Book : BaseEntity
{
    /// <summary>
    /// Gets or sets the title
    /// </summary>
    [DynamoDBProperty]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the isbn
    /// </summary>
    [DynamoDBProperty]
    public string? ISBN { get; set; }

    /// <summary>
    /// Gets or sets the list of authors
    /// </summary>
    [DynamoDBProperty] //String Set datatype
    public List<string>? Authors { get; set; }

    /// <summary>
    /// Gets or sets the coverpage
    /// </summary>
    [DynamoDBIgnore]
    public string? CoverPage { get; set; }
}