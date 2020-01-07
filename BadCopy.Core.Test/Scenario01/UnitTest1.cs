using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BadCopy.Core.Test.Scenario01
{
    [TestClass]
    public class UnitTest1
    {
        const string TestProjectRoot = "C:\\Project\\BadCopy\\BadCopy.Core.Test\\";
        const string Scenario01Root = TestProjectRoot + "Scenario01\\";

        [TestMethod]
        public void TestMethod1()
        {
            //JavascriptSerializer
            string s = JsonConvert.SerializeObject(new Batch
            {
                CopyStyle = CopyStyle.NoSolution,
                FromFolders = new List<string> { "fromfolder" },
                Name = "name",
                ToFolder = "tofolder"
            });
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var b1 = new Batch
            {
                Name = "First batch",
                FromFolderBase = Scenario01Root + "Input",
                FromFolders = new List<string> {
                    "A",
                    "B",
                    "C"
                },
                CopyStyle = CopyStyle.NoSolution,
                ToFolder = Scenario01Root + "Output",
                SearchPattern="*"
            };

            var bs = new BadCopyService();
            List<FileInfo> result = bs.GetFilesToCopy(b1);

            List<FileInfo> expected = new List<FileInfo>
            {
                new FileInfo{ 
                    BatchName="First batch", 
                    FromFile= Scenario01Root + "Input\\A\\File1.txt", 
                    ToFile = Scenario01Root + "Output\\A\\File1.txt", 
                    CopyStyle=CopyStyle.NoSolution
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= Scenario01Root + "Input\\A\\File1b.txt",
                    ToFile = Scenario01Root + "Output\\A\\File1b.txt",
                    CopyStyle=CopyStyle.NoSolution
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= Scenario01Root + "Input\\B\\File2.txt",
                    ToFile = Scenario01Root + "Output\\B\\File2.txt",
                    CopyStyle=CopyStyle.NoSolution
                },
                new FileInfo{
                    BatchName="First batch",
                    FromFile= Scenario01Root + "Input\\C\\File3.txt",
                    ToFile = Scenario01Root + "Output\\C\\File3.txt",
                    CopyStyle=CopyStyle.NoSolution
                },
            };

            Assert.AreEqual(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(result));

        }
    }

}
        //Ta reda på vilka filer som ska kopieras 
        //     - Ta hänsyn till historik? (för att slippa kopiera filen om det redan är gjort) 
        //     - Utgå ifrån fil med mappnamn och rootnamn

