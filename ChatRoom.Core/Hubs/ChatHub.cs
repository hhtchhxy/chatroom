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
    /// <summary>
    /// Websocket chat interface
    /// </summary>
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
                info.RecordTime = DateTime.Now;
                LocalCacheHelper.MsgQueue.Enqueue(info);
            }
        }

        #region Send messages
        /// <summary>
        /// Send messages to all clients personally
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendToAll(string message)
        {
            string cid = GetConnectionId();
            await Clients.All.ReceiveMessage(new(cid, LocalCacheHelper.Connections[cid], message));
        }

        /// <summary>
        /// Send messages to all clients on behalf of the system
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendSysToAll(string message) => await Clients.All.ReceiveMessage(new(systemid, systemname, message));


        /// <summary>
        /// Send a message to the specified user (individual)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendToOne(string id, string message)
        {
            string cid = GetConnectionId();
            var sendUser = LocalCacheHelper.Connections[cid];
            var data = new TransData(cid, sendUser, message);
            TransMessageInfo(data, LocalCacheHelper.Connections[id]);
            await Clients.Client(id).ReceiveMessage(data);
        }
        /// <summary>
        /// Send group message (individual)
        /// </summary>
        /// <param name="group"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendToGroup(string group, string message)
        {
            string cid = GetConnectionId();
            await Clients.Group(group).ReceiveMessage(new(cid, LocalCacheHelper.Connections[cid], message));
        }
        /// <summary>
        /// Send group message (system)
        /// </summary>
        /// <param name="group"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendSysToGroup(string group, string message) => await Clients.Group(group).ReceiveMessage(new(systemid, systemname, message));

        #endregion

        #region SignalR User
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
        #endregion
        #region SignalR group
        /// <summary>
        /// Actively join the group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public async Task AddToGroup(string group)
        {
            string cid = GetConnectionId();
            await Groups.AddToGroupAsync(cid, group);
            await SendSysToGroup(group, $@"welcome{LocalCacheHelper.Connections[cid]}join");
        }

        /// <summary>
        /// Passive joining group
        /// </summary>
        /// <param name="group"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task AddToGrouped(string group, string id)
        {
            string cid = GetConnectionId();
            await Groups.AddToGroupAsync(id, group);
            await SendSysToGroup(group, $@"welcome{LocalCacheHelper.Connections[cid]}join");
        }
        #endregion

        #region 
        /// <summary>
        /// Add to online user set
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddUser(string name)
        {
            string cid = GetConnectionId();
            if (!LocalCacheHelper.Connections.ContainsKey(cid))
            {
                await Task.Run(() => LocalCacheHelper.Connections.Add(cid, name));
                await SendSysToAll("relst");
            }
        }

        /// <summary>
        /// Get online users
        /// </summary>
        /// <returns></returns>
        public object GetUser()
        {
            string cid = GetConnectionId();
            return LocalCacheHelper.Connections.Where(t => !t.Key.Equals(cid));
        }
        #endregion

        #region 
        /// <summary>
        /// Override Link Hook
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Override broken link hook
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string cid = GetConnectionId();
            LocalCacheHelper.Connections.Remove(cid);
            await SendSysToAll("relst");
            await base.OnDisconnectedAsync(exception);
        }
        #endregion

    }
}
