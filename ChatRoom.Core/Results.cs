using ChatRoom.Core.CustomExceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core
{
    /// <summary>
    /// Unified interface return entity
    /// </summary>
    public class ApiResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }

        public object Data { get; set; }
         
        public ApiResult()
        {
        }
         
        public ApiResult(int code, string msg)
        {
            Code = code;
            Msg = msg;
        } 
        public ApiResult(int code, string msg, object data)
        {
            Code = code;
            Msg = msg;
            if (data != null)
            {
                Data = data;
            }
        }

        /// <summary>
        /// return success
        /// </summary>
        /// <returns></returns>
        public ApiResult Success()
        {
            Code = (int)ResultCode.SUCCESS;
            Msg = "ok";
            return this;
        }
         
        public static ApiResult Success(object data)
        {
            return new ApiResult((int)ResultCode.SUCCESS, "ok", data);
        }
         
        public static ApiResult Success(string msg)
        {
            return new ApiResult((int)ResultCode.SUCCESS, msg, null);
        }
         
        public static ApiResult Success(string msg, object data)
        {
            return new ApiResult((int)ResultCode.SUCCESS, msg, data);
        }
        /// <summary> 
        /// return error info
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ApiResult Error(ResultCode resultCode, string msg = "")
        {
            Code = (int)resultCode;
            Msg = msg;
            return this;
        }
         
        public static ApiResult Error(int code, string msg)
        {
            return new ApiResult(code, msg);
        }
        
        public static ApiResult Error(string msg)
        {
            return new ApiResult((int)ResultCode.ERROR, msg);
        }
    }
}
