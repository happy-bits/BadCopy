using System;
using System.Linq;

namespace BadCopy.Core.Transformations
{
    public class AddComments : Transformation
    {

        public override string[] Transform(string[] rows)
        {
            var rowsWithComments = rows.Select(r => AddCommentRowHasContent(r)).ToArray();
            return rowsWithComments;
        }

        private string AddCommentRowHasContent(string row)
        {
            if (string.IsNullOrWhiteSpace(row))
                return row;
            return "// " + row;
        }
    }
}
