namespace BadCopy.Core
{
    public class FileInfo
    {
        public string BatchName { get; set; }
        public string FromFile { get; set; }
        public string ToFile { get; set; }
        public CopyStyle CopyStyle { get; set; }
    }
}
