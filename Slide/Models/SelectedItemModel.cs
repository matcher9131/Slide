using Prism.Mvvm;
using Reactive.Bindings;

namespace Slide.Models
{
    public class SelectedItemModel : BindableBase
    {
        public ReactivePropertySlim<string?> SelectedDirectory { get; }

        public ReactivePropertySlim<string?> SelectedFile { get; }

        public SelectedItemModel()
        {
            this.SelectedDirectory = new ReactivePropertySlim<string?>(null);
            this.SelectedFile = new ReactivePropertySlim<string?>(null);
        }
    }
}
