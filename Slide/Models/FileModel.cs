﻿using Prism.Mvvm;
using Reactive.Bindings;
using Slide.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slide.Models
{
    public class FileModel : BindableBase, IDisposable
    {
        private static readonly ConcurrentDictionary<string, FileModel> Dictionary = new();

        public ReactivePropertySlim<FileInfo> FileInfo { get; }

        public ReadOnlyReactivePropertySlim<string> Name { get; }

        public ReadOnlyReactivePropertySlim<string> FullName { get; }

        public ReactivePropertySlim<int> FavoriteLevel { get; }

        public static FileModel Create(FileInfo fileInfo)
        {
            if (Dictionary.TryGetValue(fileInfo.FullName, out FileModel? value)) return value;
            var instance = new FileModel(fileInfo);
            Dictionary.TryAdd(fileInfo.FullName, instance);
            return instance;
        }

        private FileModel(FileInfo fileInfo)
        {
            this.FileInfo = new ReactivePropertySlim<FileInfo>(fileInfo);
            this.Name = this.FileInfo.Select(fileInfo => fileInfo.Name).ToReadOnlyReactivePropertySlim<string>();
            this.FullName = this.FileInfo.Select(fileInfo => fileInfo.FullName).ToReadOnlyReactivePropertySlim<string>();
            this.FavoriteLevel = new(FavoriteLevelDb.GetLevel(fileInfo.FullName));
            this.FavoriteLevel.Subscribe(lv => FavoriteLevelDb.AddOrUpdate(this.FileInfo.Value.FullName, lv));
        }

        #region IDisposable
        private readonly System.Reactive.Disposables.CompositeDisposable disposables = new();
        public void Dispose() => this.disposables.Dispose();
        #endregion



    }
}
