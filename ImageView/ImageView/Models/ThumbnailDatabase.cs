using System;
using System.Collections.Generic;
using AutoMapper;
using Castle.Components.DictionaryAdapter;
using ImageViewer.DataContracts;

namespace ImageViewer.Models
{
    public class ThumbnailDatabase
    {
        public EditableList<ThumbnailEntry> ThumbnailEntries { get; set; }

        public string DatabaseId { get; set; }

        public string DataStoragePath { get; set; }

        public DateTime LastUpdated { get; set; }

        public static void CreateMapping(IProfileExpression expression)
        {
            expression.CreateMap<ThumbnailDatabase, ThumbnailDatabaseModel>()
                .ForMember(s => s.ThumbnailEntries, o => o.MapFrom(d => new List<ThumbnailEntryModel>(Mapper.Map<List<ThumbnailEntryModel>>(d.ThumbnailEntries))))
                .ReverseMap()
                .ForMember(s => s.ThumbnailEntries, o => o.MapFrom(d => new EditableBindingList<ThumbnailEntry>(Mapper.Map<EditableBindingList<ThumbnailEntry>>(d.ThumbnailEntries))));
        }
    }
}