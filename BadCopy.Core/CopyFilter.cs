namespace BadCopy.Core
{
    public enum CopyFilter
    {
        NoFilter,               // kopiera filen
        OnlyCopyIfNotInHistory  // kopiera bara om den inte redan är kopiera (och finns i historik-log) (ej implementerat)
    }
}
