using System.Reflection;
using Autofac;
using Autofac.Core;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.User;
using SqlSugar;
using Utility;
using Utility.CustomerAttribute;
using Module = Autofac.Module;

namespace WebApi
{
   
    public static class CustomerServiceCollectionExtensions 
    {
        /// <summary>
        /// Autofac配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="projects">扫描程序</param>
        /// <param name="controller">控制器</param>
        /// <param name="service">服务接口</param>
        /// <param name="program">主程序</param>
        public static void AutofacConfigureContainer(this ContainerBuilder builder, List<string>? projects, Type controller, Type service, Type program)
        {
            projects ??= [];
            var baseType = service;//IDenpendency 是一个接口（所有要实现依赖注入的借口都要继承该接口）
            var controllerBaseType = controller;//控制器
            foreach (var item in projects)
            {
                var assemblys = Assembly.Load(item);//Service是继承接口的实现方法类库名称
                builder.RegisterAssemblyTypes(assemblys).Where(m => baseType.IsAssignableFrom(m) && m != baseType)
                    .InstancePerLifetimeScope()//生命周期，这里没有使用接口方式
                    .PropertiesAutowired();//属性注入
                //插件Controller中使用属性注入
                //builder.RegisterAssemblyTypes(assemblys).Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType).PropertiesAutowired();
            }
            //Controller中使用属性注入
            builder.RegisterAssemblyTypes(program.Assembly).Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType).PropertiesAutowired();
            
        }

        /// <summary>
        /// 批量注入扩展
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assembly"></param>
        public static void BatchAutowired(this ContainerBuilder builder, Assembly assembly)
        {
            var transientType = typeof(ITransitDependency); //瞬时注入
            var singletonType = typeof(ISingletonDependency); //单例注入
            var scopeType = typeof(IScopeDependency); //作用域注入
            //瞬时注入
            builder.RegisterAssemblyTypes(assembly).Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces().Contains(transientType))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired(new AutowiredPropertySelector());
            //单例注入
            builder.RegisterAssemblyTypes(assembly).Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces().Contains(singletonType))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance()
                .PropertiesAutowired(new AutowiredPropertySelector());
            //生命周期注入
            builder.RegisterAssemblyTypes(assembly).Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces().Contains(scopeType))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .PropertiesAutowired(new AutowiredPropertySelector());
        }

        /// <summary>
        /// 注册SqlSugar
        /// </summary>
        /// <param name="builder"></param>
        public static void AddSqlSugar(this ContainerBuilder builder)
        {
            builder.Register(x =>
            {
                var config = new ConnectionConfig
                {
                    ConnectionString = GlobalContext.SystemConfig.DBConnectionString,
                    DbType = DbType.Sqlite,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings
                    {
                        IsAutoRemoveDataCache = true,
                        //SqliteCodeFirstEnableDefaultValue = true, //启用默认值
                        //SqliteCodeFirstEnableDropColumn = true, //只支持.net core
                        SqliteCodeFirstEnableDescription = true //启用备注
                    },
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        //DataInfoCacheService = new SqlSugarCache(), //配置我们创建的缓存类
                        EntityService = (property, column) =>
                        {
                            var attributes = property.GetCustomAttributes(true); //get all attributes

                            if (attributes.Any(it => it is SugarColumn) && column.DataType == "longtext")
                            {
                                column.DataType = "nvarchar(4000)";
                            }
                        }
                    }
                };
                var sqlSugarClient = new SqlSugarClient(config);
                sqlSugarClient.Aop.OnLogExecuted = (sql, pars) => //SQL执行完
                {
                    if (sql.StartsWith("SELECT"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[SELECT]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    }

                    if (sql.StartsWith("INSERT"))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("[INSERT]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    }

                    if (sql.StartsWith("UPDATE"))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("[UPDATE]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    }

                    if (sql.StartsWith("DELETE"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[DELETE]-" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    }

                    Console.WriteLine($"执行库{sqlSugarClient.CurrentConnectionConfig.ConfigId}");
                    Console.WriteLine("NeedTime-" + sqlSugarClient.Ado.SqlExecutionTime);
                    //App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                    Console.WriteLine("Content:" +
                                      UtilMethods.GetSqlString(sqlSugarClient.CurrentConnectionConfig.DbType, sql,
                                          pars));
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("");
                };
                sqlSugarClient.Ado.CommandTimeOut = GlobalContext.SystemConfig.DBCommandTimeout;
                sqlSugarClient.Aop.OnError = (exp) =>
                {
                    SerilogHelper.Error(exp,"执行数据库操作错误");
                    Console.WriteLine("Error:" + exp.Message);
                };

                return sqlSugarClient;

            }).As<ISqlSugarClient>().SingleInstance(); // 或者使用SingleInstance(),  
        }
    }

    /// <summary>
    /// Autofac模块注册
    /// </summary>
    public class RegisterAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            ////获取所有控制器类型并使用属性注入
            //var controllersTypeAssembly = typeof(Program).Assembly.GetExportedTypes()
            //    .Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToArray();
            //builder.RegisterTypes(controllersTypeAssembly).PropertiesAutowired();

            ////批量自动注入,把需要注入层的程序集传参数,注入Service层的类
            //builder.BatchAutowired(typeof(ISingletonDependency).Assembly);

            //需要注入的程序集
            //string[] assemblyList = ["Services", "Repositories"];
            //foreach (var item in assemblyList)
            //{
            //    // 获取当前程序集（或指定程序集）
            //    var assemblyService = Assembly.Load(item);
            //    // 自动注册所有以 "Service" 结尾的非泛型类
            //    builder.RegisterAssemblyTypes(assemblyService)
            //        .Where(t => t.Name.TrimEnd('s').EndsWith(item) && t is { IsGenericType: false, IsClass: true, IsAbstract: false })
            //        .AsSelf()
            //        .AsImplementedInterfaces()
            //        .InstancePerLifetimeScope(); // Scoped 生命周期
            //}

            // 获取当前程序集（或指定程序集）
            var assemblyService = Assembly.Load("Services");
            // 自动注册所有以 "Service" 结尾的非泛型类
            builder.RegisterAssemblyTypes(assemblyService)
                .Where(t => t.Name.EndsWith("Service") && t is { IsGenericType: false, IsClass: true, IsAbstract: false })
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); // Scoped 生命周期

            // 获取当前程序集（或指定程序集）
            var assemblyRepository = Assembly.Load("Repositories");
            // 自动注册所有以 "Service" 结尾的非泛型类
            builder.RegisterAssemblyTypes(assemblyRepository)
                .Where(t => t.Name.EndsWith("Repository") && t is { IsGenericType: false, IsClass: true, IsAbstract: false })
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope(); // Scoped 生命周期

        }
        
    }
    /// <summary>
    /// 属性注入选择器
    /// </summary>
    public class AutowiredPropertySelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            //判断属性的特性是否包含自定义的属性,标记有返回true
            return propertyInfo.CustomAttributes.Any(s => s.AttributeType == typeof(AutowiredDependencyPropertyAttribute));
        }
    }


}
