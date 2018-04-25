using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;

namespace DeleteDuplicateFiles.Models
{
    public class FileHashCollection : FileHashCollectionDataModel
    {
        public static FileHashCollection CreateFileHashCollection()
        {
            var collection = new FileHashCollection {CollectionId = Guid.NewGuid(),FileHashDictionary = new Dictionary<string, ComputedFileHashDataModel>(),LastModified = DateTime.Now};
            return collection;
        }

    }
}