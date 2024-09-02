using ChatRoom.Model.Models.User;
using ChatRoom.Repository.RepositoryBase.RepositoryService;
using ChatRoom.Repository.User.IRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Repository.User.Repository
{
    public class UserRepositoryService : BaseRepository<UserDO>, IUserRepositoryService
    {
        public UserRepositoryService(ISugarUnitOfWork<SugarUnitOfWork> unitOfWork) : base(unitOfWork)
        {
        }

    }
}
