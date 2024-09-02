using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Model.DTO.User
{
    public class UserDTO
    { 
        public string UserId { get; set; }
         

        /// <summary>
        /// 用户名
        /// </summary> 
        [Required(ErrorMessage ="用户名或密码必填")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary> 
        [Required(ErrorMessage = "用户名或密码必填")]
        public string Password { get; set; } 
       
    }
}
