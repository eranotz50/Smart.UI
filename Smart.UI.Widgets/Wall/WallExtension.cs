using System.Windows;
using System.Windows.Controls;
using Smart.UI.Panels;
using Smart.UI.Widgets;

namespace Smart.UI.Widgets
{
    public static class WallExtension
    {
        public static LineDefinitions AddTriplet(this LineDefinitions source, Triplet<LineDefinition> triplet)
        {
            source.Add(triplet.Before);
            source.Add(triplet.Content);
            source.Add(triplet.After);
            return source;
        }

        public static LineDefinitions RemoveTriplet(this LineDefinitions source, Triplet<LineDefinition> triplet)
        {
            source.Remove(triplet.Before);
            source.Remove(triplet.Content);
            source.Remove(triplet.After);
            return source;
        }

        public static LineDefinitions AddTriplet(this LineDefinitions source, WallCollection<FrameworkElement> line)
        {
            return source.AddTriplet(line.Position);
        }

        public static LineDefinitions RemoveTriplet(this LineDefinitions source, WallCollection<FrameworkElement> line)
        {
            return source.RemoveTriplet(line.Position);
        }

        public static ContentSize GetContentSize(this FrameworkElement item)
        {
            return item is IPanelItem ? (item as IPanelItem).ContentSize : Wall.GetContentSize(item);
        }

        public static T SetContentSize<T>(this T source, ContentSize size) where T : FrameworkElement
        {
            var item = source as IPanelItem;
            if (item != null) item.ContentSize = size;
            else Wall.SetContentSize(source, size);
            return source;
        }


        public static T SetOrientedCellsRegion<T>(this T source, OrientedCellsRegion region, Orientation orientation)
            where T : FrameworkElement
        {
            return source.SetCellsRegion(region.ToCellsRegion(orientation));
        }

        public static T SetPosition<T>(this OrientedCellsRegion region, T source, Orientation orientation)
            where T : FrameworkElement
        {
            return region.ToCellsRegion(orientation).SetPosition(source);
        }
    }
}