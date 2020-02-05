using System;
using System.Linq;
using BadCopy.Core.Transformations;

namespace BadCopy.Core
{
    public class FileInfo // : IEquatable<FileInfo>
    {
        public string BatchName { get; set; }
        public string FromFile { get; set; }
        public string ToFile { get; set; }
        public Action Action { get; set; }
        public Transformation[] Transformations { get; set; } = new Transformation[] { };
        public bool Binary { get; set; }

        public bool Equals(FileInfo other)
        {
            return
                BatchName == other.BatchName &&
                FromFile == other.FromFile &&
                ToFile == other.ToFile &&
                Action == other.Action &&
                Binary == other.Binary &&
                SameTransformations(Transformations, other.Transformations);
        }

        private static bool SameTransformations(Transformation[] t1, Transformation[] t2)
        {
            for (int i = 0; i < t1.Length; i++)
            {
                if (t1[i].GetType() != t2[i].GetType())
                    return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals((FileInfo)obj);
        }

        internal static FileInfo Clone(FileInfo file)
        {
            return new FileInfo
            {
                Action = file.Action,
                BatchName = file.BatchName,
                Binary = file.Binary,
                FromFile = file.FromFile,
                ToFile = file.ToFile,
                Transformations = (Transformation[])file.Transformations.Clone() // funkar detta? 
            };
        }

        // används denna?
        public override int GetHashCode()
        {
            return HashCode.Combine(BatchName, FromFile, ToFile, Action, Transformations, Binary);
        }
    }
}
