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
    public class RemoveHint_Transform
    {
        [TestMethod]
        public void multiple_hints()
        {
            var actual = new RemoveHint().Transform(new[] {
                "aa",
                "## Hint",
                "bb",
                "bb",
                "bb",
                "bb",
                "## Bla bla",
                "cc",
                "cc",
                "cc",
                "## Hint",
                "dd",
                "dd",
                "dd",
                "dd",
                "## Bla bla",
                "ee",
                "ee",
            });

            var expected = new[] {
                "aa",
                "## Bla bla",
                "cc",
                "cc",
                "cc",
                "## Bla bla",
                "ee",
                "ee",
            };

            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void one_hint()
        {
            var actual = new RemoveHint().Transform(new[] {
                "aa",
                "aa",
                "aa",
                "## Hint",
                "bb",
                "bb",
                "bb",
                "bb",
                "## Bla bla",
                "cc",
                "cc"
            });

            var expected = new[] {
                "aa",
                "aa",
                "aa",
                "## Bla bla",
                "cc",
                "cc"
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void hint_at_end()
        {
            var actual = new RemoveHint().Transform(new[] {
                "aa",
                "aa",
                "## Hint",
                "bb",
                "bb",
            });

            var expected = new[] {
                "aa",
                "aa",
            };

            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void no_hits()
        {
            var actual = new RemoveHint().Transform(new[] {
                "## Bla bla",
                "aa",
                "## Bla bla",
                "bb",
                "bb",
                "## Bla bla",

            });

            var expected = new[] {
                "## Bla bla",
                "aa",
                "## Bla bla",
                "bb",
                "bb",
                "## Bla bla",
            };

            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void header_at_end()
        {
            var actual = new RemoveHint().Transform(new[] {
                "aa",
                "aa",
                "## Hint",
                "bb",
                "bb",
                "## Bla bla",

            });

            var expected = new[] {
                "aa",
                "aa",
                "## Bla bla",
            };

            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void hint_without_content()
        {
            var actual = new RemoveHint().Transform(new[] {
                "aa",
                "bb",
                "## Hint",
                "## Hint",
                "## Hint",
                "cc",
                "dd",
                "ee",

            });

            var expected = new[] {
                "aa",
                "bb",
            };

            CollectionAssert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void multiple_hitns()
        {
            var actual = new RemoveHint().Transform(new[] {
                "1",                
                "## Hint",
                "2",
                "## Hint",
                "3",
                "## AAAAAA",
                "4",
                "## Hint",
                "5",
                "## BBBBBBBB",
                "6",

            });

            var expected = new[] {
                "1",
                "## AAAAAA",
                "4",
                "## BBBBBBBB",
                "6",
            };

            CollectionAssert.AreEqual(expected, actual);

        }




    }
}
