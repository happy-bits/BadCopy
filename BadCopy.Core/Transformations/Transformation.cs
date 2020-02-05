using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core.Transformations
{
    abstract public class Transformation
    {
        public abstract string Transform(string rows);
    }
}
