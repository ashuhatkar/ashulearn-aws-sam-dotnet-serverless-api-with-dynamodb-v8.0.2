/*--****************************************************************************
--* Project Name    : aws-sam-dotnet-serverless-api-with-dynamodb
--* Reference       : Microsoft.AspNetCore.Mvc
--* Description     : Base api controller
--* Configuration Record
--* Review            Ver  Author           Date      Cr       Comments
--* 001               001  A HATKAR         20/06/24  CR-XXXXX Original
--****************************************************************************/
using Microsoft.AspNetCore.Mvc;

namespace ServerlessAPI.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public abstract partial class BaseApiController : ControllerBase
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}