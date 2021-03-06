﻿using BadCopy.Core.Transformations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadCopy.Core;
using static BadCopy.Core.Test.Scenario01.Common;

namespace BadCopy.Core.Test.Scenario01
{
    // todo: tester för fel

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
                    Action=Action.Transform,
                    Transforms = new Transform[] { new Transform(new RemoveSolutionRegion()) }
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
                    Action=Action.Copy
                }
            };

            var bs = new BadCopyService();

            BadCopyService.CopyResult result = bs.Copy(files);

            Assert.IsTrue(result.AllSucceded);

            CompareContentOfFiles(ExpectedOutputFile("A\\2-Clone.txt"), OutputFile("A\\2-Clone.txt"));

        }

        [TestMethod]
        public void copy_multiple_files()
        {
            List<FileInfo> files = new List<FileInfo>
            {
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\1.txt"),
                    ToFile = OutputFile("A\\1.txt"),
                    Action=Action.Transform,
                    Transforms = new[] { new Transform(new RemoveSolutionRegion()) }
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\Sub1\\Sub2\\6.txt"),
                    ToFile = OutputFile("A\\Sub1\\Sub2\\6.txt"),
                    Action=Action.Transform,
                    Transforms = new[] { new Transform(new RemoveSolutionRegion()) }
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\2-Clone.txt"),
                    ToFile = OutputFile("A\\2-Clone.txt"),
                    Action=Action.Copy,
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\3-Simple.txt"),
                    ToFile = OutputFile("A\\3-Simple.txt"),
                    Action=Action.Transform,
                    Transforms = new[] { new Transform(new RemoveSolutionRegion()) }
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\4-Multiple.txt"),
                    ToFile = OutputFile("A\\4-Multiple.txt"),
                    Transforms = new[] { new Transform(new RemoveSolutionRegion()) },
                    Action=Action.Transform
                }
                ,
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("B\\5-Onemore.txt"),
                    Transforms = new[] { new Transform(new RemoveSolutionRegion()) },
                    ToFile = OutputFile("B\\5-Onemore.txt"),
                    Action=Action.Transform
                },
            };

            var bs = new BadCopyService();

            BadCopyService.CopyResult result = bs.Copy(files);

            Assert.IsTrue(result.AllSucceded);

            CompareContentOfFiles(OutputFile("A\\1.txt"), ExpectedOutputFile("A\\1.txt"));
            CompareContentOfFiles(OutputFile("A\\Sub1\\Sub2\\6.txt"), ExpectedOutputFile("A\\Sub1\\Sub2\\6.txt"));
            CompareContentOfFiles(OutputFile("A\\2-Clone.txt"), ExpectedOutputFile("A\\2-Clone.txt"));
            CompareContentOfFiles(OutputFile("A\\3-Simple.txt"), ExpectedOutputFile("A\\3-Simple.txt"));
            CompareContentOfFiles(OutputFile("A\\4-Multiple.txt"), ExpectedOutputFile("A\\4-Multiple.txt"));
            CompareContentOfFiles(OutputFile("B\\5-Onemore.txt"), ExpectedOutputFile("B\\5-Onemore.txt"));

        }


        private void CompareContentOfFiles(string actualFilename, string expectedFilename)
        {
            var actual = File.ReadAllText(actualFilename).Replace("\r\n", "\n");
            var expected = File.ReadAllText(expectedFilename).Replace("\r\n", "\n");

            Assert.AreEqual(expected, actual);

        }

    }
}
