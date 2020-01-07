using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core.Test.Scenario01
{
    [TestClass]
    public class CopyTests
    {
        //[TestMethod]
        public void MyTestMethod()
        {
            List<FileInfo> files = new List<FileInfo>
            {
                new FileInfo{
                    BatchName="First batch",
                    FromFile= Common.Scenario01Root + "Input\\A\\File1.txt",
                    ToFile = Common.Scenario01Root + "Output\\A\\File1.txt",
                    CopyStyle=CopyStyle.NoSolution
                }
            };

            var bs = new BadCopyService();

            BadCopyService.CopyResult result = bs.Copy(files);

            Assert.IsTrue(result.AllSucceded);

            var actual = File.ReadAllText(Common.Scenario01Root + "Output\\A\\File1.txt");
            var expected = File.ReadAllText(Common.Scenario01Root + "ExpectedOutput\\A\\File1.txt");

            Assert.AreEqual(actual, expected);

        }

        //[TestMethod]
        //public void MyTestMethod2()
        //{
        //    List<FileInfo> expected = new List<FileInfo>
        //    {
        //        new FileInfo{
        //            BatchName="First batch",
        //            FromFile= Common.Scenario01Root + "Input\\A\\File1.txt",
        //            ToFile = Common.Scenario01Root + "Output\\A\\File1.txt",
        //            CopyStyle=CopyStyle.NoSolution
        //        },
        //        new FileInfo{
        //            BatchName="First batch",
        //            FromFile= Common.Scenario01Root + "Input\\A\\File1b.txt",
        //            ToFile = Common.Scenario01Root + "Output\\A\\File1b.txt",
        //            CopyStyle=CopyStyle.NoSolution
        //        },
        //        new FileInfo{
        //            BatchName="First batch",
        //            FromFile= Common.Scenario01Root + "Input\\B\\File2.txt",
        //            ToFile = Common.Scenario01Root + "Output\\B\\File2.txt",
        //            CopyStyle=CopyStyle.NoSolution
        //        },
        //        new FileInfo{
        //            BatchName="First batch",
        //            FromFile= Common.Scenario01Root + "Input\\C\\File3.txt",
        //            ToFile = Common.Scenario01Root + "Output\\C\\File3.txt",
        //            CopyStyle=CopyStyle.NoSolution
        //        },
        //    };

        //}    
    }
}
