using System;
using System.Collections.Generic;

namespace BadCopy.Core
{
    public class FileInfo
    {
        public string BatchName { get; set; }
        public string FromFile { get; set; }
        public string ToFile { get; set; }
        public Action Action { get; set; }
        public bool Binary { get; set; }

        public static FileInfo Clone(FileInfo fi)
        {
            return new FileInfo
            {
                BatchName = fi.BatchName,
                FromFile = fi.FromFile,
                ToFile = fi.ToFile,
                Action = fi.Action,
                Binary = fi.Binary
            };
        }

        public override bool Equals(object otherobject)
        {
            var other = (FileInfo)otherobject;
            bool result = BatchName == other.BatchName &&
                FromFile == other.FromFile &&
                ToFile == other.ToFile &&
                Action == other.Action; 
            return result;
        }

        public override int GetHashCode()
        {
            var hashCode = -303826034;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BatchName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FromFile);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ToFile);
            hashCode = hashCode * -1521134295 + EqualityComparer<Action>.Default.GetHashCode(Action);
            return hashCode;
        }

    }
}
