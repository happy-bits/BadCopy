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
    public class TransformUtility_GetTestMethodSignatures
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

            List<string> result =  x.GetTestMethodSignatures(s);

            CollectionAssert.AreEqual(new string[] {
            "public void test1()",
            "public void test2()",
            "public void test3()",
            }, result);

        }

        [TestMethod]
        public void should_find_five_testmethods()
        {
            var s = @"

                    {

                        [TestMethod]
                        public void test1()
                        {
                        }

                        [TestMethod]
                        public void test2()
                        {
                        }

                        [TestMethod]
                        public void test3()
                        {
                        }

                        [TestMethod]
                        public void test4()
                        {
                        }

                        [TestMethod]
                        public void test5()
                        {
                        }

                        public void test6()
                        {
                        }

                        public void test7()
                        {
                        }
                    ";

            List<string> result = new TransformUtility().GetTestMethodSignatures(s);

            CollectionAssert.AreEqual(new string[] {
            "public void test1()",
            "public void test2()",
            "public void test3()",
            "public void test4()",
            "public void test5()",
            }, result);
        }

        [TestMethod]
        public void should_not_find_any_testmethods()
        {
            var s = @"

                        aaaa
                        aaaa

                        public void test1()
                        {
                        }

                        public void test2()
                        {
                        }

                    ";

            List<string> result = new TransformUtility().GetTestMethodSignatures(s);

            CollectionAssert.AreEqual(new string[] {}, result);
        }
    }
}
