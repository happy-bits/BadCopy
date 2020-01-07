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
            foreach (var folder in batch.FromFolders)
            {
                string[] files = GetAllFilesInFolderAndSubfolders(Path.Combine(batch.FromFolderBase, folder), batch.SearchPattern);
                foreach (var fullfilename in files)
                {
                    string filename = GetFilenameFromFullFilename(fullfilename);
                    result.Add(new FileInfo
                    {
                        BatchName = batch.Name,
                        CopyStyle = batch.CopyStyle,
                        FromFile = Path.Combine(fullfilename),
                        ToFile = Path.Combine(batch.ToFolder, folder, filename),
                    });
                }
            }
            return result;
        }

        public enum CopyResultFileState
        {
            Unknown, Success, FailedRead, FailedWrite, UnknownCopyStyle
        }

        public class CopyResultFile
        {
            public FileInfo FileInfo { get; set; }
            public CopyResultFileState State { get; set; }
        }

        public class CopyResult
        {
            public List<CopyResultFile> CopyResultFiles { get; set; } = new List<CopyResultFile>();
            public bool AllSucceded => CopyResultFiles.All(x=>x.State == CopyResultFileState.Success);
        }

        public CopyResult Copy(List<FileInfo> files)
        {
            var result = new CopyResult();

            foreach (var file in files)
            {
                var crf = new CopyResultFile
                {
                    FileInfo = FileInfo.Clone(file),
                    State = CopyResultFileState.Unknown
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
                switch (file.FileInfo.CopyStyle)
                {
                    case CopyStyle.NoSolution:
                        newcontent = RemoveSolutionRegion(content);
                        break;
                    case CopyStyle.NoSolutionNoHash:
                        throw new NotImplementedException();
                    case CopyStyle.Clone:
                        newcontent = content;
                        break;
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

                file.State = CopyResultFileState.Success;
            }
            return result;
        }

        private void CreateDirectoryIfNotExist(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        public string RemoveSolutionRegion(string content)
        {
            return Regex.Replace(content, @"\s*#region solution\s*\n[\s\S]*?\n\s*#endregion\s*\n", "\n", RegexOptions.Multiline);
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

            return Directory.GetFiles(folder, searchpattern);

            // todo: hantera underbibliotek
        }
    }
}
