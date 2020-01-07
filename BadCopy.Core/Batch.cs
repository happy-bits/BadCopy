using System.Collections.Generic;

namespace BadCopy.Core
{
    public class Batch
    {
        public string Name { get; set; }
        public List<string> FromFolders { get; set; }
        public CopyStyle CopyStyle { get; set; }
        public string ToFolder { get; set; }
    }
}
