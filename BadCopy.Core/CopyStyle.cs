using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core
{
    public enum CopyStyle
    {
        Unknown,          // kopieringstilen är inte angiven (troligen pga ett fel)
        NoSolution,       // kopia men ha inte med #region solution .... #endregion
        NoSolutionNoHash, // samma som ovan men ta även bort själva regiontaggen (#region)
        Clone,            // exakt kopia av filen 
    }
}
