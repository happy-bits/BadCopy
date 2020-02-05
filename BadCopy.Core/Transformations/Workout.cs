using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core.Transformations
{
    public class Workout : Transformation
    {
        public override string[] Transform(string[] rows)
        {
            // todo: här hoppar vi tillbaka från array till sträng. Finns alternativt sätt?
            var _utility = new TransformUtility();
            string input = string.Join('\n', rows);

            string firstSolutionMethod = _utility.GetAllNonTestMethodSignatures(input).First();

            int index = input.IndexOf(firstSolutionMethod);

            var beforeSolution = input.Substring(0, index).TrimEnd() + "\n\n";

            string result = beforeSolution;
            string tabbing = "    ";
            foreach (string method in _utility.GetAllNonTestMethodSignatures(input))
            {
                string methodWithBody = _utility.GenerateNotImplementedMethod(method, tabbing + tabbing, tabbing);// "        "
                result += methodWithBody + "\n";
            }
            result += tabbing + "}\n";
            result += "}\n";


            return result.Split('\n');

        }
    }
}
