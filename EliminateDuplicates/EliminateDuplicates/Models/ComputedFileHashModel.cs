using System;
using AutoMapper;
using DeleteDuplicateFiles.DataModels;

namespace DeleteDuplicateFiles.Models
{
    /// <summary>
    /// ComputedFileHash Model
    /// </summary>
    public class ComputedFileHashModel
    {
        /// <summary>
        /// Gets or sets the full path.
        /// </summary>
        /// <value>
        /// The full path.
        /// </value>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the last write time.
        /// </summary>
        /// <value>
        /// The last write time.
        /// </value>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// Gets or sets the creation time.
        /// </summary>
        /// <value>
        /// The creation time.
        /// </value>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets the cr C32 hash value.
        /// </summary>
        /// <value>
        /// The cr C32 hash value.
        /// </value>
        public string CRC32HashValue { get; set; }

        /// <summary>
        /// Gets or sets the m d5 hash value.
        /// </summary>
        /// <value>
        /// The m d5 hash value.
        /// </value>
        public string MD5HashValue { get; set; }

        /// <summary>
        /// Gets the drive letter.
        /// </summary>
        /// <value>
        /// The drive letter.
        /// </value>
        public string DriveLetter
        {
            get
            {
                if (FullPath != null && FullPath.Length > 3)
                    return FullPath.Substring(0, 3);
                return "";
            }
        }

        public static void CreateMappings(IProfileExpression expression)
        {
            expression.CreateMap<ComputedFileHashModel, ComputedFileHashDataModel>()
                .ReverseMap();
        }

    }
}