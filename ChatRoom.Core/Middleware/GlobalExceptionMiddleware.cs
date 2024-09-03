using ChatRoom.Core.Extension;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using ChatRoom.Core.CustomExceptions;

namespace ChatRoom.Core.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        /// <summary>
        /// Capture global exceptions and record non custom exception logs
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {

            int code = (int)ResultCode.ERROR;
            string msg;
            string error = string.Empty; 
            if (ex is CustomException customException)
            {
                code = customException.Code == 0 ? (int)ResultCode.SUCCESS : (int)ResultCode.ERROR;
                msg = customException.Message;
            }
            else
            {
                code = (int)ResultCode.ERROR;
                //msg = ex.Message;
                msg = "system exception";
                string ip = HttpContextExtension.GetClientUserIp(context);
                _logger.LogError(code, $"ip:{ip};msg:{msg}");
            }
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            ApiResult apiResult = new(code, msg);
            string responseResult = JsonSerializer.Serialize(apiResult, options).ToLower();
            context.Response.ContentType = "text/json;charset=utf-8";
            await context.Response.WriteAsync(responseResult, Encoding.UTF8);
        } 
    }
}