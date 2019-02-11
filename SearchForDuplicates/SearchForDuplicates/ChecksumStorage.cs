using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using GeneralToolkitLib.Hashing;
using GeneralToolkitLib.Log;

namespace SearchForDuplicates
{
    public class ChecksumStorage
    {
        public bool HasChanged { get; set; }
    }
}