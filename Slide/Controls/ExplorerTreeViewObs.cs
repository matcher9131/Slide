using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Slide.Controls
{
    public class ExplorerTreeViewObs : TreeView
    {
        public ExplorerTreeViewObs()
        {
            this.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(this.OnItemExpanded));
            this.SelectedItemChanged += (s, e) => {
                this.SelectedPath = this.GetSelectedPath() ?? string.Empty;
            };
        }

        private static TreeViewItem GenerateDriveNode(DriveInfo drive)
        {
            var item = new TreeViewItem
            {
                Tag = drive,
                Header = drive.ToString()
            };
            item.Items.Add("*");
            return item;
        }

        private static TreeViewItem GenerateDirectoryNode(DirectoryInfo directory)
        {
            var item = new TreeViewItem
            {
                Tag = directory,
                Header = directory.Name,
            };
            item.Items.Add("*");
            return item;
        }

        public void InitExplorer()
        {
            this.Items.Clear();
            foreach (var drive in DriveInfo.GetDrives())
            {
                this.Items.Add(GenerateDriveNode(drive));
            }
        }

        private void OnItemExpanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();
            DirectoryInfo dir = item.Tag is DriveInfo driveInfo ? driveInfo.RootDirectory
                : item.Tag is DirectoryInfo directory ? directory
                : throw new InvalidOperationException();
            foreach (var sub in dir.GetDirectories())
            {
                if (sub.Attributes.HasFlag(FileAttributes.Hidden)) continue;
                if (sub.Attributes.HasFlag(FileAttributes.System)) continue;
                item.Items.Add(GenerateDirectoryNode(sub));
            }
        }

        #region Members for Binding
        private bool Expand(TreeViewItem item, int index, string[] pathParts)
        {
            if (index > pathParts.Length) return false;
            if (index == pathParts.Length)
            {
                item.IsSelected = true;
                return true;
            }
            if (!item.IsExpanded)
            {
                item.IsExpanded = true;
            }
            string name = pathParts[index].ToLower();
            foreach (TreeViewItem folderItem in item.Items)
            {
                var directoryInfo = (DirectoryInfo)folderItem.Tag;
                if (directoryInfo.Name.ToLower() == name)
                {
                    this.Expand(folderItem, index + 1, pathParts);
                    return true;
                }
            }
            return false;
        }

        private string? GetSelectedPath()
        {
            if (this.SelectedItem is TreeViewItem item)
            {
                return item.Tag is DriveInfo driveInfo ? driveInfo.RootDirectory.FullName
                    : item.Tag is DirectoryInfo directoryInfo ? directoryInfo.FullName
                    : throw new InvalidOperationException();
            }
            else
            {
                return null;
            }
        }

        private void SetSelectedPath(string path)
        {
            this.InitExplorer();

            var split = Path.GetFullPath(path).Split(
                new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
                StringSplitOptions.RemoveEmptyEntries
            );
            var drive = new DriveInfo(split[0]);
            foreach (TreeViewItem item in Items)
            {
                var name = ((DriveInfo)item.Tag).Name.ToLower();
                if (name == drive.Name.ToLower())
                {
                    if (!this.Expand(item, 1, split))
                    {
                        MessageBox.Show("指定されたフォルダは存在しません。");
                    }
                    break;
                }
            }
        }

        public string SelectedPath
        {
            get { return (string)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        public static readonly DependencyProperty SelectedPathProperty = DependencyProperty.Register(nameof(SelectedPath), typeof(string), typeof(ExplorerTreeViewObs));

        private bool IsSelectionUpdateRequired(string newPath)
        {
            if (string.IsNullOrEmpty(newPath)) return true;
            var selectedPath = this.GetSelectedPath();
            if (string.IsNullOrEmpty(selectedPath)) return true;
            return !Path.GetFullPath(newPath).Equals(Path.GetFullPath(selectedPath));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == SelectedPathProperty && e.NewValue is string newValue)
            {
                if (this.IsSelectionUpdateRequired(newValue))
                {
                    this.SetSelectedPath(newValue);
                }
            }
        }
        #endregion
    }
}
