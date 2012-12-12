using System.Windows;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    public class CanvasPlaceholder : Placeholder
    {
        public virtual double GetWidth(double slotWidth)
        {
            double width = slotWidth;
            if (Left.IsValid()) width -= Left;
            if (Right.IsValid()) width -= Right;
            return width; //Math.Abs(width);
        }

        public virtual double GetHeight(double slotHeight)
        {
            double height = slotHeight;
            if (Top.IsValid()) height -= Top;
            if (Bottom.IsValid()) height -= Bottom;
            return height; // Math.Abs(height);
        }

        public override Size GetSize(Size constrains)
        {
            return new Size(GetWidth(constrains.Width).NotLessThan(1.0), GetHeight(constrains.Height).NotLessThan(1.0));
        }


        /// <summary>
        /// класс для рассчета относительного расположения в канвасе
        /// </summary>
        /// <param name="size">желаемый размер контролза</param>
        /// <param name="constrains">ограничения канваса по размеру</param>
        /// <returns></returns>
        public override Rect GetBoundary(Size size, Size constrains)
        {
            var current = new Point(0, 0);
            if (Left.IsValid())
            {
                current.X = Left;
                if (Right.IsValid()) size.Width = (constrains.Width - Left - Right).NotLessThan(1.0);
                else if (Center.IsValid()) size.Width = ((constrains.Width/2) - Left + Center).NotLessThan(1.0);
            }
            else if (Right.IsValid())
            {
                current.X = constrains.Width - size.Width - Right;
                if (Center.IsValid()) size.Width = ((constrains.Width/2) - Right + Center).NotLessThan(1.0);
            }
            else if (Center.IsValid()) current.X = constrains.Width/2 - size.Width/2 + Center;

            if (Top.IsValid())
            {
                current.Y = Top;
                if (Bottom.IsValid()) size.Height = (constrains.Height - Top - Bottom).NotLessThan(1.0);
                else if (Middle.IsValid()) size.Height = ((constrains.Height/2) - Top + Middle).NotLessThan(1.0);
            }
            else if (Bottom.IsValid())
            {
                current.Y = constrains.Height - size.Height - Bottom;
                if (Middle.IsValid()) size.Height = ((constrains.Height/2) - Bottom + Middle).NotLessThan(1.0);
            }
            else if (Middle.IsValid()) current.Y = constrains.Height/2 - size.Height/2 + Middle;
            return new Rect(current, size);
        }
    }
}