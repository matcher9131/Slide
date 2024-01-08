using Prism.Ioc;
using Prism.Unity;
using Slide.Models;
using Slide.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace Slide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<SelectedItemModel>();
            containerRegistry.RegisterSingleton<FavoriteLevel>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            FavoriteLevelDb.Load();
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            FavoriteLevelDb.Save();
        }
    }
}
