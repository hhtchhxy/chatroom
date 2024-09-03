using ChatRoom.Core.CustomExceptions;
using ChatRoom.Core.Tools;
using ChatRoom.Model.DTO.User;
using ChatRoom.Model.Models.User;
using ChatRoom.Repository.User.IRepository;
using ChatRoom.Service.User.IUser;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Service.User.UserService
{
    internal class LoginService : ILoginService
    {
        private readonly IUserRepositoryService _userRepositoryService;
        private readonly ILogger<LoginService> _logger;

        public LoginService(IUserRepositoryService userRepositoryService, ILogger<LoginService> logger)
        {
            this._userRepositoryService = userRepositoryService;
            this._logger = logger;
        }
        /// <summary>
        /// user register
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public Task<bool> Login(UserDTO dto)
        { 
            var userInfo = _userRepositoryService.QueryOne(t => t.UserName == dto.UserName);
            if (userInfo == null)
            {
                throw new CustomException("The user name or password is incorrect");
            }
            string pdw = CommonTools.ComputeMD5(dto.Password + userInfo.Salt);
            if (pdw != userInfo.Password)
            {
                throw new CustomException("The user name or password is incorrect");
            }

            userInfo.OnlineStatus = 1;
            var flag = _userRepositoryService.Update(userInfo);
            return Task.FromResult(flag);
        }
        /// <summary>
        /// user login
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public Task<bool> Register(UserDTO dto)
        {
            var userInfo = _userRepositoryService.QueryOne(t => t.UserName == dto.UserName);
            if (userInfo != null)
            {
                throw new CustomException("The user name already exists");
            }
            if (!CommonTools.CheckPassword(dto.Password))
            {
                throw new CustomException("The password strength does not meet requirements");
            }

            UserDO user = new()
            {
                CreateTime = DateTime.Now,
                CreateBy = dto.UserId,
                UserName = dto.UserName,
                UserId = CommonTools.CreateID(),
                Salt = CommonTools.GenerateCode(8),
                OnlineStatus = 1
            };
            user.Password = CommonTools.ComputeMD5(dto.Password + user.Salt);
            user.Id = _userRepositoryService.InsertIdentity(user);
            return Task.FromResult(user.Id > 0);

        }
    }
}
