using System;
using System.Collections.Generic;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;
using GeneralToolkitLib.Hashing;
using GeneralToolkitLib.Utility;

namespace DeleteDuplicateFiles.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso>
    ///     <cref>System.IEquatable{DeleteDuplicateFiles.Models.ApplicationSettingsModel}</cref>
    /// </seealso>
    /// <inheritdoc />
    public class ApplicationSettingsModel : IEquatable<ApplicationSettingsModel>
    {
        /// <summary>
        /// The hash code
        /// </summary>
        private readonly int _hashCode;

        /// <summary>
        /// 
        /// </summary>
        public enum DeletionModes
        {
            /// <summary>
            /// The permanent
            /// </summary>
            Permanent = 1,
            /// <summary>
            /// The recycle bin
            /// </summary>
            RecycleBin = 2
        }

        /// <summary>
        /// 
        /// </summary>
        public enum HashAlgorithms
        {
            /// <summary>
            /// The cr C32
            /// </summary>
            CRC32 = 1,
            /// <summary>
            /// The m d5
            /// </summary>
            MD5 = 2
        }

        /// <summary>
        /// 
        /// </summary>
        public enum MasterFileSelectionMethods
        {
            /// <summary>
            /// The oldest modified date
            /// </summary>
            OldestModifiedDate = 1,
            /// <summary>
            /// The newest modified date
            /// </summary>
            NewestModifiedDate = 2
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettingsModel"/> class.
        /// </summary>
        public ApplicationSettingsModel()
        {
            _hashCode = Helpers.Helpers.GenerateHashcode(this);
        }

        /// <summary>
        /// Gets or sets the hash algorithm.
        /// </summary>
        /// <value>
        /// The hash algorithm.
        /// </value>
        public HashAlgorithms HashAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the master file selection method.
        /// </summary>
        /// <value>
        /// The master file selection method.
        /// </value>
        public MasterFileSelectionMethods MasterFileSelectionMethod { get; set; }

        /// <summary>
        /// Gets or sets the deletion mode.
        /// </summary>
        /// <value>
        /// The deletion mode.
        /// </value>
        public DeletionModes DeletionMode { get; set; }

        /// <summary>
        /// Gets or sets the maximum no of hashing threads.
        /// </summary>
        /// <value>
        /// The maximum no of hashing threads.
        /// </value>
        public int MaximumNoOfHashingThreads { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore hidden files and directories].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ignore hidden files and directories]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreHiddenFilesAndDirectories { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore system files and directories].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ignore system files and directories]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreSystemFilesAndDirectories { get; set; }

        /// <summary>
        /// Gets or sets the last profile file path.
        /// </summary>
        /// <value>
        /// The last profile file path.
        /// </value>
        public string LastProfileFilePath { get; set; }



        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="expression">The expression.</param>
        public static void CreateMappings(IProfileExpression expression)
        {
            expression.CreateMap<ApplicationSettingsModel, ApplicationSettingsDataModel>()
                .ReverseMap();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(ApplicationSettingsModel other)
        {
            return TypedObjectCompare.PublicInstancePropertiesEqual(this, other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as ApplicationSettingsModel);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _hashCode;
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"MaximumNoOfHashingThreads: {MaximumNoOfHashingThreads}, IgnoreSystemFilesAndDirectories: {IgnoreSystemFilesAndDirectories}, DeletionMode: {DeletionMode}, HashAlgorithm: {HashAlgorithm}";
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="model1">The model1.</param>
        /// <param name="model2">The model2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(ApplicationSettingsModel model1, ApplicationSettingsModel model2)
        {
            return EqualityComparer<ApplicationSettingsModel>.Default.Equals(model1, model2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="model1">The model1.</param>
        /// <param name="model2">The model2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(ApplicationSettingsModel model1, ApplicationSettingsModel model2)
        {
            return !(model1 == model2);
        }

        public string DefaultKey => SHA256.GetSHA256HashAsHexString("e2BYqPwjhF5WGcBRgcF8tOMJFhMAMWXG4Itp0PZEjSiBRhDqgHudSTCeTm1LipD4KU0GAtq1VAsUfrkNNjYd5LTVhys4TMftvZ0R5FHtDPxRlnSpXit2UvGvVmPWdEbNrCnw6YVLwWt1cdf2spd11IcY5XL022M69gFYNFYnKn3N9QOJFvUc8q5b6z0erkYZBgbOpGFg3CcAJIIHxPiO7ipUUXRX9Yk88YDnTctDUlrfVr6ZZvBTArkDL3UUNMct");
    }
}