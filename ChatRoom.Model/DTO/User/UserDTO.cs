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
        /// Username
        /// </summary> 
        [Required(ErrorMessage = "Username required")]
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary> 
        [Required(ErrorMessage = "Password required")]
        public string Password { get; set; }
        /// <summary>
        /// Verification code
        /// </summary>

        public string VerificationCode { get; set; } = "ab12";
        public string UUID { get; set; }

    }
}
