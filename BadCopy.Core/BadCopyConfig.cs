using System.Collections.Generic;

namespace BadCopy.Core
{
    public class BadCopyConfig
    {
        public List<Batch> Batches { get; set; }
        public string ReplaceSolutionWith { get; set; }
    }
}
