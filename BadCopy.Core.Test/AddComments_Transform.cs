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
    public class AddComments_Transform
    {

        [TestMethod]
        public void two_lines()
        {
            var ac = new AddComments();
            var result = ac.Transform("aaa\nbbb");
            var expected = "// aaa\n// bbb";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void three_lines_with_one_empty()
        {
            var ac = new AddComments();
            var result = ac.Transform("aaa\nbbb\n");
            var expected = "// aaa\n// bbb\n";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void just_empty_lines()
        {
            var ac = new AddComments();
            var result = ac.Transform(" \n   \t    \n");
            var expected = " \n   \t    \n";

            Assert.AreEqual(expected, result);
        }
    }
}
