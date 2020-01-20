using System.Collections.Generic;

namespace BadCopy.Core
{
    public class BadCopyConfigFile
    {
        public List<Batch> Batches { get; set; }
        public string ReplaceSolutionWith { get; set; }

        // Här är samma properties som i "Batch", fast nullable

        public string FromFolderBase { get; set; }
        public string ToFolder { get; set; }
        
        public List<string> FromFolders { get; set; }
        public List<string> SpecificFiles { get; set; }
        public List<string> SpecificFileEndings { get; set; }
        public List<string> SkipFolders { get; set; }
        public List<Variable> Variables { get; set; }
        public Action Action { get; set; }

        public BadCopyConfig MergeConfiguration()
        {
            var result = new BadCopyConfig
            {
                Batches = Batches,
                ReplaceSolutionWith = ReplaceSolutionWith,
                ToFolder = ToFolder
            };

            // todo: refactor, går det att göra mer generiskt?

            foreach (var b in Batches)
            {
                if (b.Variables == null) b.Variables = Variables; // måste vara först....
                if (b.FromFolderBase == null) b.FromFolderBase = FromFolderBase;
                if (b.FromFolders == null) b.FromFolders = FromFolders;
                if (b.ToFolder == null) b.ToFolder = ToFolder;
                if (b.FromFolders == null) b.FromFolders = FromFolders;
                if (b.SpecificFiles == null) b.SpecificFiles = SpecificFiles;
                if (b.SpecificFileEndings == null) b.SpecificFileEndings = SpecificFileEndings;
                if (b.SkipFolders == null) b.SkipFolders = SkipFolders;
                if (b.Action == Action.Unknown) b.Action = Action;
            }
            return result;
        }
    }
}
