using ChatRoom.Model.Models.Chat;
using ChatRoom.Model.Models.User;
using ChatRoom.Repository.Chat.IRepository;
using ChatRoom.Repository.RepositoryBase.RepositoryService;
using ChatRoom.Repository.User.IRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Repository.Chat.Repository
{
    public class MessageRepositoryService : BaseRepository<MessageInfoDO>, IMessageRepositoryService
    {
        public MessageRepositoryService(ISugarUnitOfWork<SugarUnitOfWork> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
