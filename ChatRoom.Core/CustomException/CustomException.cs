using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.CustomException
{
    public class CustomException: Exception
    {
        public int Code { get; set; }
        public string Msg { get; set; } 

        public CustomException(string msg) : base(msg)
        {
        }
        public CustomException(int code, string msg) : base(msg)
        {
            Code = code;
            Msg = msg;
        }

        public CustomException(ResultCode resultCode, string msg) : base(msg)
        {
            Code = (int)resultCode;
        } 
    }
}
