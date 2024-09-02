using ChatRoom.Repository.RepositoryBase.IRepositoryService;
using ChatRoom.Repository.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatRoom.Model.Models.Chat;

namespace ChatRoom.Repository.Chat.IRepository
{
    public interface IMessageRepositoryService : IBaseRepository<MessageInfoDO>
    {
    }
}
