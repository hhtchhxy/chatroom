using ChatRoom.Core.CustomException;
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

        public Task<bool> Login(UserDTO dto)
        {

            //if (dto.UserName == null|| dto.Password==null)
            //{
            //    throw new CustomException("请输入用户名和密码");
            //}
            var userInfo = _userRepositoryService.QueryOne(t => t.UserName == dto.UserName);
            if (userInfo == null)
            {
                throw new CustomException("用户名或密码错误");
            }
            string pdw = CommonTools.ComputeMD5(dto.Password + userInfo.Salt);
            if (pdw != userInfo.Password)
            {
                throw new CustomException("用户名或密码错误");
            }

            userInfo.OnlineStatus = 1;
            var flag = _userRepositoryService.Update(userInfo);
            return Task.FromResult(flag);
        }

        public Task<bool> Register(UserDTO dto)
        {
            var userInfo = _userRepositoryService.QueryOne(t => t.UserName == dto.UserName);
            if (userInfo != null)
            {
                throw new CustomException("用户名已存在");
            }
            if (!CommonTools.CheckPassword(dto.Password))
            {
                throw new CustomException("密码强度不符合要求");
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
