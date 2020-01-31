using BadCopy.Core.Transformations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core.Test.Scenario01
{
    [TestClass]
    public class RemoveSolutionRegionTests
    {

        [TestMethod]
        public void simple()
        {
            var x = new RemoveSolutionRegion();
            var result = x.Transform("aa\n#region solution\nbb\n#endregion\ncc");
            Assert.AreEqual("aa\ncc", result);
        }

        [TestMethod]
        public void more_spaces()
        {
            var x = new RemoveSolutionRegion();
            var result = x.Transform("aa\n   #region solution  \n  b  \n  b  \n  #endregion    \ncc");
            Assert.AreEqual("aa\ncc", result);
        }

        [TestMethod]
        public void double_regions()
        {
            var x = new RemoveSolutionRegion();
            var result = x.Transform("aa\n#region solution\n  bbb  \n#endregion\ncc\n#region solution\n  ddd  \n#endregion\nee");
            Assert.AreEqual("aa\ncc\nee", result);
        }


        [TestMethod]
        public void spaces_and_tabs()
        {

            string input = "\t aaa \t\n \t \t #region solution    \nbbb\n    #endregion\t    \n\t ccc";
            var x = new RemoveSolutionRegion();
            var result = x.Transform(input);
            Assert.AreEqual("\t aaa \t\n\t ccc", result);
        }

        [TestMethod]
        public void different_type_of_line_breaks()
        {
            string input = "\r\n\t\taaa\r\n\t\t#region solution\r\n\t\t\tbbb                \r\n\t\t#endregion\r\n\t\tccc";
            var x = new RemoveSolutionRegion();
            var result = x.Transform(input);
            Assert.AreEqual("\n\t\taaa\n\t\tccc", result);
        }

        [TestMethod]
        public void starts_with_region()
        {
            var x = new RemoveSolutionRegion();
            var result = x.Transform("#region solution\nbb\n#endregion\ncc");
            Assert.AreEqual("cc", result);
        }

        [TestMethod]
        public void starts_with_region_with_spaces()
        {
            var x = new RemoveSolutionRegion();
            var result = x.Transform("\t \t\t   #region solution\nbb\n#endregion\ncc");
            Assert.AreEqual("cc", result);
        }

        [TestMethod]
        public void ends_with_region()
        {
            var x = new RemoveSolutionRegion();
            var result = x.Transform("aa\n#region solution\nbb\n#endregion");
            Assert.AreEqual("aa\n", result);
        }

        [TestMethod]
        public void ends_with_region_with_spaces()
        {
            var x = new RemoveSolutionRegion();
            var result = x.Transform("aa\n#region solution\nbb\n#endregion\t \t   ");
            Assert.AreEqual("aa\n", result);
        }
    }
}
