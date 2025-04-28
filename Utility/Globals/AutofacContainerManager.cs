using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Globals
{
    public static class AutofacContainerManager
    {
        private static readonly Lock Obj = new Lock();
        private static ILifetimeScope _container { get; set; }

        public static void InitContainer(ILifetimeScope container)
        {
            //防止过程中方法被调用_container发生改变
            if (_container == null)
            {
                lock (Obj)
                {
                    _container ??= container;
                }
            }
        }
        /// <summary>
        /// 手动获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>() where T : notnull
        {
            return _container.Resolve<T>();
        }
    }
}
