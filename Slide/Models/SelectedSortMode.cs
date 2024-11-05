using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slide.Models
{
    public enum SortMode
    {
        Filename,
        LastWriteTime,
        CreationTime
    }

    public class SelectedSortMode
    {
        public ReactivePropertySlim<SortMode> DirectorySortMode { get; }
        public ReactivePropertySlim<SortMode> FileSortMode { get; }

        public SelectedSortMode()
        {
            this.DirectorySortMode = new ReactivePropertySlim<SortMode>(SortMode.Filename);
            this.FileSortMode = new ReactivePropertySlim<SortMode>(SortMode.Filename);
        }
    }
}
