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


        public List<string> GetAllMethodSignatures(string s)
        {
            return Regex.Matches(s, @"([ \t]*public .*\(.*)").Select(x=>x.Value.Trim()).ToList();
        }

        public List<string> GetAllNonTestMethodSignatures(string s)
        {
            var testMethods = GetTestMethodSignatures(s);
            var all = GetAllMethodSignatures(s);
            all.RemoveAll(x => testMethods.Contains(x));
            return all;
        }

        public string GenerateNotImplementedMethod(string methodSignature, string whitespaces, string tabbing)
        {
            char leftBird = '{';
            char rightBird = '}';
            
            string result = "";
            result += $"{whitespaces}{methodSignature}\n";
            result += $"{whitespaces}{leftBird}\n";
            result += $"{whitespaces}{tabbing}throw new NotImplementedException();\n";
            result += $"{whitespaces}{rightBird}\n";
            return result;
        }
    }
}
