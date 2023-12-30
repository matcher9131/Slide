using Reactive.Bindings;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace Slide
{
    public class MainViewModel : INotifyPropertyChanged
    {
        static readonly string[] Extensions = new string[] { ".bmp", ".png", ".jpg", ".jpeg", ".webp", ".gif" };

        public MainViewModel()
        {
            this.SelectedPath = new ReactivePropertySlim<string?>("C:\\");
            this.Filenames = this.SelectedPath
                .Select(x => Directory.EnumerateFiles(x!)
                    .AsParallel()
                    .AsOrdered()
                    .Where(filepath => Extensions.Contains(Path.GetExtension(filepath).ToLower()))
                    .Select(filepath => new FilepathStructure(filepath, Path.GetFileName(filepath)))
                    .OrderBy(filepath => filepath.Filename, new FilenameComparer())
                    .ToArray()
                ).ToReadOnlyReactivePropertySlim();
            this.OpenExplorerCommand = this.SelectedPath.Select(path => !string.IsNullOrEmpty(path)).ToReactiveCommand();
            this.OpenExplorerCommand.Subscribe(_ => this.OpenExplorer());
        }

        public ReactivePropertySlim<string?> SelectedPath { get; }

        public ReadOnlyReactivePropertySlim<FilepathStructure[]?> Filenames { get; }

        public ReactiveCommand OpenExplorerCommand { get; init; }
        private void OpenExplorer()
        {
            System.Diagnostics.Process.Start("explorer.exe", this.SelectedPath.Value!);
        }
                
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
