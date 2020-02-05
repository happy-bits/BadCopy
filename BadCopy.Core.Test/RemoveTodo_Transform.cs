using BadCopy.Core.Transformations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core.Test
{
    [TestClass]
    public class RemoveTodo_Transform
    {
        [TestMethod]
        public void one_todos()
        {
            var input = "// todo: aaa\nbbb";
            var actual = new RemoveTodo().Transform(input);
            var expected = "bbb";

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void one_todos_breif()
        {
            var input = "//todo:aaa\nbbb";
            var actual = new RemoveTodo().Transform(input);
            var expected = "bbb";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void multiple_todos()
        {
            var input = "// todo: aaa\nbbb\n// todo: ccc\nddd";
            var actual = new RemoveTodo().Transform(input);
            var expected = "bbb\nddd";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void only_todos()
        {
            var input = "// todo: aaa\n// todo: bbb\n   \t  //  \t  todo: \t ccc\n// todo: ddd\n";
            var actual = new RemoveTodo().Transform(input);
            var expected = "";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void one_todos_with_spaces()
        {
            var input = "   \t  //    todo:    \t aaa\nbbb";
            var actual = new RemoveTodo().Transform(input);
            var expected = "bbb";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void todos_misspelled()
        {
            var input = "// todo aaa\nbbb";
            var actual = new RemoveTodo().Transform(input);
            var expected = input;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void todos_misspelled_2()
        {
            var input = "todo: aaa\nbbb";
            var actual = new RemoveTodo().Transform(input);
            var expected = input;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void no_todos()
        {
            var input = "aaa\nbbb";
            var actual = new RemoveTodo().Transform(input);
            var expected = "aaa\nbbb";

            Assert.AreEqual(expected, actual);
        }
    }
}
