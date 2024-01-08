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
        private readonly FavoriteLevel favoriteLevel;

        public ReadOnlyReactivePropertySlim<Color> Level0ButtonColor { get; }

        public ReadOnlyReactivePropertySlim<Color> Level1ButtonColor { get; }

        public ReadOnlyReactivePropertySlim<Color> Level2ButtonColor { get; }

        public ReactiveCommand Level0ButtonClickCommand { get; }

        public ReactiveCommand Level1ButtonClickCommand { get; }

        public ReactiveCommand Level2ButtonClickCommand { get; }

        public FavoriteLevelFilterViewModel(FavoriteLevel favoriteLevel)
        {
            this.favoriteLevel = favoriteLevel;
            this.Level0ButtonColor = this.favoriteLevel.SelectedLevel
                .Select(level => FavoriteLevelColors.GetColor(0, level == 0))
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.Level1ButtonColor = this.favoriteLevel.SelectedLevel
                .Select(level => FavoriteLevelColors.GetColor(1, level == 1))
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.Level2ButtonColor = this.favoriteLevel.SelectedLevel
                .Select(level => FavoriteLevelColors.GetColor(2, level == 2))
                .ToReadOnlyReactivePropertySlim()
                .AddTo(this.disposables);
            this.Level0ButtonClickCommand = new ReactiveCommand().WithSubscribe(() => this.ChangeSelectedLevel(0)).AddTo(this.disposables);
            this.Level1ButtonClickCommand = new ReactiveCommand().WithSubscribe(() => this.ChangeSelectedLevel(1)).AddTo(this.disposables);
            this.Level2ButtonClickCommand = new ReactiveCommand().WithSubscribe(() => this.ChangeSelectedLevel(2)).AddTo(this.disposables);
        }

        private void ChangeSelectedLevel(int newLevel)
        {
            this.favoriteLevel.SelectedLevel.Value = newLevel;
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion
    }
}
