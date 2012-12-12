using Smart.UI.Panels;

namespace Smart.UI.Panels
{
    public static class LineDefinitionsExtensions
    {
        /// <summary>
        /// Не учитывает режим роста панели, поэтому его я отдельным ифом потом отрабатываю
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="num"></param>
        /// <param name="shift"></param>
        /// <param name="grow"></param>
        /// <returns></returns>
        public static LineDefinitions GrowLine(this LineDefinitions source, int num, double shift, LineGrowthMode grow)
        {
            switch (grow)
            {
                case LineGrowthMode.WithPanel:
                    source.GrowLineWithPanel(num, shift);
                    break;
                case LineGrowthMode.WithNeighbours:
                    source.GrowLineCutNeighbours(num, shift);
                    break;
                case LineGrowthMode.WithRightNeighbour:
                    source.GrowLineCutRightNeighbour(num, shift);
                    break;
                case LineGrowthMode.WithLeftNeighbour:
                    source.GrowLineCutLeftNeighbour(num, shift);
                    break;
                    /*  case LineGrowthMode.WithAllOtherLines:
                    source.GrowLineCutOthers(num, shift);
                    break;*/
                case LineGrowthMode.ChosenLinesOnly:
                    source.GrowLineOnly(num, shift);
                    break;
            }
            return source;
        }
    }
}