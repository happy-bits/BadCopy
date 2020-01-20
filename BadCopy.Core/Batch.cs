using System.Collections.Generic;

namespace BadCopy.Core
{
    public class Batch
    {
        private string fromFolderBase;
        private string toFolder;

        public string Name { get; set; }
        public string FromFolderBase { get => ReplaceVariablesWithValues(fromFolderBase); set => fromFolderBase = value; }
        public string ToFolder { get => ReplaceVariablesWithValues(toFolder); set => toFolder = value; }

        public CopyStyle? CopyStyle { get; set; }
        public List<string> FromFolders { get; set; }
        public List<string> SpecificFiles { get; set; }
        public List<string> SpecificFileEndings { get; set; }
        public List<string> SkipFolders { get; set; }
        public List<Variable> Variables { get; set; }

        private string ReplaceVariablesWithValues(string valueWithVariables)
        {
            if (valueWithVariables == null)
                return null;

            foreach (var v in Variables)
                valueWithVariables = valueWithVariables.Replace(v.Name, v.Value);

            return valueWithVariables;
        }
    }
}
