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
        [TestMethod]
        public void copy_one_file_without_solution()
        {
            List<FileInfo> files = new List<FileInfo>
            {
                new FileInfo{
                    BatchName="First batch",
                    FromFile= Common.Scenario01Root + "Input\\A\\1.txt",
                    ToFile = Common.Scenario01Root + "Output\\A\\1.txt",
                    CopyStyle=CopyStyle.NoSolution
                }
            };

            var bs = new BadCopyService();

            BadCopyService.CopyResult result = bs.Copy(files);

            Assert.IsTrue(result.AllSucceded);

            CompareContentOfFiles(Common.Scenario01Root + "Output\\A\\1.txt", Common.Scenario01Root + "ExpectedOutput\\A\\1.txt");

        }

        [TestMethod]
        public void clone_one_file()
        {
            List<FileInfo> files = new List<FileInfo>
            {
                new FileInfo{
                    BatchName="First batch",
                    FromFile= Common.Scenario01Root + "Input\\A\\2-Clone.txt",
                    ToFile = Common.Scenario01Root + "Output\\A\\2-Clone.txt",
                    CopyStyle=CopyStyle.Clone
                }
            };

            var bs = new BadCopyService();

            BadCopyService.CopyResult result = bs.Copy(files);

            Assert.IsTrue(result.AllSucceded);

            CompareContentOfFiles(Common.Scenario01Root + "Output\\A\\2-Clone.txt", Common.Scenario01Root + "ExpectedOutput\\A\\2-Clone.txt");

        }

        private void CompareContentOfFiles(string v1, string v2)
        {
            var actual = File.ReadAllText(v1).Replace("\r\n", "\n");
            var expected = File.ReadAllText(v2).Replace("\r\n", "\n");

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
