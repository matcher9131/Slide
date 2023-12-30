using System.Collections.Generic;

namespace Slide
{
    public record TreeViewItemModel(
        string ShortName,
        string FullName,
        IEnumerable<TreeViewItemModel> Children
    );
}
