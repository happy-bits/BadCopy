using System;
using System.Collections.Generic;

namespace BadCopy.Core
{
    public class FileInfo //: IEquatable<FileInfo>
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

        public override bool Equals(object otherobject)
        {
            var other = (FileInfo)otherobject;
            bool result = BatchName == other.BatchName &&
                FromFile == other.FromFile &&
                ToFile == other.ToFile &&
                CopyStyle == other.CopyStyle;
            return result;
        }

        public override int GetHashCode()
        {
            var hashCode = -303826034;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BatchName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FromFile);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ToFile);
            hashCode = hashCode * -1521134295 + CopyStyle.GetHashCode();
            return hashCode;
        }

        //public bool Equals(FileInfo other)
        //{
        //    bool result = BatchName == other.BatchName &&
        //        FromFile == other.FromFile &&
        //        ToFile == other.ToFile &&
        //        CopyStyle == other.CopyStyle;
        //    return result;

        //}
    }
}
