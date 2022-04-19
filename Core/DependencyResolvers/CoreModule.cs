using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DependencyResolvers
{
    //Projenin kendisi ile ilgili bağımlılıkları Business katmanında çözdük.
    //Buradaki olay ise Uygulama Seviyesinde Servis bağımlılıklarımızı çözümleyeceğimiz yer.
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddMemoryCache(); //IMemoryCache _memoryCache; .NET Core kendisi injection yapıyor.
                                                //Arka planda hazır bir IMemoryCache'in instance'ı oluşturuyor.
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            //Redis ile cacheleme yapmak isteseydik şu satırı silerdik. serviceCollection.AddMemoryCache();
            //serviceCollection.AddSingleton<ICacheManager, RedisCacheManager>();
            serviceCollection.AddSingleton<Stopwatch>();
        }
    }
}
