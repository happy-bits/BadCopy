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
        public string ReplaceSolutionWith { get; set; } = "";

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
                string[] files = GetAllFilesInFolderAndSubfolders(Path.Combine(batch.FromFolderBase, folder), batch.SpecificFiles, batch.SpecificFileEndings, batch.SkipFolders);
                foreach (var fullfilename in files)
                {
                    var fullfilenameWithoutFromFolder = RemoveFirstPartOfString(fullfilename, batch.FromFolderBase).TrimStart('\\');
                    result.Add(new FileInfo
                    {
                        BatchName = batch.Name,
                        CopyStyle = (CopyStyle)batch.CopyStyle,
                        FromFile = Path.Combine(fullfilename),
                        ToFile = Path.Combine(batch.ToFolder, fullfilenameWithoutFromFolder),
                    });
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

        public int DeleteFolder(string folderToDelete)
        {
            var allFolders = Directory.EnumerateDirectories(folderToDelete, "*.*", SearchOption.AllDirectories).ToArray();
            var deletedFolders = 0;
            foreach (var folder in allFolders)
            {
                if (folder.Contains(@"\.vs\") || folder.EndsWith(@"\.vs") || folder.Contains(@"\.git\") || folder.EndsWith(@"\.git"))
                    continue;
                Directory.Delete(folder, true); 
                deletedFolders++;
            }
            return deletedFolders;
        }

        public int DeleteFiles(string folderToDelete)
        {
            var allFiles = Directory.EnumerateFiles(folderToDelete, "*.*", SearchOption.AllDirectories).ToArray();
            var deletedFiles = 0;
            foreach (var file in allFiles)
            {
                if (file.Contains(@"\.vs\") || file.Contains(@"\.git\"))
                    continue;
                File.Delete(file);
                deletedFiles++;
            }
            return deletedFiles;
        }

        public CopyResult Copy(List<FileInfo> files)
        {
            var result = new CopyResult();

            foreach (var file in files)
            {
                var crf = new CopyResultFile
                {
                    FileInfo = FileInfo.Clone(file),
                    State = CopyResultFileState.Incomplete
                };
                result.CopyResultFiles.Add(crf);
            }

            foreach (var file in result.CopyResultFiles)
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
                string newcontent = null;

                CopyResultFileState? successState = null;

                switch (file.FileInfo.CopyStyle)
                {
                    case CopyStyle.NoSolution:
                        newcontent = RemoveSolutionRegion(content);
                        successState = CopyResultFileState.SuccessNoSolution;
                        break;
                    case CopyStyle.Clone:
                        newcontent = content;
                        successState = CopyResultFileState.SuccessClone;
                        break;
                    case CopyStyle.NoSolutionNoHash:
                        throw new NotImplementedException();
                    default:
                        file.State = CopyResultFileState.UnknownCopyStyle;
                        continue;
                }

                try
                {
                    var directory = GetDirectory(file.FileInfo.ToFile);
                    CreateDirectoryIfNotExist(directory);
                    File.WriteAllText(file.FileInfo.ToFile, newcontent);
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

        public string RemoveSolutionRegion(string content)
        {
            content = content.Replace("\r\n", "\n"); // todo: rätt att göra det här?

            // todo: snyggare sätt där detta inte behövs?

            if (content.Trim().EndsWith("#endregion"))
                content += "\n";

            return Regex.Replace(content, @"[ \t]*#region solution\s*\n[\s\S]*?\n\s*#endregion[ \t]*\n", ReplaceSolutionWith, RegexOptions.Multiline);
        }

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
