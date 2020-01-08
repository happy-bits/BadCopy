using System.Collections.Generic;

namespace BadCopy.Core
{
    public class Batch
    {
        public string Name { get; set; }
        public List<string> FromFolders { get; set; }
        public CopyStyle CopyStyle { get; set; }
        public string FromFolderBase { get; set; }
        public CopyFilter CopyFilter { get; set; }
        public string ToFolder { get; set; }
        public List<string> SpecificFiles { get; set; }
        public List<string> SpecificFileEndings { get; set; }
        public List<string> SkipFolders { get; set; }
    }
}
