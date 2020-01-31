using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BadCopy.Core.Transformations
{
    public class TransformUtility
    {

        public List<string> GetTestMethodSignatures(string s)
        {
            return Regex.Matches(s, @"(\[TestMethod\]\s*)(public.*)").Select(x=>x.Groups[2].Value.Trim()).ToList();
        }
    }
}
