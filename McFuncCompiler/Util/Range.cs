namespace McFuncCompiler.Util
{
    /// <summary>
    /// Simple range.
    /// </summary>
    public sealed class Range
    {
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public int Length => Maximum - Minimum;

        public Range(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override string ToString()
        {
            return Minimum + "-" + Maximum;
        }
    }
}
