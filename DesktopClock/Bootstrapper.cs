using DesktopClock.Views;
using Prism.DryIoc;
using Prism.Ioc;
using System.Windows;

namespace DesktopClock
{
    public class Bootstrapper : PrismBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}