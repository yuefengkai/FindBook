using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace User.Api.Filters
{
    /// <summary>
    /// 全局异常处理过滤
    /// </summary>
    public class GlobalExceptionFilter:IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<GlobalExceptionFilter> _iLogger;
        public GlobalExceptionFilter(IHostingEnvironment env,ILogger<GlobalExceptionFilter> iLogger)
        {
            _env = env;
            _iLogger = iLogger;
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                
                if (_env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception.StackTrace;
                }
              
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                json.Message = "发生了内部未知错误";
                
                if (_env.IsDevelopment())
                {
                    json.DeveloperMessage = context.Exception.StackTrace;
                }

                context.Result = new InternalServerErrorObjectResult(json);
            }
            
            _iLogger.LogError(context.Exception,context.Exception.Message);
            
            context.ExceptionHandled = true;
        }
    }

    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
