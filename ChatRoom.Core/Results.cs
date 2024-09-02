using ChatRoom.Core.CustomException;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core
{
    public class ApiResult
    {
        public int Code { get; set; }
        public string Msg { get; set; }

        public object Data { get; set; }

        /// <summary>
        /// 初始化一个新创建的APIResult对象，使其表示一个空消息
        /// </summary>
        public ApiResult()
        {
        }

        /// <summary>
        /// 初始化一个新创建的 ApiResult 对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public ApiResult(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }

        /// <summary>
        /// 初始化一个新创建的 ApiResult 对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
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
        /// 返回成功消息
        /// </summary>
        /// <returns></returns>
        public ApiResult Success()
        {
            Code = (int)ResultCode.SUCCESS;
            Msg = "ok";
            return this;
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns>成功消息</returns>
        public static ApiResult Success(object data)
        {
            return new ApiResult((int)ResultCode.SUCCESS, "ok", data);
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <returns>成功消息</returns>
        public static ApiResult Success(string msg)
        {
            return new ApiResult((int)ResultCode.SUCCESS, msg, null);
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <param name="data">数据对象</param>
        /// <returns>成功消息</returns>
        public static ApiResult Success(string msg, object data)
        {
            return new ApiResult((int)ResultCode.SUCCESS, msg, data);
        }


        public ApiResult Error(ResultCode resultCode, string msg = "")
        {
            Code = (int)resultCode;
            Msg = msg;
            return this;
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResult Error(int code, string msg)
        {
            return new ApiResult(code, msg);
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResult Error(string msg)
        {
            return new ApiResult((int)ResultCode.ERROR, msg);
        }
    }
}
