namespace Smart.UI.Panels
{
    /// <summary>
    /// Режим вырастания грида
    /// </summary>
    public enum LineGrowthMode
    {
        Default,
        WithLeftNeighbour,
        WithRightNeighbour,
        WithNeighbours,
        WithPanel,
        WithAllOtherLines,
        ChosenLinesOnly,
        None
    }
    /*
    public enum ConstrainsMode
    {
        Default,
        ChangePanel
    }
    */

    public class LineMover
    {
        public LineGrowthMode Growth = LineGrowthMode.WithNeighbours;
        public int Source = 0;
        public int Target = 1;
        //  public LineMoverRole Role = LineMoverRole.Mover;

        public LineMover(int source = -1, int target = 1, LineGrowthMode growthMode = LineGrowthMode.WithRightNeighbour)
        {
            //   Role = role;
            Source = source;
            Target = target;
            Growth = growthMode;
        }
    }
}