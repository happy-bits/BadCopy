using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BadCopy.Core.Test.Scenario01.Common;

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
                    FromFile = InputFile("A\\1.txt"),
                    ToFile = OutputFile("A\\1.txt"),
                    CopyStyle=CopyStyle.NoSolution
                }
            };

            var bs = new BadCopyService();

            BadCopyService.CopyResult result = bs.Copy(files);

            Assert.IsTrue(result.AllSucceded);

            CompareContentOfFiles(OutputFile("A\\1.txt"), ExpectedOutputFile("A\\1.txt"));

        }


        [TestMethod]
        public void clone_one_file()
        {
            List<FileInfo> files = new List<FileInfo>
            {
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\2-Clone.txt"),
                    ToFile = OutputFile("A\\2-Clone.txt"),
                    CopyStyle=CopyStyle.Clone
                }
            };

            var bs = new BadCopyService();

            BadCopyService.CopyResult result = bs.Copy(files);

            Assert.IsTrue(result.AllSucceded);

            CompareContentOfFiles(OutputFile("A\\2-Clone.txt"), ExpectedOutputFile("A\\2-Clone.txt"));

        }

        [TestMethod]
        public void copy_four_files()
        {
            List<FileInfo> files = new List<FileInfo>
            {
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\1.txt"),
                    ToFile = OutputFile("A\\1.txt"),
                    CopyStyle=CopyStyle.NoSolution
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\2-Clone.txt"),
                    ToFile = OutputFile("A\\2-Clone.txt"),
                    CopyStyle=CopyStyle.Clone
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\3-Simple.txt"),
                    ToFile = OutputFile("A\\3-Simple.txt"),
                    CopyStyle=CopyStyle.NoSolution
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\4-Multiple.txt"),
                    ToFile = OutputFile("A\\4-Multiple.txt"),
                    CopyStyle=CopyStyle.NoSolution
                }
                ,
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("B\\5-Onemore.txt"),
                    ToFile = OutputFile("B\\5-Onemore.txt"),
                    CopyStyle=CopyStyle.NoSolution
                },
            };

            var bs = new BadCopyService();

            BadCopyService.CopyResult result = bs.Copy(files);

            Assert.IsTrue(result.AllSucceded);

            CompareContentOfFiles(OutputFile("A\\1.txt"), ExpectedOutputFile("A\\1.txt"));
            CompareContentOfFiles(OutputFile("A\\2-Clone.txt"), ExpectedOutputFile("A\\2-Clone.txt"));
            CompareContentOfFiles(OutputFile("A\\3-Simple.txt"), ExpectedOutputFile("A\\3-Simple.txt"));
            CompareContentOfFiles(OutputFile("A\\4-Multiple.txt"), ExpectedOutputFile("A\\4-Multiple.txt"));
            CompareContentOfFiles(OutputFile("B\\5-Onemore.txt"), ExpectedOutputFile("B\\5-Onemore.txt"));

        }


        private void CompareContentOfFiles(string v1, string v2)
        {
            var actual = File.ReadAllText(v1).Replace("\r\n", "\n");
            var expected = File.ReadAllText(v2).Replace("\r\n", "\n");

            Assert.AreEqual(actual, expected);

        }

    }
}
