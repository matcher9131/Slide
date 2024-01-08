using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slide.Models
{
    public class FavoriteLevel : BindableBase
    {
        public ReactivePropertySlim<int> SelectedLevel { get; }

        public FavoriteLevel()
        {
            this.SelectedLevel = new ReactivePropertySlim<int>();
        }
    }
}
