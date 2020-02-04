using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using static BadCopy.Core.Test.Scenario01.Common;


namespace BadCopy.Core.Test.Scenario01
{
    [TestClass]
    public class GetFilesToCopyTests
    {

        [TestMethod]
        public void should_give_correct_filepaths_from_a_batch()
        {
            var b1 = new Batch
            {
                Name = "First batch",
                FromFolderBase = Scenario01Root + "Input",
                FromFolders = new List<string> {
                    "A",
                    "B",
                },
                Action = Action.Transform,
                ToFolder = Scenario01Root + "Output",
            };

            var bs = new BadCopyService();
            List<FileInfo> result = bs.GetFilesToCopy(b1);

            List<FileInfo> expected = new List<FileInfo>
            {
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\1.txt"),
                    ToFile = OutputFile("A\\1.txt"),
                    Action = Action.Transform
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\2-Clone.txt"),
                    ToFile = OutputFile("A\\2-Clone.txt"),
                    Action = Action.Transform
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\3-Simple.txt"),
                    ToFile = OutputFile("A\\3-Simple.txt"),
                    Action = Action.Transform
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\4-Multiple.txt"),
                    ToFile = OutputFile("A\\4-Multiple.txt"),
                    Action = Action.Transform
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("A\\Sub1\\Sub2\\6.txt"),
                    ToFile = OutputFile("A\\Sub1\\Sub2\\6.txt"),
                    Action = Action.Transform
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= InputFile("B\\5-Onemore.txt"),
                    ToFile = OutputFile("B\\5-Onemore.txt"),
                    Action =  Action.Transform
                },

            };

            CollectionAssert.AreEqual(expected, result);

        }
    }

}


//[TestMethod]
//public void TestMethod1()
//{
//    //JavascriptSerializer
//    string s = JsonConvert.SerializeObject(new Batch
//    {
//        CopyStyle = CopyStyle.NoSolution,
//        FromFolders = new List<string> { "fromfolder" },
//        Name = "name",
//        ToFolder = "tofolder"
//    });
//}
