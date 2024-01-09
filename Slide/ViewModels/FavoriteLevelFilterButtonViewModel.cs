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
    public class FavoriteLevelFilterButtonViewModel : BindableBase, IDisposable
    {
        private readonly SelectedFavoriteLevel selectedFavoriteLevel;
        private readonly int level;

        public ReadOnlyReactivePropertySlim<Color> ButtonColor { get; }

        public ReactiveCommand ClickCommand { get; }

        public FavoriteLevelFilterButtonViewModel(SelectedFavoriteLevel selectedFavoriteLevel, int level)
        {
            this.selectedFavoriteLevel = selectedFavoriteLevel;
            this.level = level;
            this.ButtonColor = this.selectedFavoriteLevel.Level
                .Select(level => FavoriteLevelColors.GetColor(this.level, level == this.level))
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.ClickCommand = new ReactiveCommand().WithSubscribe(this.SetSelectedLevel).AddTo(this.disposables);
        }

        private void SetSelectedLevel()
        {
            this.selectedFavoriteLevel.Level.Value = this.level;
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
