using BadCopy.Core.Transformations;

namespace BadCopy.Core
{
    public class Transform
    {
        public string Name { 
            set {
                switch (value)
                {
                    case "RemoveTodo": Transformation = new RemoveTodo(); break;
                    case "RemoveHint": Transformation = new RemoveHint(); break;
                    case "AddComments": Transformation = new AddComments(); break;
                    case "RemoveSolutionRegion": Transformation = new RemoveSolutionRegion(); break;
                    case "ReplaceSolutionRegionWithTodo": Transformation = new ReplaceSolutionRegionWithTodo(); break;
                    case "Workout": Transformation = new Workout(); break;
                    throw new System.Exception("Unknown transformation " + value);
                }
            }
        }

        public Transformation Transformation { get; set; }

        public string[] FileExtensions { get; set; }

        public bool AcceptAllFiles => FileExtensions == null;

        public Transform(Transformation t)
        {
            Transformation = t;
        }
    }
}
