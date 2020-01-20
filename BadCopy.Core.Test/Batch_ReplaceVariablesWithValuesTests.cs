using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace BadCopy.Core.Test
{
    [TestClass]
    public class Batch_ReplaceVariablesWithValuesTests
    {
        [TestMethod]
        public void should_replace_variable_with_value_real()
        {
            var b = new Batch
            {
                Variables = new List<Variable> {
                    new Variable{Name="[ProjectPath]", Value="C:\\Project\\Hello"},
                },
                FromFolderBase = "[ProjectPath]\\Subdirectory"
            };

            Assert.AreEqual("C:\\Project\\Hello\\Subdirectory", b.FromFolderBase);
        }

        [TestMethod]
        public void should_replace_variable_with_value_academic()
        {
            var b = new Batch
            {
                Variables = new List<Variable> {
                    new Variable{Name="[ABC]", Value="__"},
                },
                FromFolderBase = "1111[ABC]2222[ABC]"
            };

            Assert.AreEqual("1111__2222__", b.FromFolderBase);
        }
    }
}
