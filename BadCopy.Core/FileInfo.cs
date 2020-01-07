namespace BadCopy.Core
{
    public class FileInfo
    {
        public string BatchName { get; set; }
        public string FromFile { get; set; }
        public string ToFile { get; set; }
        public CopyStyle CopyStyle { get; set; }

        public static FileInfo Clone(FileInfo fi)
        {
            return new FileInfo
            {
                BatchName = fi.BatchName,
                FromFile = fi.FromFile,
                ToFile = fi.ToFile,
                CopyStyle = fi.CopyStyle
            };
        }
    }
}
