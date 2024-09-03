using ChatRoom.Model.Models.Chat;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.Cache
{
    public class LocalCacheHelper
    {
        public readonly static Dictionary<string, string> Connections = new();
        public readonly static ConcurrentQueue<MessageInfoDO> MsgQueue = new ConcurrentQueue<MessageInfoDO>();
        public static void SetCache(string key,object value)
        {
            //TODO Cache
        }
        public static object GetCache(string key)
        {
            //TODO 
            return "ab12";
        }

    }
}
