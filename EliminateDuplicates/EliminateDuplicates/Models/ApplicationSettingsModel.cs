using System;
using System.Collections.Generic;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;
using GeneralToolkitLib.Utility;

namespace DeleteDuplicateFiles.Models
{
    /// <inheritdoc />
    public sealed class ApplicationSettingsModel : IEquatable<ApplicationSettingsModel>, ICloneable
    {
        public enum DeletionModes
        {
            Permanent = 1,
            RecycleBin = 2
        }

        public enum HashAlgorithms
        {
            CRC32 = 1,
            MD5 = 2
        }

        public enum MasterFileSelectionMethods
        {
            OldestModifiedDate = 1,
            NewestModifiedDate = 2
        }
        
        public ApplicationSettingsModel()
        {

        }

        public HashAlgorithms HashAlgorithm { get; set; }
        
        public MasterFileSelectionMethods MasterFileSelectionMethod { get; set; }
       
        public DeletionModes DeletionMode { get; set; }
        
        public int MaximumNoOfHashingThreads { get; set; }
       
        public bool IgnoreHiddenFilesAndDirectories { get; set; }
     
        public bool IgnoreSystemFilesAndDirectories { get; set; }
     
        public string LastProfileFilePath { get; set; }
        

        public static ApplicationSettingsModel GetDefaultSettings()
        {
            return new ApplicationSettingsModel()
            {
                //Default value
                MaximumNoOfHashingThreads = Environment.ProcessorCount,
                IgnoreSystemFilesAndDirectories = true,
                IgnoreHiddenFilesAndDirectories = true,
                DeletionMode = DeletionModes.RecycleBin,
                HashAlgorithm = HashAlgorithms.MD5,
                MasterFileSelectionMethod = MasterFileSelectionMethods.OldestModifiedDate,
                LastProfileFilePath = null,
            };
        }

        public static void CreateMappings(IProfileExpression expression)
        {
            expression.CreateMap<ApplicationSettingsModel, ApplicationSettingsDataModel>()
                .ReverseMap();
        }

        public bool Equals(ApplicationSettingsModel other)
        {
            return TypedObjectCompare.PublicInstancePropertiesEqual(this, other);
        }

        public object Clone()
        {
            var clone = MemberwiseClone();
            return clone;
        }

        public override string ToString()
        {
            return $"MaximumNoOfHashingThreads: {MaximumNoOfHashingThreads}, IgnoreSystemFilesAndDirectories: {IgnoreSystemFilesAndDirectories}, DeletionMode: {DeletionMode}, HashAlgorithm: {HashAlgorithm}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ApplicationSettingsModel instance))
            {
                return false;
            }

            return TypedObjectCompare.PublicInstancePropertiesEqual(this, (ApplicationSettingsModel) obj);
        }


        public static bool EqualityCheck(ApplicationSettingsModel a, ApplicationSettingsModel b)
        {
            return TypedObjectCompare.PublicInstancePropertiesEqual(a, b);
            
        }

        public static bool operator ==(ApplicationSettingsModel model1, ApplicationSettingsModel model2)
        {
            return EqualityComparer<ApplicationSettingsModel>.Default.Equals(model1, model2);
        }

        public static bool operator !=(ApplicationSettingsModel model1, ApplicationSettingsModel model2)
        {
            return !(model1 == model2);
        }
    }
}