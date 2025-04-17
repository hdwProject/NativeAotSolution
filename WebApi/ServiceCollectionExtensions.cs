//using Microsoft.CodeAnalysis;
using System.Reflection;
using Services;

namespace WebApi
{
   
    public static class ServiceCollectionExtensions 
    {
        // 自动注册程序集中所有标记类
        public static IServiceCollection AutoRegisterServices(this IServiceCollection services, Assembly assembly, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            //var types = assembly.GetTypes()
            //    .Where(t => t.IsClass && !t.IsAbstract)
            //    .Where(t => t.GetCustomAttribute<AutoInjectAttribute>() != null);
            var servicesDll = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory +"Services.dll");
          
            var types = servicesDll.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => t.IsInterface).Where(x=>x.GetType() == typeof(IDependency));

            var tttt = servicesDll.DefinedTypes.Where(t => t.IsInterface);

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                if (interfaces.Length > 0)
                {
                    foreach (var interfaceType in interfaces)
                    {
                        services.Add(new ServiceDescriptor(interfaceType, provider => CreateInjectedInstance(provider, type), lifetime));
                    }
                }
                else
                {
                    services.Add(new ServiceDescriptor(type, provider => CreateInjectedInstance(provider, type), lifetime));
                }
            }
            return services;
        }

        private static object CreateInjectedInstance(IServiceProvider provider, Type type)
        {
            // 使用 ASP.NET Core 8.0 优化的 ActivatorUtilities
            var instance = ActivatorUtilities.CreateInstance(provider, type);

            // 自动属性注入
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite && p.GetCustomAttribute<AutoInjectAttribute>() != null);

            foreach (var prop in properties)
            {
                var service = provider.GetService(prop.PropertyType);
                if (service != null)
                {
                    prop.SetValue(instance, service);
                }
                else
                {
                    throw new InvalidOperationException($"Service {prop.PropertyType.Name} not registered");
                }
            }
            return instance;
        }
    }
}
