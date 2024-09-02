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
        public readonly static Dictionary<string, string> connections = new();
        public readonly static ConcurrentQueue<MessageInfoDO> msgQueue = new ConcurrentQueue<MessageInfoDO>();

    }
}
