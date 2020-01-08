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
            foreach (var folder in batch.FromFolders)
            {
                string[] files = GetAllFilesInFolderAndSubfolders(Path.Combine(batch.FromFolderBase, folder), batch.SearchPattern);
                foreach (var fullfilename in files)
                {
                    var fullfilenameWithoutFromFolder = RemoveFirstPartOfString(fullfilename, batch.FromFolderBase).TrimStart('\\');
                    result.Add(new FileInfo
                    {
                        BatchName = batch.Name,
                        CopyStyle = batch.CopyStyle,
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
            public bool AllSucceded => CopyResultFiles.All(x=>
                x.State == CopyResultFileState.SuccessNoSolution || 
                x.State == CopyResultFileState.SuccessClone);
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

        private string GetFilenameFromFullFilename(string fullfilename)
        {
            // todo: kan finnas bättre sätt

            return fullfilename.Split('\\').Last();
        }

        private string[] GetAllFilesInFolderAndSubfolders(string folder, string searchpattern)
        {
            if (!Directory.Exists(folder))
                throw new BadCopyServiceException("Folder " + folder + " don't exist");

            return Directory.EnumerateFiles(folder, searchpattern, SearchOption.AllDirectories).ToArray();

            //return Directory.GetFiles(folder, searchpattern);

            // todo: hantera underbibliotek
        }
    }
}
