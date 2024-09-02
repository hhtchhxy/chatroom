using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.SqlSuger
{
    /// <summary>
    /// 查询条件公用基类
    /// </summary>
    public class SearchCondition
    {
         
   
        /// <summary>
        /// 页面索引
        /// </summary>
        public int page
        {
            get;
            set;
        }

        /// <summary>
        /// 页面大小
        /// </summary>
        public int size
        {
            get;
            set;
        } = 10;

        /// <summary>
        /// 如果要获取总数，请设置-1
        /// </summary>
        public long total
        {
            get;
            set;
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string entity
        {
            get;
            set;
        }

        /// <summary>
        /// 条件
        /// </summary>
        public string condi
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string by
        {
            get;
            set;
        }

        /// <summary>
        /// 要查询字段
        /// </summary>
        public string feild
        {
            get;
            set;
        }
    } 
}
