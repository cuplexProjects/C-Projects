using System;
using System.IO;
using GeneralToolkitLib.Log;
using GeneralToolkitLib.Storage;
using GeneralToolkitLib.Storage.Models;
using ImageView.DataContracts;
using ImageView.Events;

namespace ImageView.Models.Implementation
{
    
    public class BookmarkContainerImplementation : BookmarkContainerBase
    {
        private BookmarkContainerImplementation()
        {
        }

        public BookmarkContainerImplementation(BookmarkContainer container)
        {
            RootFolder = container.RootFolder;
            LastUpdate = container.LastUpdate;
            ContainerId = container.ContainerId;
        }

        public BookmarkFolderImplementation RootFolderImplementation { get; protected set; }

        public void SetDataUpdated()
        {
            IsDirty = true;
        }

        public override bool SaveBookmarkFile(BookmarkContainer bookmarkContainer, string password, string fileName)
        {
            try
            {
                var settings = new StorageManagerSettings(false, Environment.ProcessorCount, true, password);
                var storageManager = new StorageManager(settings);
                bool successful = storageManager.SerializeObjectToFile(bookmarkContainer, fileName, null);

                if (successful)
                {
                    LastUpdate = DateTime.Now;
                    IsDirty = false;
                }
                return successful;
            }
            catch (Exception ex)
            {
                LogWriter.LogError("BookmarkService.SaveBookmarkFile(string path) : " + ex.Message, ex);
                return false;
            }
        }

        public static BookmarkContainerImplementation CreateRootContainer(BookmarkUpdatedEventHandler updatedEvent)
        {
            var container = new BookmarkContainerImplementation
            {
                ContainerId = Guid.NewGuid().ToString(),
                LastUpdate = DateTime.Now
            };
            container.RootFolder = BookmarkFolderImplementation.CreateRootBookmarkFolder(updatedEvent, container);
            container.RootFolderImplementation = (BookmarkFolderImplementation) container.RootFolder;


            return container;
        }
    }
}