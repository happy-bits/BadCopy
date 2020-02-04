using BadCopy.Core.Transformations;
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
    public class WorkoutTransformation_Transform
    {
        [TestMethod]
        public void sample_01()
        {
            var wo = new WorkoutTransformation();

            string input = File.ReadAllText("WorkoutSamples\\01.txt");
            string expected = File.ReadAllText("WorkoutSamples\\01-expected.txt");

            string result = wo.Transform(input);

            Assert.AreEqual(expected, result);
        }    
    }
}
