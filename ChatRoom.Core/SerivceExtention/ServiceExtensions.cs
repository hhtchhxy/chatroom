using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Core.SerivceExtention
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 自动注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddAutoInject(this IServiceCollection services, string ServiceName)
        { 
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
