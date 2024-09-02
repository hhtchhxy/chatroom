using ChatRoom.Repository.RepositoryBase.IRepositoryService;
using ChatRoom.Repository.RepositoryBase.RepositoryService;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using ChatRoom.Repository.RepositoryBase;

namespace ChatRoom.Api.SerivceExtention
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoInject(this IServiceCollection services, string ServiceName)
        { 
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var arrDll = new List<string> { $"{ServiceName}.Service.dll", $"{ServiceName}.Repository.dll" };
            arrDll.ForEach(d =>
            {
                var allTypes = Directory.GetFiles(baseDir, d).Select(Assembly.LoadFrom).SelectMany(y => y.DefinedTypes).ToList();
                allTypes?.ForEach(thisType =>
                { 
                    var allInterfaces = thisType.GetInterfaces().Where(p => p.GetInterfaces().Contains(typeof(IBaseDomain))).ToList();
                    allInterfaces?.ForEach(thisInterface =>
                    {
                        services.AddScoped(thisInterface, thisType);
                    });
                });
            });
        }
    }
}
