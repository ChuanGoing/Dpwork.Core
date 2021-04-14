using Dpwork.Core.Dependence;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;


namespace Dpwork.Core.Extensions
{
    public static class DependencyInjectionExtension
    {
        /// <summary>
        /// 程序集自动注入
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        public static IServiceCollection RegisterModule<TModule>(this IServiceCollection services)
        {
            var moduleType = typeof(TModule);
            var assembly = moduleType.Assembly;
            ((IModule)assembly.CreateInstance(moduleType.FullName)).Load(services);

            foreach (var type in assembly.ExportedTypes)
            {
                if (type.IsPublic && !type.IsAbstract && type.IsClass)
                {
                    foreach (var intrType in type.GetInterfaces())
                    {
                        var impInterfaces = intrType.GetInterfaces();
                        if (impInterfaces.Any(imp => imp.Equals(typeof(ITransient))))
                        {
                            services.AddTransient(intrType, type);
                        }
                        else if (impInterfaces.Any(imp => imp.Equals(typeof(IScope))))
                        {
                            services.AddScoped(intrType, type);
                        }
                        else if (impInterfaces.Any(imp => imp.Equals(typeof(ISingleton))))
                        {
                            services.AddSingleton(intrType, type);
                        }
                    }
                }
            }
            return services;
        }

        public static IServiceCollection AddBetaCore<TModule>(this IServiceCollection services, Action<DpworkContext> context = null)
        {
            var defaultContext = new DpworkContext();

            if (context != null)
            {
                context.Invoke(defaultContext);
            }
            foreach (var ctx in defaultContext.Contexts)
            {
                services.AddScoped(ctx.Key, ctx.Value);
            }

            services.RegisterModule<DpworkModule>();
            services.RegisterModule<TModule>();

            return services;
        }

        //public static void AddAutoMapperService<TModule>(this IServiceCollection services)
        //where TModule : Module, new()
        //{
        //    services.AddAutoMapper(System.Reflection.Assembly.GetAssembly(typeof(TModule)));
        //}
    }
}
