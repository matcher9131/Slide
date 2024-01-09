using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Slide.Models;
using Slide.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Slide.ViewModels
{
    public class FavoriteLevelFilterViewModel : BindableBase, IDisposable
    {
        private readonly SelectedFavoriteLevel selectedFavoriteLevel;

        public FavoriteLevelFilterButtonViewModel[] ButtonViewModels { get; }

        public FavoriteLevelFilterViewModel(SelectedFavoriteLevel selectedFavoriteLevel)
        {
            this.selectedFavoriteLevel = selectedFavoriteLevel;
            this.ButtonViewModels = Enumerable.Range(0, 4).Select(i => new FavoriteLevelFilterButtonViewModel(this.selectedFavoriteLevel, i)).ToArray();
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
