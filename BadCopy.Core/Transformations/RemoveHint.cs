using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// todo: väldigt klumpig metod. Går det att göra den mer generisk och lättläst?

namespace BadCopy.Core.Transformations
{
    public class RemoveHint : Transformation
    {
        public override string[] Transform(string[] rows)
        {
            var input = RowsToString(rows);

            // todo: snyggare sätt där detta inte behövs?

            var result = Regex.Replace(input, @"[ \t]*## Hint\s*\n[\s\S]*?\n\s*##", "##", RegexOptions.Multiline);


            return StringToRows(result);
        }

        //public override string[] Transform(string[] rows)
        //{

        //    var regions = new List<Region>();
        //    var nextRegion = new Region();

        //    for (int i = 0; i < rows.Length; i++)
        //    {
        //        var row = rows[i].Trim();

        //        bool isLastRow = i == rows.Length - 1;
        //        bool regionStartIsSet = nextRegion.StartLineNumber != null;
        //        bool regionEndIsSet = nextRegion.EndLineNumber != null;

        //        if (isLastRow && regionStartIsSet && !regionEndIsSet)
        //        {
        //            nextRegion.EndLineNumber = i;
        //            if (!row.StartsWith("##"))
        //                nextRegion.EndLineNumber = i + 1;
        //            regions.Add(nextRegion);
        //        }
        //        else if (row == "## Hint")
        //        {
        //            nextRegion.StartLineNumber = i;
        //        }
        //        else if (row.StartsWith("##") && regionStartIsSet && !regionEndIsSet)
        //        {
        //            nextRegion.EndLineNumber = i;
        //            regions.Add(nextRegion);
        //            nextRegion = new Region();
        //        }

        //    }



        //    var result = rows.ToList();
            
        //    int nrOfRegions = regions.Count;

        //    for (int i = nrOfRegions - 1; i >= 0; i--)
        //    {
        //        var region = regions[i];
        //        var nrOfRowsToDelete = (int)(region.EndLineNumber - region.StartLineNumber);
        //        result.RemoveRange((int)region.StartLineNumber, nrOfRowsToDelete);
        //    }

        //    return result.ToArray();


        //}

        //class Region
        //{
        //    public int? StartLineNumber { get; set; }
        //    public int? EndLineNumber { get; set; }
        //}
    }
}
