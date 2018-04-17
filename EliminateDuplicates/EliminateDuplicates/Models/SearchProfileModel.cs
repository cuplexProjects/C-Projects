using System.Collections.Generic;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;

namespace DeleteDuplicateFiles.Models
{
    public class SearchProfileModel
    {
        protected SearchProfileModel()
        {
        }

        public SearchProfileModel(List<ScanFolderListItem> scanFolderList, List<PreferredDirectoryDataModel> preferredDirecoryList)
        {
            ScanFolderList = scanFolderList;
            PreferredDirecoryList = preferredDirecoryList;
        }

        public SearchProfileModel(string profileName, List<ScanFolderListItem> scanFolderList, List<PreferredDirectoryDataModel> preferredDirecoryList)
        {
            ScanFolderList = scanFolderList;
            PreferredDirecoryList = preferredDirecoryList;
            ProfileName = profileName;
        }


        public List<ScanFolderListItem> ScanFolderList { get; set; }

        public List<PreferredDirectoryDataModel> PreferredDirecoryList { get; set; }

        public string FileNameFilter { get; set; }

        public string ProfileName { get; set; }

        public string FullPath { get; set; }

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
                .ReverseMap();
        }
    }
}