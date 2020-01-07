

using System.IO;

namespace BadCopy.Core.Test.Scenario01
{
    public class Common
    {
        public static readonly string TestProjectRoot = "C:\\Project\\BadCopy\\BadCopy.Core.Test\\";
        public static readonly string Scenario01Root = TestProjectRoot + "Scenario01\\";

        public static string InputFile(string filenamewithpath)
        {
            return Path.Combine(Scenario01Root, "Input", filenamewithpath);
        }

        public static string OutputFile(string filenamewithpath)
        {
            return Path.Combine(Scenario01Root, "Output", filenamewithpath);
        }

        public static string ExpectedOutputFile(string filenamewithpath)
        {
            return Path.Combine(Scenario01Root, "ExpectedOutput", filenamewithpath);
        }

    }
}
