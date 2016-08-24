using System;
using System.IO;
using System.Runtime.Serialization;

namespace ImageView
{
    [Serializable]
    public class ImageReferenceElement : ISerializable, IComparable<ImageReferenceElement>, IEquatable<ImageReferenceElement>
    {
        public ImageReferenceElement()
        {
        }

        protected ImageReferenceElement(SerializationInfo info, StreamingContext context)
        {
            Directory = info.GetString("Directory");
            FileName = info.GetString("FileName");
            CompletePath = info.GetString("CompletePath");

            Size = info.GetInt64("Size");
            CreationTime = info.GetDateTime("CreationTime");
            LastWriteTime = info.GetDateTime("LastWriteTime");
            LastAccessTime = info.GetDateTime("LastAccessTime");
        }

        public string Directory { get; set; }
        public string FileName { get; set; }
        public string CompletePath { get; set; }
        public long Size { get; set; }

        public string SizeInKb
        {
            get { return Math.Round(Size/1024d, 1) + " kB"; }
        }

        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }

        public string FileExtention
        {
            get
            {
                if (FileName != null)
                    return Path.GetExtension(FileName);
                return null;
            }
        }

        public int CompareTo(ImageReferenceElement other)
        {
            return String.CompareOrdinal(CompletePath, other.CompletePath);
        }

        public bool Equals(ImageReferenceElement other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Directory, other.Directory) && string.Equals(FileName, other.FileName) && string.Equals(CompletePath, other.CompletePath) && Size == other.Size && CreationTime.Equals(other.CreationTime) &&
                   LastWriteTime.Equals(other.LastWriteTime) && LastAccessTime.Equals(other.LastAccessTime);
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Directory", Directory);
            info.AddValue("FileName", FileName);
            info.AddValue("CompletePath", CompletePath);

            info.AddValue("Size", Size);
            info.AddValue("CreationTime", CreationTime);
            info.AddValue("LastWriteTime", LastWriteTime);
            info.AddValue("LastAccessTime", LastAccessTime);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Directory != null ? Directory.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (FileName != null ? FileName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CompletePath != null ? CompletePath.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Size.GetHashCode();
                hashCode = (hashCode*397) ^ CreationTime.GetHashCode();
                hashCode = (hashCode*397) ^ LastWriteTime.GetHashCode();
                hashCode = (hashCode*397) ^ LastAccessTime.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ImageReferenceElement) obj);
        }


        public static bool operator ==(ImageReferenceElement c1, ImageReferenceElement c2)
        {
            if (ReferenceEquals(c1, c2))
                return true;

            if (((object) c1 == null) || ((object) c2 == null))
                return false;

            return c1.Equals(c2);
        }

        public static bool operator !=(ImageReferenceElement c1, ImageReferenceElement c2)
        {
            return !(c1 == c2);
        }
    }
}