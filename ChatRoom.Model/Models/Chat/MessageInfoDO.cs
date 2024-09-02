using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Model.Models.Chat
{
    [SugarTable("tb_message")]
    public class MessageInfoDO
    { 
         
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnName = "id")]
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Content { get; set; }

        [SugarColumn(ColumnDescription = "发送时间", ColumnName = "record_time")]
        public DateTime RecordTime { get; set; }
    }
}
