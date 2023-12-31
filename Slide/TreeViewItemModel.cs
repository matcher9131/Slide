using System.Collections.Generic;

namespace Slide
{
    public record TreeViewItemModel(
        string Name,
        string Path,
        IEnumerable<TreeViewItemModel> Children
    );
}
