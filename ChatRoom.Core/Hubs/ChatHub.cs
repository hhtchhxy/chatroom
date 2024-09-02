using ChatRoom.Core.Cache;
using ChatRoom.Model.Models.Chat;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChatRoom.Core.Hubs.IChatClient;

namespace ChatRoom.Core.Hubs
{
    public interface IChatClient
    {
        public record TransData(string Id, string User, string Message);
        Task ReceiveMessage(TransData data);
    }

    public class ChatHub : Hub<IChatClient>
    {
        private readonly string systemid = "system";
        private readonly string systemname = "system";

        private void TransMessageInfo(IChatClient.TransData data, string recipient)
        {
            if (data != null)
            {
                MessageInfoDO info = new MessageInfoDO();
                info.Recipient = recipient;
                info.Sender = data.User;
                info.Content = data.Message;
                info.RecordTime=DateTime.Now;
                LocalCacheHelper.msgQueue.Enqueue(info);
            }
        }

        #region 发送消息


        /// <summary>
        /// 以系统名义向所有客户端发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendSysToAll(string message) => await Clients.All.ReceiveMessage(new(systemid, systemname, message));


        /// <summary>
        /// 发送消息给指定用户(个人)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendToOne(string id, string message)
        {
            string cid = GetConnectionId();
            var sendUser = LocalCacheHelper.connections[cid];
            var data = new TransData(cid, sendUser, message);
            TransMessageInfo(data, LocalCacheHelper.connections[id]);
            await Clients.Client(id).ReceiveMessage(data);
        }


        #endregion

        #region SignalR用户
        /// <summary>
        /// 获取连接的唯一 ID
        /// </summary>
        /// <returns></returns>
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        #endregion


        #region 临时用户操作
        /// <summary>
        /// 添加到在线用户集
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUser(string name)
        {
            string cid = GetConnectionId();
            if (!LocalCacheHelper.connections.ContainsKey(cid))
            {
                await Task.Run(() => LocalCacheHelper.connections.Add(cid, name));
                await SendSysToAll("relst");
            }
        }

        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <returns></returns>
        public object GetUser()
        {
            string cid = GetConnectionId();
            return LocalCacheHelper.connections.Where(t => !t.Key.Equals(cid));
        }
        #endregion

        #region 重写连接断开钩子
        /// <summary>
        /// 重写链接钩子
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 重写断开链接钩子
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string cid = GetConnectionId();
            LocalCacheHelper.connections.Remove(cid);
            await SendSysToAll("relst");
            await base.OnDisconnectedAsync(exception);
        }
        #endregion

    }
}
