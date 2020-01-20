namespace BadCopy.Core
{
    public enum CopyStyle
    {
        Unknown,          // kopieringstilen är inte angiven (troligen pga ett fel)
        NoSolution,       // kopia men ha inte med #region solution .... #endregion
        Clone,            // exakt kopia av filen 
    }
}
