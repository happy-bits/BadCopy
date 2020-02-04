using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BadCopy.Core
{


    public class BadCopyService
    {
        public class BadCopyServiceException : Exception
        {
            public BadCopyServiceException(string message) : base(message)
            {

            }
        }

        public List<FileInfo> GetFilesToCopy(Batch batch)
        {
            var result = new List<FileInfo>();
            var fromfolders = batch.FromFolders ?? new List<string> { "" };
            foreach (var folder in fromfolders)
            {
                var path = batch.FromFolderBase == null ? folder : Path.Combine(batch.FromFolderBase, folder);
                string[] files = GetAllFilesInFolderAndSubfolders(path, batch.SpecificFiles, batch.SpecificFileEndings, batch.SkipFolders);

                int index = 0;
                foreach (var fullfilename in files)
                {
                    var toFile = RemoveFirstPartOfString(fullfilename, batch.FromFolderBase).TrimStart('\\');

                    if (batch.RenameFilesTo != null)
                        toFile = batch.RenameFilesTo[index];

                    result.Add(new FileInfo
                    {
                        BatchName = batch.Name,
                        FromFile = Path.Combine(fullfilename),
                        ToFile = Path.Combine(batch.ToFolder, toFile),
                        Action = batch.Action,
                        Binary = fullfilename.ToUpper().EndsWith(".PNG"),
                        Transformations = batch.Transformations,
                    });
                    index++;
                }
            }
            return result;
        }

        private string RemoveFirstPartOfString(string original, string stringToRemove)
        {
            if (!original.StartsWith(stringToRemove))
                return original;

            return original.Substring(stringToRemove.Length);
        }

        public enum CopyResultFileState
        {
            Incomplete,
            FailedRead, FailedWrite, UnknownCopyStyle,
            SuccessNoSolution,
            SuccessClone
        }

        public class CopyResultFile
        {
            public FileInfo FileInfo { get; set; }
            public CopyResultFileState State { get; set; }
        }

        public class CopyResult
        {
            public List<CopyResultFile> CopyResultFiles { get; set; } = new List<CopyResultFile>();
            public bool AllSucceded => CopyResultFiles.All(x =>
                x.State == CopyResultFileState.SuccessNoSolution ||
                x.State == CopyResultFileState.SuccessClone);
        }

        // todo: skriv om så den blir enklare och enklare att testa (separera själva borttagandet från att samla info om vad som ska tas bort)

        public int DeleteFolder(string folderToDelete)
        {
            if (!Directory.Exists(folderToDelete))
                return 0;

            int totaldeletedfolders = 0;

            while (true)
            {
                DeleteFilesInFolder(folderToDelete);

                string[] allFolders = GetAllSubFolders(folderToDelete);

                int deletedFolders = 0;

                foreach (var folder in allFolders)
                {
                    if (!Directory.Exists(folder))
                        continue;

                    // todo: gör detta till en inställning

                    if (IsFolderOrSubFolder(folder, ".vs") || IsFolderOrSubFolder(folder, ".git"))
                        continue;

                    DeleteFilesInFolder(folder);

                    if (Directory.EnumerateDirectories(folder, "*.*", SearchOption.AllDirectories).Count() == 0)
                    {
                        Directory.Delete(folder, true);
                        deletedFolders++;
                        totaldeletedfolders++;
                    }
                }
                if (deletedFolders == 0)
                    break;
            }

            return totaldeletedfolders;

        }

        private static bool IsFolderOrSubFolder(string folder, string subfolder)
        {
            return folder.Contains($"\\{subfolder}\\") || folder.EndsWith($"\\{subfolder}");
        }

        private static string[] GetAllSubFolders(string folderToDelete)
        {
            if (!Directory.Exists(folderToDelete))
                return new string[] { };

            return Directory.EnumerateDirectories(folderToDelete, "*.*", SearchOption.AllDirectories).ToArray();
        }

        public int DeleteFilesInFolder(string folder)
        {
            if (!Directory.Exists(folder))
                return 0;

            var allFiles = Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly).ToArray();
            var deletedFiles = 0;
            foreach (var file in allFiles)
            {
                // todo: gör detta konfigureringsbart

                if (GetFilename(file) == ".gitignore")
                    continue;
                File.Delete(file);
                deletedFiles++;
            }
            return deletedFiles;
        }

        private string GetFilename(string filenameWithPath)
        {
            if (filenameWithPath.IndexOf('\\') == -1)
                return filenameWithPath;

            return filenameWithPath.Split('\\').Last();

            throw new NotImplementedException();
        }

        // todo: refactor

        public CopyResult Copy(List<FileInfo> files)
        {
            var result = new CopyResult();

            foreach (var file in files)
            {
                var crf = new CopyResultFile
                {
                    FileInfo = file, // FileInfo.Clone(file),
                    State = CopyResultFileState.Incomplete
                };
                result.CopyResultFiles.Add(crf);
            }

            foreach (var file in result.CopyResultFiles)
            {
                CopyResultFileState? successState = null;
                string newcontent = null;

                if (file.FileInfo.Binary)
                {
                    successState = CopyResultFileState.SuccessClone;
                }
                else
                {
                    string content = null;

                    try
                    {
                        content = File.ReadAllText(file.FileInfo.FromFile);
                    }
                    catch
                    {
                        file.State = CopyResultFileState.FailedRead;
                        continue;
                    }


                    switch (file.FileInfo.Action)
                    {
                        case Action.Transform:

                            foreach(var trans in file.FileInfo.Transformations)
                            {
                                newcontent = trans.Transform(content);
                            }
                            successState = CopyResultFileState.SuccessNoSolution;
                            break;
                        case Action.Copy:
                            newcontent = content;
                            successState = CopyResultFileState.SuccessClone;
                            break;
                        default:
                            file.State = CopyResultFileState.UnknownCopyStyle;
                            continue;
                    }
                }
                try
                {
                    var directory = GetDirectory(file.FileInfo.ToFile);
                    CreateDirectoryIfNotExist(directory);

                    if (file.FileInfo.Binary)
                    {
                        File.Copy(file.FileInfo.FromFile, file.FileInfo.ToFile, true);
                    }
                    else
                    {
                        File.WriteAllText(file.FileInfo.ToFile, newcontent);
                    }

                }
                catch
                {
                    file.State = CopyResultFileState.FailedWrite;
                    continue;
                }

                file.State = (CopyResultFileState)successState;
            }
            return result;
        }

        private void CreateDirectoryIfNotExist(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        // todo: gör mer generisk så det inte behöver vara exakt "#region solution" (kunna skicka in ett regex)


        private string GetDirectory(string fullfilename)
        {
            // todo: kan finnas bättre sätt
            return fullfilename.Substring(0, fullfilename.LastIndexOf('\\'));
        }

        private string[] GetAllFilesInFolderAndSubfolders(string folder, List<string> specificFiles, List<string> specificLineEndings, List<string> skipFolders)
        {
            if (!Directory.Exists(folder))
                throw new BadCopyServiceException("Folder " + folder + " don't exist");

            var allFiles = Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories).ToArray();

            // todo: verifiera att det verkligen är filer

            if (skipFolders != null)
                allFiles = allFiles.Where(file => !ContainsAny("\\" + file, skipFolders)).ToArray();

            if (specificLineEndings != null)
                allFiles = allFiles.Where(file => EndsWithAny(file, specificLineEndings)).ToArray();

            if (specificFiles != null)
                allFiles = allFiles.Where(file => EndsWithAny("\\" + file, specificFiles)).ToArray();

            return allFiles;

        }

        // todo: lyft in som övning i MethodsAndLists

        private bool ContainsAny(string comparestring, List<string> candidates)
        {
            return candidates.Any(candidate => comparestring.Contains(candidate));

        }

        // todo: lyft in som övning i MethodsAndLists

        private bool EndsWithAny(string comparestring, List<string> candidates)
        {
            return candidates.Any(candidate => comparestring.EndsWith(candidate));
        }
    }
}
