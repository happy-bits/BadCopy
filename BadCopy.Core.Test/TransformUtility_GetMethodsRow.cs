using BadCopy.Core.Transformations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core.Test
{
    [TestClass]
    public class TransformUtility_GetMethodsRow
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var x = new TransformUtility();

            var s = @"

                public List<int> Add100ToEachNumber(List<int> numbers)
                {
                    var result = new List<int>();
                    foreach (var number in numbers)
                    {
                        int newNumber = number + 100;
                        result.Add(newNumber);
                    }

                    return result;
                }

                public List<int> Add100ToEachNumber_Linq(List<int> numbers)
                {
                    return numbers.Select(x => x + 100).ToList();
                }

            ";

            List<string> result =  x.GetMethodsRows(s);

            CollectionAssert.AreEqual(new string[] {
            "public List<int> Add100ToEachNumber(List<int> numbers)",
            "public List<int> Add100ToEachNumber_Linq(List<int> numbers)"
            }, result);

        }
    }
}
