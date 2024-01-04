using Reactive.Bindings;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Slide.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        

        public MainWindowViewModel()
        {
            
        }

        

        public ReactiveCommand OpenExplorerCommand { get; init; }
        private void OpenExplorer()
        {
            // System.Diagnostics.Process.Start("explorer.exe", this.SelectedPath.Value!);
        }
                
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
