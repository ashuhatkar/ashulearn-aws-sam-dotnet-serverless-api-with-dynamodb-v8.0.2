/*--****************************************************************************
 --* Project Name    : aws-sam-dotnet-serverless-api-with-dynamodb
 --* Reference       : Startup references
 --* Description     : Program class
 --* Configuration Record
 --* Review            Ver  Author           Date      Cr       Comments
 --* 001               001  A HATKAR         15/12/24  CR-XXXXX Original
 --****************************************************************************/
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using ServerlessAPI;
using ServerlessAPI.Repositories;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//Logger
builder.Logging
    .ClearProviders()
    .AddJsonConsole();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

//string region = Environment.GetEnvironmentVariable("AWS_REGION") ?? RegionEndpoint.APSoutheast2.SystemName;

// Add aws dynamoDb services
var accessKey = builder.Configuration.GetSection("AWS:AccessKey").Value;
var secretKey = builder.Configuration.GetSection("AWS:SecretKey").Value;
var credentials = new BasicAWSCredentials(accessKey, secretKey);
var config = new AmazonDynamoDBConfig()
{
    RegionEndpoint = Amazon.RegionEndpoint.APSoutheast2
};
var client = new AmazonDynamoDBClient(credentials, config);

builder.Services
    //.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(RegionEndpoint.GetBySystemName(region)))
    .AddSingleton<IAmazonDynamoDB>(client)
    .AddScoped<IDynamoDBContext, DynamoDBContext>()
    .AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

// Add AWS Lambda support. When running the application as an AWS Serverless application, Kestrel is replaced
// with a Lambda function contained in the Amazon.Lambda.AspNetCoreServer package, which marshals the request into the ASP.NET Core hosting framework.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();
