using System.Threading;
using DryIoc;
using IContainer = DryIoc.IContainer;

namespace DesktopClock.Resource
{
    public static class GlobalVariable
    {
        /// <summary>
        /// 全局服务容器
        /// </summary>
        public static readonly IContainer GlobalContainer = new Container();

        /// <summary>
        /// 窗口启动完成标识
        /// </summary>
        public static readonly AutoResetEvent WindowStarted = new(false);
    }
}
