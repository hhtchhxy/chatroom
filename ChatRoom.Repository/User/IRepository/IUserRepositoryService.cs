using ChatRoom.Core.SerivceExtention;
using ChatRoom.Model.Models.User;
using ChatRoom.Repository.RepositoryBase;
using ChatRoom.Repository.RepositoryBase.IRepositoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Repository.User.IRepository
{
    public interface IUserRepositoryService:IBaseRepository<UserDO>,IBaseDomain
    {
    }
}
