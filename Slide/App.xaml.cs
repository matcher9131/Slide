using Prism.Ioc;
using Prism.Unity;
using Slide.Models;
using Slide.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
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
        private static Mutex? mutex;

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<SelectedItemModel>();
            containerRegistry.RegisterSingleton<SelectedFavoriteLevel>();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            mutex = new Mutex(false, "{265A0339-6333-415E-A85E-9B285D2E593E}");
            if (!mutex.WaitOne(0, false))
            {
                // 多重起動
                MessageBox.Show("既に起動しています。", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                mutex.Close();
                mutex = null;
                this.Shutdown();
                return;
            }

            FavoriteLevelDb.Load();
        }

        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            // mutex == nullなら多重起動によるShutdown
            if (mutex == null) return;

            FavoriteLevelDb.Save();

            mutex.ReleaseMutex();
            mutex.Close();
            mutex = null;
        }
    }
}
