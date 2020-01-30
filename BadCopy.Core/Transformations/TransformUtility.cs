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
        public List<string> GetMethodsRows(string s)
        {
            //var result = Regex.Match(s, @"((\s*?)( *(public|private).*).*)+", RegexOptions.);
            var result = new List<string>();

            var splitt = s.Split("public");

            for(var i=1; i<splitt.Length; i++)
            {
                result.Add("public" + splitt[i].Split('\n','\r')[0]);
            }

            return result;
        }
    }
}
