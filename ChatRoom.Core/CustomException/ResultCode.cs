using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.CustomException
{
     
    public enum ResultCode
    {
        [Description("success")]
        SUCCESS = 200,

        [Description("系统异常")]
        ERROR = 500, 

        [Description("forbidden")]
        FORBIDDEN = 403,

        [Description("Bad Request")]
        BAD_REQUEST = 400
    } 
}
