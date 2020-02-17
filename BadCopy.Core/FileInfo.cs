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
        public bool Binary { get; set; }
        public Transform[] Transforms { get; set; } = new Transform[] { };
        public string Extension => FromFile.Split('.').Last();

        public bool Equals(FileInfo other)
        {
            return
                BatchName == other.BatchName &&
                FromFile == other.FromFile &&
                ToFile == other.ToFile &&
                Action == other.Action &&
                Binary == other.Binary &&
                SameTransformations(Transforms, other.Transforms);
        }

        private static bool SameTransformations(Transform[] t1, Transform[] t2)
        {
            for (int i = 0; i < t1.Length; i++)
            {
                if (t1[i].Transformation.GetType() != t2[i].Transformation.GetType())
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
                Transforms = (Transform[])file.Transforms.Clone() // funkar detta? 
            };
        }

        // används denna?
        public override int GetHashCode()
        {
            return HashCode.Combine(BatchName, FromFile, ToFile, Action, Transforms, Binary);
        }
    }
}
