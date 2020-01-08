using BadCopy.Core;
using ConsoleUtilities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace BadCopy.UI
{
    class Program
    {
        static ConsoleCompanion cc = new ConsoleCompanion();

        static void Main()
        {
            try
            {
                BadCopyConfig config = ReadBadCopyConfigurationFile();

                var bcs = new BadCopyService();
                bcs.ReplaceSolutionWith = config.ReplaceSolutionWith;

                foreach (var batch in config.Batches)
                {
                    var files = bcs.GetFilesToCopy(batch);
                    var result = bcs.Copy(files);
                    
                    if (result.AllSucceded)
                    {
                        int noSolution = result.CopyResultFiles.Count(x => x.State == BadCopyService.CopyResultFileState.SuccessNoSolution);
                        int clonedFiles = result.CopyResultFiles.Count(x => x.State == BadCopyService.CopyResultFileState.SuccessClone);

                        cc.WriteLineGreen($"Batch {batch.Name} succeeded. {noSolution} files without solution. {clonedFiles} cloned files.");
                    } else
                    {
                        cc.WriteLineRed($"Batch {batch.Name} failed. ");

                        // todo: refactor, förkorta

                        var failedReads = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.FailedRead).ToArray();
                        var failedWrites = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.FailedWrite).ToArray();
                        var unknownCopyStyle = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.UnknownCopyStyle).ToArray();
                        var incompleted = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.Incomplete).ToArray();

                        if (failedReads.Any())
                        {
                            cc.Space();
                            cc.WriteLineRed($"\tFailed to read:");
                            foreach (var file in failedReads)
                                cc.WriteLineRed($"\t\t{file.FileInfo.FromFile}");
                        }

                        if (failedWrites.Any())
                        {
                            cc.Space();
                            cc.WriteLineRed($"\tFailed to write:");
                            foreach (var file in failedWrites)
                                cc.WriteLineRed($"\t\t{file.FileInfo.ToFile}");
                        }


                        if (unknownCopyStyle.Any())
                        {
                            cc.Space();
                            cc.WriteLineRed($"\tUnknown copy style:");
                            foreach (var file in unknownCopyStyle)
                                cc.WriteLineRed($"\t\t{file.FileInfo.FromFile}");
                        }

                        if (incompleted.Any())
                        {
                            cc.Space();
                            cc.WriteLineRed($"\nIncompleted:");
                            foreach (var file in incompleted)
                                cc.WriteLineRed($"\t\t{file.FileInfo.FromFile}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                cc.WriteLineRed(ex.Message);
                return;
            }

            // todo: fixa i ConsoleCompanion så att förra färgen automatiskt kommer tillbaka efter t.ex WriteLineRed (då behövs inte denna raden)
            cc.WriteLine("");

        }

        private static BadCopyConfig ReadBadCopyConfigurationFile()
        {
            string filecontent;

            try
            {
                filecontent = File.ReadAllText("badcopy.json");
            }
            catch
            {
                throw new Exception("Could not find the file 'badcopy.json'. Check that it exist in the current folder.");
            }

            try
            {
                return JsonConvert.DeserializeObject<BadCopyConfig>(filecontent);
            }
            catch
            {
                throw new Exception("Found the file 'badcopy.json' but it was in the wrong format. Check the format of the file.");
            }

        }
    }
}

//var config = new BadCopyConfig();
//config.Batches = new List<Batch> { b1 };
//            string s = JsonConvert.SerializeObject(config);
//File.WriteAllText("badcopy.json", s);


////string s = JsonConvert.SerializeObject(new Batch
////{
////    CopyStyle = CopyStyle.NoSolution,
////    FromFolders = new List<string> { "fromfolder" },
////    Name = "name",
////    ToFolder = "tofolder"
////});
