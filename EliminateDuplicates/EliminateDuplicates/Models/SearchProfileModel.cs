using System.Collections.Generic;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;

namespace DeleteDuplicateFiles.Models
{
    public class SearchProfileModel
    {
        public SearchProfileModel(string profileName, List<ScanFolderModel> scanFolderList, List<PreferredDirectoryDataModel> preferredDirecoryList)
        {
            ScanFolderList = scanFolderList;
            PreferredDirecoryList = preferredDirecoryList;
            ProfileName = profileName;
        }


        public List<ScanFolderModel> ScanFolderList { get; set; }

        public List<PreferredDirectoryDataModel> PreferredDirecoryList { get; set; }

        public string FileNameFilter { get; set; }

        public string ProfileName { get; set; }

        public string FullPath { get; set; }

        public bool IncludeSubfolders { get; set; }

        public void ScanFolderListUpdated()
        {

        }

        public static void CreateMappings(IProfileExpression expression)
        {
            expression.CreateMap<SearchProfileModel, SearchProfileDataModel>()
                .ForMember(d => d.ProfileName, o => o.MapFrom(d => d.ProfileName))
                .ForMember(d => d.FileNameFilter, o => o.MapFrom(d => d.FileNameFilter))
                .ForMember(d => d.PreferredDirecoryList, o => o.MapFrom(d => d.PreferredDirecoryList))
                .ForMember(d => d.ScanFolderList, o => o.MapFrom(d => d.ScanFolderList))
                .ForMember(d => d.IncludeSubfolders, o => o.MapFrom(d => d.IncludeSubfolders))
                .ReverseMap();
        }
    }
}