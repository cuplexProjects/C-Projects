using System;
using AutoMapper;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageView.DataContracts;
using ImageView.ObjectMapper;

namespace ImageView.Models.Implementation
{
    public abstract class BookmarkContainerBase : BookmarkContainer
    {
        public bool IsDirty { get; protected set; }
        public Action BookmarkTreeUpdated;
        public abstract bool SaveBookmarkFile(BookmarkContainer bookmarkContainer, string password, string fileName);
        public static BookmarkContainer LoadBookmarkFile(string password, string fileName)
        {
            BookmarkContainer bookmarkContainer = null;
            var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
            var storageManager = new StorageManager(settings);
            bookmarkContainer = storageManager.DeserializeObjectFromFile<BookmarkContainer>(fileName, null);
            return bookmarkContainer;
        }

        static BookmarkContainerBase()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<BookmarkContainerBase, BookmarkContainer>();
                cfg.AddProfile<OrganizationProfile>();
            });
            var mapper = config.CreateMapper();
     


        }
    }
}