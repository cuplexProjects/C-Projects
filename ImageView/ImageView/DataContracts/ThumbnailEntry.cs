using System;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract(Name = "ThumbnailEntry")]
    public class ThumbnailEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThumbnailEntry"/> class.
        /// </summary>
        public ThumbnailEntry()
        {
            UniqueId = Guid.NewGuid();
        }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataMember(Name = "FileName", Order = 1)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the directory.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        [DataMember(Name = "Directory", Order = 2)]
        public string Directory { get; set; }

        /// <summary>
        /// Gets or sets the file position.
        /// </summary>
        /// <value>
        /// The file position.
        /// </value>
        [DataMember(Name = "FilePosition", Order = 3)]
        public long FilePosition { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        [DataMember(Name = "Length", Order = 4)]
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        [DataMember(Name = "Date", Order = 5)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the source image date.
        /// </summary>
        /// <value>
        /// The source image date.
        /// </value>
        [DataMember(Name = "SourceImageDate", Order = 6)]
        public DateTime SourceImageDate { get; set; }

        /// <summary>
        /// Gets or sets the length of the source image.
        /// </summary>
        /// <value>
        /// The length of the source image.
        /// </value>
        [DataMember(Name = "SourceImageLength", Order = 7)]
        public long SourceImageLength { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        [DataMember(Name = "UniqueId", Order = 8)]
        public Guid UniqueId { get; set; }
    }
}
