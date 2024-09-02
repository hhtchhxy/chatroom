using ChatRoom.Model.DTO.User;
using ChatRoom.Model.Models.User;
using ChatRoom.Repository.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Service.User.IUser
{
    public interface ILoginService : IBaseDomain
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<bool> Register(UserDTO dto);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<bool> Login(UserDTO dto);
    }
}
