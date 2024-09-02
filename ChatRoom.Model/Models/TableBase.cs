using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Model.Models
{
    public class TableBase
    {
        [SugarColumn(IsOnlyIgnoreUpdate = true, IsNullable = true, ColumnDescription = "创建用户", ColumnName = "create_by")]      
        [JsonIgnore]
        public string CreateBy { get; set; }

        [SugarColumn(IsOnlyIgnoreUpdate = true, IsNullable = true, ColumnDescription = "创建时间", ColumnName = "create_time")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [JsonIgnore]
        [SugarColumn(IsOnlyIgnoreInsert = true, IsNullable = true, ColumnDescription = "修改用户", ColumnName = "update_by")]
        public string UpdateBy { get; set; }

        //[JsonIgnore] 
        [SugarColumn(IsOnlyIgnoreInsert = true, IsNullable = true, ColumnDescription = "修改时间", ColumnName = "update_time")]
        public DateTime? UpdateTime { get; set; }
    }
}
