using BadCopy.Core.Transformations;

namespace BadCopy.Core
{
    public struct FileInfo
    {
        public string BatchName { get; set; }
        public string FromFile { get; set; }
        public string ToFile { get; set; }
        public Action Action { get; set; }
        public Transformation[] Transformations { get; set; }
        public bool Binary { get; set; }
    }
}
