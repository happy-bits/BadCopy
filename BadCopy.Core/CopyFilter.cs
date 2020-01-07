namespace BadCopy.Core
{
    public enum CopyFilter
    {
        CopyFile,               // kopiera filen
        OnlyCopyIfNotInHistory  // kopiera bara om den inte redan är kopiera (och finns i historik-log)
    }
}
