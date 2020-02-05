
namespace BadCopy.Core.Transformations
{
    abstract public class Transformation
    {
        public string Transform(string input)
        {
            var rows = StringToRows(input);

            var arr = Transform(rows);

            return RowsToString(arr);
        }

        public abstract string[] Transform(string[] rows);

        protected string[] StringToRows(string input)
        {
            input = input.Replace("\r\n", "\n");
            return input.Split('\n');
        }

        protected string RowsToString(string[] rows)
        {
            return string.Join('\n', rows);
        }
    }
}
