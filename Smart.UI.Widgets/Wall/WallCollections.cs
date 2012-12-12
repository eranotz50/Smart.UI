using System.Windows;
using Smart.UI.Panels;
using Smart.Classes.Collections;

namespace Smart.UI.Widgets
{
    public class WallCollection2D<T> : SmartCollection2D<WallCollection<T>, T> where T : FrameworkElement
    {
        /// <summary>
        /// Strores triplets of linedefinitions
        /// </summary>
        public SmartCollection<Triplet<LineDefinition>> OtherLineTriplets;


        public WallCollection2D()
        {
            OtherLineTriplets = new SmartCollection<Triplet<LineDefinition>>();
        }

        /// <summary>
        /// Returns LineSpan of the element
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cells"></param>
        /// <returns></returns>
        public int GetLineSpan(int index, int cells)
        {
            int count = index + cells;
            if (cells == 1) return 1;
            if (OtherLineTriplets.Count > count)
                return OtherLineTriplets[count].Before.Num - OtherLineTriplets[index].After.Num;
            return (cells - 1)*3 + 1;
        }

        public void UpdateOtherLineDefs(int cellNum)
        {
            while (OtherLineTriplets.Count < cellNum) OtherLineTriplets.Add(new Triplet<LineDefinition>());
        }
    }

    public class WallCollection<T> : SmartCollection<T>
    {
        public Triplet<LineDefinition> Position;
    }
}