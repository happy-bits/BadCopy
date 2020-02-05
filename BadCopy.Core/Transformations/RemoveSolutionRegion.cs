using System.Text.RegularExpressions;

namespace BadCopy.Core.Transformations
{
    public class RemoveSolutionRegion : Transformation
    {
        public override string[] Transform(string[] rows)
        {

            var input = RowsToString(rows);

            // todo: snyggare sätt där detta inte behövs?

            if (input.Trim().EndsWith("#endregion"))
                input += "\n";

            var result = Regex.Replace(input, @"[ \t]*#region solution\s*\n[\s\S]*?\n\s*#endregion[ \t]*\n", "", RegexOptions.Multiline);

            return StringToRows(result);

        }
    }
}

/*
 
    Alternativ utan regex:
     
    public class RemoveSolutionRegion : Transformation
    {
        class Region
        {
            public int? StartLineNumber { get; set; }
            public int? EndLineNumber { get; set; }
        }

        public override string[] Transform(string[] rows)
        {
            var regions = new List<Region>();
            var nextRegion = new Region();

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i].Trim() == "#region solution")
                {
                    if (nextRegion.StartLineNumber != null || nextRegion.EndLineNumber != null)
                        throw new Exception($"Didn't expect start region at row {i}");

                    nextRegion.StartLineNumber = i;
                }

                if (rows[i].Trim() == "#endregion")
                {
                    if (nextRegion.StartLineNumber == null || nextRegion.EndLineNumber != null)
                        throw new Exception($"Didn't expect end region at row {i}");

                    nextRegion.EndLineNumber = i;
                    regions.Add(nextRegion);
                    nextRegion = new Region();
                }
            }

            var result = rows.ToList();
            int nrOfRegions = regions.Count;

            for (int i = nrOfRegions - 1; i >0; i--)
            {
                var region = regions[i];
                var nrOfRowsToDelete = (int)(region.EndLineNumber - region.StartLineNumber);
                result.RemoveRange((int)region.StartLineNumber, nrOfRowsToDelete);
            }

            return result.ToArray();

        }
    }


     */
