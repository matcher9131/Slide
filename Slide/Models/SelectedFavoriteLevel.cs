using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slide.Models
{
    public class SelectedFavoriteLevel : BindableBase
    {
        public ReactivePropertySlim<int> Level { get; }

        public SelectedFavoriteLevel()
        {
            this.Level = new ReactivePropertySlim<int>();
        }
    }
}
