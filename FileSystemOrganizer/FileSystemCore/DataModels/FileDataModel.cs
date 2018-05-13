using System;
using System.Runtime.Serialization;
using FileSystemCore.Models;

namespace FileSystemCore.DataModels
{
    /// <summary>
    /// FileModel
    /// </summary>
    [DataContract]
    public class FileDataModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileModel"/> class.
        /// </summary>
        public FileDataModel()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember(Name = "Id", Order = 1)]
        public Guid Id { get; protected set; }

        /// <summary>
        /// Gets or sets the folder identifier.
        /// </summary>
        /// <value>
        /// The folder identifier.
        /// </value>
        [DataMember(Name = "FolderId", Order = 2)]
        public Guid FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember(Name = "Name", Order = 3)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full path.
        /// </summary>
        /// <value>
        /// The full path.
        /// </value>
        [DataMember(Name = "FullPath", Order = 4)]
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        [DataMember(Name = "Created", Order = 5)]
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        /// <value>
        /// The last modified.
        /// </value>
        [DataMember(Name = "LastModified", Order = 6)]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the last acess.
        /// </summary>
        /// <value>
        /// The last acess.
        /// </value>
        [DataMember(Name = "LastAcess", Order = 7)]
        public DateTime LastAcess { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        [DataMember(Name = "Attributes", Order = 8)]
        public System.IO.FileAttributes Attributes { get; set; }
    }
}
