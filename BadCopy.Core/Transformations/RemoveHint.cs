using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BadCopy.Core.Transformations
{
    public class RemoveHint : Transformation
    {

        public override string[] Transform(string[] rows)
        {

            var result = new List<string>();
            bool keep = true;

            foreach (var row in rows)
            {
                if (IsHintHeader(row))
                {
                    keep = false;
                } else if (IsHeader(row))
                {
                    keep = true;
                    result.Add(row);
                } else
                {
                    if (keep)
                        result.Add(row);
                }
            }
            return result.ToArray();

        }

        private bool IsHeader(string row)
        {
            return Regex.IsMatch(row, "[ \t]*##[ \t]*");
        }

        private bool IsHintHeader(string row)
        {
            return Regex.IsMatch(row, "[ \t]*## Hint[ \t]*");
        }

    }
}
