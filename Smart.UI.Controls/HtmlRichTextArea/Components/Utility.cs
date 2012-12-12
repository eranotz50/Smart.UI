using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Liquid
{
    public class Utility
    {
        /// <summary>
        /// Parses a string containing a Thickness value and returns a Thickness object
        /// </summary>
        /// <param name="value">String containing a Thickness value</param>
        /// <param name="xaml">Indicates whether it is a XAML Thickness object or not (CSS)</param>
        /// <returns>A Thickness object</returns>
        public static Thickness ParseThickness(string value, bool xaml)
        {
            Thickness result;
            string[] split = (xaml
                                  ? value.Split(',')
                                  : Regex.Replace(value, "px", "", RegexOptions.IgnoreCase).Split(' '));

            if (split.Length == 1)
            {
                result = new Thickness(double.Parse(split[0]));
            }
            else
            {
                if (xaml)
                {
                    result = new Thickness(double.Parse(split[0]), double.Parse(split[1]), double.Parse(split[2]),
                                           double.Parse(split[3]));
                }
                else
                {
                    result = new Thickness(double.Parse(split[3]), double.Parse(split[0]), double.Parse(split[1]),
                                           double.Parse(split[2]));
                }
            }

            return result;
        }

        /// <summary>
        /// Converts a XAML Thickness object to a string for use in CSS margins and paddings (Top,Right,Bottom,Left)
        /// </summary>
        /// <param name="thickness">Thickness to convert</param>
        /// <returns>CSS Thickness string</returns>
        public static string ThicknessToCSS(Thickness thickness)
        {
            string result = thickness.Top.ToString(CultureInfo.InvariantCulture) + "px ";

            result += thickness.Right.ToString(CultureInfo.InvariantCulture) + "px ";
            result += thickness.Bottom.ToString(CultureInfo.InvariantCulture) + "px ";
            result += thickness.Left.ToString(CultureInfo.InvariantCulture) + "px";

            return result;
        }

        /// <summary>
        /// Determines whether a style ID is a HTML heading tag
        /// </summary>
        /// <param name="styleID">Style ID</param>
        /// <returns>True if the style is a heading, false if not</returns>
        public static bool IsStyleAHeading(string styleID)
        {
            string temp = styleID.ToLower();

            return (temp == "h1" || temp == "h2" || temp == "h3" || temp == "h4");
        }

        public static Brush GetBrush(string brush)
        {
            Brush result;
            Color tempColor;

            if (brush.StartsWith("#"))
            {
                if (brush.Length == 7)
                {
                    brush = brush.Replace("#", "#ff");
                }
                tempColor = HtmlRichTextAreaStyle.StringToColor(brush.TrimStart('#'));
                result = new SolidColorBrush(tempColor);
            }
            else
            {
                result = (Brush) CreateFromXaml(brush);
            }

            return result;
        }

        /// <summary>
        /// Determines whether 2 brushes are the same
        /// </summary>
        /// <param name="a">Brush A</param>
        /// <param name="b">Brush B</param>
        /// <returns>Indicates whether the brushes are the same</returns>
        public static bool CompareBrush(Brush a, Brush b)
        {
            bool result = (a == b);

            if (!result && a is SolidColorBrush && b is SolidColorBrush)
            {
                if (((SolidColorBrush) a).Color == ((SolidColorBrush) b).Color)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Creates an element from the supplied XAML string
        /// </summary>
        /// <param name="xaml">XAML string</param>
        /// <returns>Instantiated element</returns>
        public static object CreateFromXaml(string xaml)
        {
            string ns = "http://schemas.microsoft.com/client/2007";

            if (!xaml.Contains(ns))
            {
                xaml = xaml.Insert(xaml.IndexOf('>'), " xmlns=\"" + ns + "\"");
            }

            return XamlReader.Load(xaml);
        }

        /// <summary>
        /// Gets the color of the brush as a 6-digit hex number preceeded by a #
        /// </summary>
        /// <param name="brush">A Brush object</param>
        /// <returns>6-digit hex value</returns>
        internal static string BrushHex6(Brush brush)
        {
            LinearGradientBrush gradientBrush;
            var color = new Color();

            if (brush is SolidColorBrush)
            {
                color = ((SolidColorBrush) brush).Color;
            }
            else if (brush is LinearGradientBrush)
            {
                gradientBrush = (LinearGradientBrush) brush;
                if (gradientBrush.GradientStops.Count > 0)
                {
                    color = gradientBrush.GradientStops[gradientBrush.GradientStops.Count - 1].Color;
                }
            }

            if (color.A == 0)
            {
                return string.Empty;
            }
            else
            {
                return "#" + color.ToString().Substring(3);
            }
        }
    }
}