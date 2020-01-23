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
        static readonly ConsoleCompanion cc = new ConsoleCompanion();

        static void Main(params string[] args)
        {
            // todo: refactor denna supermetod
            //try
            //{
            // todo: inställning: börja med att ta bort allt i målet
            // todo: validering av badconfig.json (ex att FromFolders finns)
            // todo: koppla ihop FromFolder och ToFolder så det blir en variabel. Ett par.
            // todo: kunna byta namn på en fil (iallafall när man kopierar filen rätt av)

            SetWindowSize();

            var bcs = new BadCopyService();

            CommandArguments commandargs = new CommandArguments(args);

            BadCopyConfigFile configFile = ReadBadCopyConfigurationFile(commandargs.ConfigFileName);

            BadCopyConfig config = configFile.MergeConfiguration();

            cc.Space();

            bcs.ReplaceSolutionWith = config.ReplaceSolutionWith;

            var batchesToRun = commandargs.OnlyLastBatches == null ? config.Batches : config.Batches.TakeLast((int)commandargs.OnlyLastBatches);
            foreach (var batch in batchesToRun)
            {
                switch (batch.Action)
                {
                    case Core.Action.DeleteFolder:

                        var countDeletedFolders = bcs.DeleteFolder(batch.FolderToDelete);
                        cc.WriteLineGreen($"Deleted {countDeletedFolders} folders in destination folder.");
                        cc.Space();

                        break;

                    case Core.Action.Copy:
                    case Core.Action.CopyWithoutSolution:

                        var files = bcs.GetFilesToCopy(batch);
                        var result = bcs.Copy(files);

                        if (result.AllSucceded)
                        {
                            var noSolution = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.SuccessNoSolution);
                            var clonedFiles = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.SuccessClone);

                            if (noSolution.Count() + clonedFiles.Count() == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Batch '{batch.Name}' finished but nothing copied.");
                                Console.ForegroundColor = ConsoleColor.White;
                                continue;
                            }

                            cc.WriteLineGreen($"Batch '{batch.Name}' succeeded.");

                            if (noSolution.Any())
                            {
                                cc.WriteLineGreen($"\n\tWithout solution:\n");
                                foreach (var file in noSolution)
                                    cc.WriteLineGreen($"\t\t{file.FileInfo.ToFile}");
                            }
                            if (clonedFiles.Any())
                            {
                                cc.WriteLineGreen($"\n\tCloned:\n");
                                foreach (var file in clonedFiles)
                                    cc.WriteLineGreen($"\t\t{file.FileInfo.ToFile}");
                            }
                            cc.Space();
                        }
                        else
                        {
                            cc.WriteLineRed($"Batch '{batch.Name}' failed. ");

                            // todo: refactor, förkorta

                            var failedReads = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.FailedRead).ToArray();
                            var failedWrites = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.FailedWrite).ToArray();
                            var unknownCopyStyle = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.UnknownCopyStyle).ToArray();
                            var incompleted = result.CopyResultFiles.Where(x => x.State == BadCopyService.CopyResultFileState.Incomplete).ToArray();

                            if (failedReads.Any())
                            {
                                cc.WriteLineRed($"\n\tFailed to read:\n");
                                foreach (var file in failedReads)
                                    cc.WriteLineRed($"\t\t{file.FileInfo.FromFile}");
                            }

                            if (failedWrites.Any())
                            {
                                cc.WriteLineRed($"\n\tFailed to write:\n");
                                foreach (var file in failedWrites)
                                    cc.WriteLineRed($"\t\t{file.FileInfo.ToFile}");
                            }


                            if (unknownCopyStyle.Any())
                            {
                                cc.WriteLineRed($"\n\tUnknown copy style:\n");
                                foreach (var file in unknownCopyStyle)
                                    cc.WriteLineRed($"\t\t{file.FileInfo.FromFile}");
                            }

                            if (incompleted.Any())
                            {
                                cc.WriteLineRed($"\n\tIncompleted:\n");
                                foreach (var file in incompleted)
                                    cc.WriteLineRed($"\t\t{file.FileInfo.FromFile}");
                            }
                            cc.Space();
                        }
                        break;
                    default:
                        throw new Exception("Unknown action");
                }


            }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    cc.WriteLineRed(ex.Message);
            //    return;
            //}

            // todo: fixa i ConsoleCompanion så att förra färgen automatiskt kommer tillbaka efter t.ex WriteLineRed (då behövs inte denna raden)
            cc.WriteLine("");
        }

        private static void SetWindowSize()
        {
            Console.WindowWidth = 120;
            Console.WindowHeight = 20;
            Console.BufferWidth = 120;
            Console.BufferHeight = 300;

        }

        static private BadCopyConfigFile ReadBadCopyConfigurationFile(string configFileName)
        {
            string filecontent;

            try
            {
                filecontent = File.ReadAllText(configFileName);
            }
            catch
            {
                throw new Exception($"Could not find the file '{configFileName}'. Check that it exist in the current folder.");
            }

            try
            {
                return JsonConvert.DeserializeObject<BadCopyConfigFile>(filecontent);
            }
            catch
            {
                throw new Exception($"Found the file '{configFileName}' but it was in the wrong format. Check the format of the file.");
            }

        }
    }
}
