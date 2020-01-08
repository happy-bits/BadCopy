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
            var x = new BadCopyService();
            var result = x.RemoveSolutionRegion("aa\n#region solution\nbb\n#endregion\ncc");
            Assert.AreEqual("aa\ncc", result);
        }

        [TestMethod]
        public void more_spaces()
        {
            var x = new BadCopyService();
            var result = x.RemoveSolutionRegion("aa\n   #region solution  \n  b  \n  b  \n  #endregion    \ncc");
            Assert.AreEqual("aa\ncc", result);
        }

        [TestMethod]
        public void double_regions()
        {
            var x = new BadCopyService();
            var result = x.RemoveSolutionRegion("aa\n#region solution\n  bbb  \n#endregion\ncc\n#region solution\n  ddd  \n#endregion\nee");
            Assert.AreEqual("aa\ncc\nee", result);
        }


        [TestMethod]
        public void spaces_and_tabs()
        {

            string input = "\t aaa \t\n \t \t #region solution    \nbbb\n    #endregion\t    \n\t ccc";
            var x = new BadCopyService();
            var result = x.RemoveSolutionRegion(input);
            Assert.AreEqual("\t aaa \t\n\t ccc", result);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            string input = "\r\n\t\taaa\r\n\t\t#region solution\r\n\t\t\tbbb                \r\n\t\t#endregion\r\n\t\tccc";
            var x = new BadCopyService();
            var result = x.RemoveSolutionRegion(input);
            Assert.AreEqual("\n\t\taaa\n\t\tccc", result);
        }
    }
}
