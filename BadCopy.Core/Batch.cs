using BadCopy.Core.Transformations;
using System.Collections.Generic;
using System.Linq;

namespace BadCopy.Core
{
    public class Batch
    {
        private string _fromFolderBase;
        private string _toFolder;
        private string _folderToDelete;

        public string Name { get; set; }
        public Action Action { get; set; }

        public string FromFolderBase { get => ReplaceVariablesWithValues(_fromFolderBase); set => _fromFolderBase = value; }
        public string ToFolder { get => ReplaceVariablesWithValues(_toFolder); set => _toFolder = value; }

        public List<string> FromFolders { get; set; }
        public List<string> SpecificFiles { get; set; }
        public List<string> RenameFilesTo { get; set; }
        public List<string> SpecificFileEndings { get; set; }
        public List<string> SkipFolders { get; set; }
        public List<Variable> Variables { get; set; } 
        public string FolderToDelete { get => ReplaceVariablesWithValues(_folderToDelete); set => _folderToDelete = value; }

        public string[] TransformationNames { get; set; } // används bara vid inläsning av jsonfilen

        public Transformation[] Transformations { get {

                var result = new List<Transformation>();
                foreach(string name in TransformationNames)
                {
                    
                    switch (name)
                    {
                        case "RemoveSolutionRegion": result.Add(new RemoveSolutionRegion());break;
                        case "ReplaceSolutionRegionWithTodo": result.Add(new ReplaceSolutionRegionWithTodo());break;
                        case "Workout": result.Add(new Workout());break;
                        default: throw new System.Exception("Unknown transformation " + name);
                    }
                }
                return result.ToArray();
            }
        }

        private string ReplaceVariablesWithValues(string valueWithVariables)
        {
            if (Variables == null)
                return valueWithVariables;

            if (valueWithVariables == null )
                return null;

            foreach (var v in Variables)
                valueWithVariables = valueWithVariables.Replace(v.Name, v.Value);

            return valueWithVariables;
        }
    }
}
