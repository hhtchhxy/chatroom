using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace ChatRoom.Core.SqlSuger
{
    public static class SugerDBExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="set"></param>
        public static void AddSqlSugarSetup(this IServiceCollection services, string DbConnection)
        {
            var sqlSugar = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = DbConnection,
                IsAutoCloseConnection = true,
            },
             db => {  /***写AOP等方法***/});
            ISugarUnitOfWork<SugarUnitOfWork> context = new SugarUnitOfWork<SugarUnitOfWork>(sqlSugar);
            services.AddSingleton(context);
        }
    }
}
