using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.CustomExceptions
{
     
    public enum ResultCode
    {
        [Description("success")]
        SUCCESS = 200,

        [Description("system exception")]
        ERROR = 500, 

        [Description("forbidden")]
        FORBIDDEN = 403,

        [Description("Bad Request")]
        BAD_REQUEST = 400
    } 
}
