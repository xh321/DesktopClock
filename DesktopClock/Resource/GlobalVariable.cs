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
    }
}
