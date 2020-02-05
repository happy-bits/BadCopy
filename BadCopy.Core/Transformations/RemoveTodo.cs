using System.Linq;
using System.Text.RegularExpressions;

namespace BadCopy.Core.Transformations
{
    public class RemoveTodo : Transformation
    {
        public override string[] Transform(string[] rows)
        {
            return rows.Where(r => !RowContainTodoComment(r)).ToArray();
        }

        private bool RowContainTodoComment(string row)
        {
            return Regex.IsMatch(row, @"^\s*//\s*todo\:");
        }
    }
}
