using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core.Test
{
    [TestClass]
    public class BadCopyConfigFileTests
    {
        [TestMethod]
        public void merge_configuration()
        {
            var b = new BadCopyConfigFile
            {
                ReplaceSolutionWith = "A",
                FromFolderBase = "B",
                CopyStyle = CopyStyle.NoSolutionNoHash,
                SpecificFiles = new List<string> { "C", "D", "E" },
                SkipFolders = new List<string> { "H", "I", "J" },
                Batches = new List<Batch>
                {
                    new Batch
                    {
                        Name="F",
                        FromFolderBase="G",
                        SkipFolders = new List<string> { "K", "L" },
                    }
                }
            };
            
            var result = b.MergeConfiguration();

            Assert.AreEqual("A", result.ReplaceSolutionWith);
            Assert.AreEqual("G", result.Batches[0].FromFolderBase);
            Assert.AreEqual(CopyStyle.NoSolutionNoHash, result.Batches[0].CopyStyle);
            CollectionAssert.AreEqual(new List<string> { "C", "D", "E" }, result.Batches[0].SpecificFiles);
            CollectionAssert.AreEqual(new List<string> { "K", "L" }, result.Batches[0].SkipFolders);
        }    
    }
}
