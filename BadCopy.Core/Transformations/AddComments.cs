using System;
using System.Linq;

namespace BadCopy.Core.Transformations
{
    public class AddComments : Transformation
    {
        public override string Transform(string input)
        {
            input = new TransformUtility().AdjustNewLine(input);

            var rows = input.Split('\n');
            var rowsWithComments = rows.Select(r => AddCommentRowHasContent(r));
            var result = string.Join('\n', rowsWithComments);

            return result;
        }

        private string AddCommentRowHasContent(string row)
        {
            if (string.IsNullOrWhiteSpace(row))
                return row;
            return "// " + row;
        }
    }
}
