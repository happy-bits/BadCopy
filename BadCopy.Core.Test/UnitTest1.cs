using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BadCopy.Core.Test
{
    [TestClass]
    public class UnitTest1
    {
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
    }
}
