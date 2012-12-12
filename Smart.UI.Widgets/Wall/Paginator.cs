using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Smart.UI.Widgets
{
    public interface IPaginator
    {
        int CellsIn(FrameworkElement item);
        Func<WallCollection<FrameworkElement>, FrameworkElement, Boolean> PaginationGenerator(int itemsInPage);
    }

    public class Paginator : IPaginator
    {
        #region IPaginator Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>how many cells are in one item</returns>
        public int CellsIn(FrameworkElement item)
        {
            return CellsFromContentSize(item.GetContentSize());
        }

        public Func<WallCollection<FrameworkElement>, FrameworkElement, bool> PaginationGenerator(int cellsInPage)
        {
            return
                (line, item) => item == null ? CellsIn(line) > cellsInPage : CellsIn(line) + CellsIn(item) > cellsInPage;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns>how many cells are in one item</returns>
        public int CellsIn(IEnumerable<FrameworkElement> items)
        {
            return items.Sum(i => CellsIn(i));
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>how many cells are in one item</returns>
        public int CellsIn(ContentWallPanelItem item)
        {
            //TO DO add workaround with auto
            return CellsFromContentSize(item.ContentSize);
        }
        */
        /// <summary>
        /// Tranfers ContentSize into cells
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public int CellsFromContentSize(ContentSize size)
        {
            switch (size)
            {
                case ContentSize.Small:
                    return 1;
                case ContentSize.Medium:
                    return 2;
                case ContentSize.Large:
                    return 3;
                default:
                    return 2;
            }
        }
    }
}