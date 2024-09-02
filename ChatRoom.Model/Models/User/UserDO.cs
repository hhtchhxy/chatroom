using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatRoom.Model.Models.User
{
    [SugarTable("tb_user")]
    public class UserDO:TableBase
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [JsonConverter(typeof(ValueToStringConverter))]
        [SugarColumn(ColumnName = "user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(ColumnDescription = "用户名", ColumnName = "user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonIgnore] 
        public string Password { get; set; }
        /// <summary>
        /// 密码混淆Salt
        /// </summary>
        [JsonIgnore]
        public string Salt { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(ColumnDescription = "昵称", ColumnName = "nickname")]
        public string NickName { get; set; }
        
        /// <summary>
        /// 电话号码
        /// </summary>
        [SugarColumn(ColumnDescription = "电话号码", IsNullable = true, ColumnName = "phone")]
        public string Phone { get; set; }
         

        /// <summary>
        /// 登录状态(1,在线 | 0,离线)
        /// </summary>
        [SugarColumn(ColumnDescription = "登录状态(1,在线 | 0,离线)", ColumnName = "online_status")]
        public int? OnlineStatus { get; set; }
    }
}
