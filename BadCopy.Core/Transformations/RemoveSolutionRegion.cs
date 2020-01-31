using System.Text.RegularExpressions;

namespace BadCopy.Core.Transformations
{
    public class RemoveSolutionRegion: Transformation
    {
        public override string Transform(string input)
        {
            input = input.Replace("\r\n", "\n");

            if (input.Trim().EndsWith("#endregion"))
                input += "\n";

            return Regex.Replace(input, @"[ \t]*#region solution\s*\n[\s\S]*?\n\s*#endregion[ \t]*\n", "", RegexOptions.Multiline);
        }
    }
}
