using ChatRoom.Core;
using ChatRoom.Core.Cache;
using ChatRoom.Core.CustomExceptions;
using ChatRoom.Model.DTO.User;
using ChatRoom.Service.User.IUser;
using Hei.Captcha;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ChatRoom.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly SecurityCodeHelper _securityCode;


        public LoginController(ILoginService loginService, SecurityCodeHelper securityCode)
        {
            this._loginService = loginService;
            this._securityCode = securityCode;
        }
        /// <summary>
        /// user register
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
                throw new CustomException("Request parameter error");
            }
            //TODO Restrictions such as verification code, error count, and rate limit
            var flag = await _loginService.Register(dto);
            if (!flag)
            {
                throw new CustomException("fail to register");
            }
            return Ok(ApiResult.Success("registered successfully！"));
        }
        /// <summary>
        /// user login
        /// </summary>
        /// <param name="loginBody"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        [Route("login")]
        [HttpPost] 
        public async Task<IActionResult> Login([FromBody] UserDTO loginBody)
        {
            if (loginBody == null) { throw new CustomException("Request parameter error"); }
            if (!string.IsNullOrEmpty(loginBody.UUID))
            {
                var cacheCode = LocalCacheHelper.GetCache(loginBody.UUID);
                if (string.IsNullOrEmpty(loginBody.UUID)|| cacheCode == null || cacheCode!= loginBody.VerificationCode)
                {
                    return Ok(ApiResult.Error("Invalid verification code"));
                }
                }
            //TODO Restrictions such as verification code, error count, rate limit,entry log,abnormal login reminder
            var flag = await _loginService.Login(loginBody);
            return Ok(flag ? ApiResult.Success("login successfully", "token"): ApiResult.Error("login failure"));
        }
        /// <summary>
        /// generate verification code
        /// </summary>
        /// <returns></returns>
        [HttpGet("generatecode")]
        public IActionResult CaptchaImage()
        {
            string uuid = Guid.NewGuid().ToString().Replace("-", ""); 
            var code = _securityCode.GetRandomEnDigitalText(4);
            byte[]  imgByte = _securityCode.GetEnDigitalCodeByte(code);
             
            string base64Str = Convert.ToBase64String(imgByte);
            LocalCacheHelper.SetCache(uuid, code);
            var obj = new { uuid, img = base64Str };
            return Ok(ApiResult.Success("",obj));
        }
    }
}
