using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadCopy.Core
{
    class BadCopyServiceException: Exception
    {
        public BadCopyServiceException(string message):base(message)
        {

        }
    }

    public class BadCopyService
    {
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
                        BatchName=batch.Name,
                        CopyStyle=batch.CopyStyle,
                        FromFile=Path.Combine(fullfilename),
                        ToFile = Path.Combine(batch.ToFolder, folder, filename),
                    });
                }
            }
            return result;
        }

        private string GetFilenameFromFullFilename(string fullfilename)
        {
            return fullfilename.Split('\\').Last();
        }

        private string[] GetAllFilesInFolderAndSubfolders(string folder, string searchpattern)
        {
            if (!Directory.Exists(folder))
                throw new BadCopyServiceException("Folder "+folder+" don't exist");

            return Directory.GetFiles(folder, searchpattern);

            // todo: hantera underbibliotek
        }
    }
}
