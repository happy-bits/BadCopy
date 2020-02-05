using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BadCopy.Core.Transformations
{
    public class RemoveTodo : Transformation
    {
        public override string Transform(string input)
        {
            input = new TransformUtility().AdjustNewLine(input);

            var rows = input.Split('\n');

            var rowsWithoutTodo = rows.Where(r => !RowContainTodoComment(r));

            var result = string.Join('\n', rowsWithoutTodo);

            return result;
        }

        private bool RowContainTodoComment(string row)
        {
            return Regex.IsMatch(row, @"^\s*//\s*todo\:");
        }
    }
}
