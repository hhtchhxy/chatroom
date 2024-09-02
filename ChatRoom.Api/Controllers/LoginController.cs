using ChatRoom.Core;
using ChatRoom.Core.CustomException;
using ChatRoom.Model.DTO.User;
using ChatRoom.Service.User.IUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatRoom.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            this._loginService = loginService;
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserDTO dto)
        {
            if (dto == null)
            {
                throw new CustomException("请求参数错误");
            }
            //TODO 验证码、错误次数、限流等限制
            var flag = await _loginService.Register(dto);
            if (!flag)
            {
                throw new CustomException("注册失败");
            }
            return Ok(ApiResult.Success("注册成功！"));
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginBody"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [Route("login")]
        [HttpPost] 
        public async Task<IActionResult> Login([FromBody] UserDTO loginBody)
        {
            if (loginBody == null) { throw new CustomException("请求参数错误"); }
            //TODO 验证码、错误次数、限流等限制
            var flag = await _loginService.Login(loginBody);
            return Ok(flag ? ApiResult.Success("登录成功！", "token"): ApiResult.Error("登录失败！"));
        }
    }
}
