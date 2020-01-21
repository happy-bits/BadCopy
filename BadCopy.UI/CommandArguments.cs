using System;
using System.Linq;

namespace BadCopy.UI
{
    public class CommandArguments
    {
        public string ConfigFileName { get; }
        public int? OnlyLastBatches { get; }

        public CommandArguments(string[] args)
        {
            ConfigFileName = TryGetArgument(args, "config-file", "badcopy.json");

            var onlyLastBatches = TryGetArgument(args, "only-last-batches", null);
            OnlyLastBatches = onlyLastBatches == null ? null : (int?)int.Parse(onlyLastBatches);
        }


        static private string TryGetArgument(string[] args, string parameterName, string defaultValue)
        {
            // --config-file=badcopy-root.json
            var hits = args.Where(x => x.Trim().StartsWith("--" + parameterName));

            if (hits.Count() == 0)
                return defaultValue;

            if (hits.Count() >= 2)
                throw new Exception($"Too many parameters with parameter {parameterName}");

            var split = hits.First().Split('=');

            if (split.Count() != 2)
                throw new Exception($"Wrong format for {parameterName}");

            return split[1];

        }
    }
}
