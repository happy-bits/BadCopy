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
    public class TransformUtility_GetAllMethodSignatures
    {
        [TestMethod]
        public void real_world_example()
        {
            var x = new TransformUtility();

            var s = @"

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workout.Test.Level1
{

    [TestClass]
    public class Add100ToEachNumberTest
    {

        [TestMethod]
        public void test1()
        {
            var input = new List<int> { 5, 15, 23, 200 };
            var expected = new List<int> { 105, 115, 123, 300 };
            var result = Add100ToEachNumber(input);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void test2()
        {
            TTTTTTTTTTTTTTTTTTTTTTTTT2
            TTTTTTTTTTTTTTTTTTTTTTTTT2
        }

        [TestMethod]
        public void test3()
        {
            TTTTTTTTTTTTT3
            TTTTTTTTTTTTT3
            TTTTTTTTTTTTT3
        }

        public List<int> Meth1(List<int> numbers)
        {
            var result = new List<int>();
            foreach (var number in numbers)
            {
                int newNumber = number + 100;
                result.Add(newNumber);
            }

            return result;
        }

        public List<int> Meth2(List<int> numbers)
        {
            2222222222
        }

        public List<int> Meth3(List<int> numbers)
        {
            3333333333333333
        }

        public List<int> Meth4(List<int> numbers)
        {
            4444444444
        }
        
    }
}

";

            List<string> result =  x.GetAllMethodSignatures(s);

            CollectionAssert.AreEqual(new string[] {
            "public void test1()",
            "public void test2()",
            "public void test3()",
            "public List<int> Meth1(List<int> numbers)",
            "public List<int> Meth2(List<int> numbers)",
            "public List<int> Meth3(List<int> numbers)",
            "public List<int> Meth4(List<int> numbers)",
            }, result);

            List<string> result2 = x.GetAllNonTestMethodSignatures(s);

            CollectionAssert.AreEqual(new string[] {
            "public List<int> Meth1(List<int> numbers)",
            "public List<int> Meth2(List<int> numbers)",
            "public List<int> Meth3(List<int> numbers)",
            "public List<int> Meth4(List<int> numbers)",
            }, result2);




        }

    }
}
