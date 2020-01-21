using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core.Test
{
    [TestClass]
    public class BadCopyService_DeleteFolderTests
    {
        [TestMethod]
        public void scenario_1()
        {
            if (Directory.Exists("A"))
                Directory.Delete("A", true);

            Directory.CreateDirectory(@"A");
            Directory.CreateDirectory(@"A\B");
            File.WriteAllText(@"A\B\textfile.txt", "should be gone"); // ska tas bort
            Directory.CreateDirectory(@"A\B\.vs");
            File.WriteAllText(@"A\B\.vs\textfile.txt", "this should stay"); 
            Directory.CreateDirectory(@"A\B\.vs\C");
            Directory.CreateDirectory(@"A\D"); // ska tas bort
            File.WriteAllText(@"A\D\textfile.txt", "should be gone"); // ska tas bort
            Directory.CreateDirectory(@"A\E"); // ska tas bort

            var nrDeleted = new BadCopyService().DeleteFolder("A");

            Assert.AreEqual(2, nrDeleted);

            string[] subDirs = GetAllSubdirectories("A");

            CollectionAssert.AreEquivalent(new []
            {
                @"A\B",
                @"A\B\.vs",
                @"A\B\.vs\C",
            }, subDirs);

            Assert.IsTrue(File.Exists(@"A\B\.vs\textfile.txt"));

            Assert.IsFalse(File.Exists(@"A\B\textfile.txt"));
            Assert.IsFalse(File.Exists(@"A\D\textfile.txt"));

        }

        [TestMethod]
        public void scenario_2()
        {

            if (Directory.Exists("X"))
                Directory.Delete("X", true);

            Directory.CreateDirectory(@"X");
            Directory.CreateDirectory(@"X\Y"); // ska tas bort
            Directory.CreateDirectory(@"X\Z"); // ska tas bort
            Directory.CreateDirectory(@"X\Z\W"); // ska tas bort
            Directory.CreateDirectory(@"X\Z\W\Y"); // ska tas bort

            var nrDeleted = new BadCopyService().DeleteFolder("X");

            Assert.AreEqual(4, nrDeleted);

            string[] subDirs = GetAllSubdirectories("X");

            CollectionAssert.AreEquivalent(new string[]
            {
            }, subDirs);

        }

        private string[] GetAllSubdirectories(string folder)
        {
            var current = Directory.GetCurrentDirectory();
            return Directory
                .EnumerateDirectories(folder, "*.*", SearchOption.AllDirectories)
                .Select(x=>x.Replace(current, "")) // todo: teoretiskt risk för bugg
                .ToArray();
        }
    }
}
