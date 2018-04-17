using System.Collections.Generic;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;

namespace DeleteDuplicateFiles.Models
{
    public class FileHashCollection
    {
        public FileHashCollection()
        {
            FileHashDictionary = new Dictionary<string, ComputedFileHashModel>();
        }

    
        public Dictionary<string, ComputedFileHashModel> FileHashDictionary { get; set; }

        public static void CreateMappings(IProfileExpression expression)
        {
            expression.CreateMap<FileHashCollection, FileHashCollectionDataModel>()
                .ReverseMap();
        }
    }
}