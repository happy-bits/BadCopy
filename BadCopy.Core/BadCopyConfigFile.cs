﻿using System.Collections.Generic;

namespace BadCopy.Core
{
    public class BadCopyConfigFile
    {
        public List<Batch> Batches { get; set; }

        // Här är samma properties som i "Batch", fast nullable

        public string FromFolderBase { get; set; }
        public string ToFolder { get; set; }
        public string[] TransformationNames { get; set; }
        
        public List<string> FromFolders { get; set; }
        public List<string> SpecificFiles { get; set; }
        public List<string> SpecificFileEndings { get; set; }
        public List<string> SkipFolders { get; set; }
        public List<Variable> Variables { get; set; }
        public Action Action { get; set; }
        public Transform[] Transforms { get; set; }

        public BadCopyConfig MergeConfiguration()
        {
            var result = new BadCopyConfig
            {
                Batches = Batches,
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
                if (b.Transforms == null) b.Transforms = Transforms;
            }
            return result;
        }
    }
}
