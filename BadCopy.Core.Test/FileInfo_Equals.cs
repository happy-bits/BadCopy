using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadCopy.Core.Transformations;
namespace BadCopy.Core.Test
{
    [TestClass]
    public class FileInfo_Equals
    {
        [TestMethod]
        public void empty()
        {
            var f1 = new FileInfo();
            var f2 = new FileInfo();

            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod]
        public void different_batch_name()
        {
            var f1 = new FileInfo { BatchName = "xxx" };
            var f2 = new FileInfo { BatchName = "xxxxxxxx" };

            Assert.IsFalse(f1.Equals(f2));
        }

        [TestMethod]
        public void same_batch_name()
        {
            var f1 = new FileInfo { BatchName = "xxx" };
            var f2 = new FileInfo { BatchName = "xxx" };

            Assert.IsTrue(f1.Equals(f2));
        }


        [TestMethod]
        public void same_transformations()
        {
            var f1 = new FileInfo { Transforms = new Transform[] { new Transform(new AddComments()) } };
            var f2 = new FileInfo { Transforms = new Transform[] { new Transform(new AddComments()) } };

            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod]
        public void different_transformations()
        {
            var f1 = new FileInfo
            {
                Transforms = new Transform[] {
                    new Transform (new Workout()),
                    new Transform (new ReplaceSolutionRegionWithTodo()),
                }
            };
            var f2 = new FileInfo
            {
                Transforms = new Transform[] {
                    new Transform (new Workout()),
                    new Transform (new RemoveSolutionRegion()),
                }
            };

            Assert.IsFalse(f1.Equals(f2));
        }

        [TestMethod]
        public void order_of_transformation_should_matter()
        {
            var f1 = new FileInfo
            {
                Transforms = new Transform[] {
                    new Transform (new RemoveSolutionRegion()),
                    new Transform (new Workout()),
                }
            };
            var f2 = new FileInfo
            {
                Transforms = new Transform[] {
                    new Transform (new Workout()),
                    new Transform (new RemoveSolutionRegion()),
                }
            };

            Assert.IsFalse(f1.Equals(f2));
        }


    }
}
