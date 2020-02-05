using System.Text.RegularExpressions;

namespace BadCopy.Core.Transformations
{
    public class ReplaceSolutionRegionWithTodo: Transformation
    {
        public override string Transform(string input)
        {
            input = new TransformUtility().AdjustNewLine(input);

            // todo: snyggare sätt där detta inte behövs?

            if (input.Trim().EndsWith("#endregion"))
                input += "\n";

            return Regex.Replace(input, @"[ \t]*#region solution\s*\n[\s\S]*?\n\s*#endregion[ \t]*\n", "\n// todo: add code here\n", RegexOptions.Multiline);
        }
    }
}
