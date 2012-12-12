using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Liquid
{
    public enum HtmlElementDisplay
    {
        Block,
        Inline
    };

    public class HtmlRichTextAreaStyle
    {
        public HtmlRichTextAreaStyle()
        {
            ID = string.Empty;
            FontFamily = "Arial";
            FontSize = 12;
            Margin = new Thickness();
            VerticalAlignment = VerticalAlignment.Center;
            FontWeight = FontWeights.Normal;
            Display = HtmlElementDisplay.Inline;
        }

        public HtmlRichTextAreaStyle(HtmlRichTextAreaStyle style) :
            this(
            style.ID, style.FontFamily, style.FontSize, style.FontWeight, style.FontStyle, style.TextDecoration,
            style.Foreground, style.Display)
        {
        }

        public HtmlRichTextAreaStyle(string id, string fontFamily, double? fontSize, FontWeight? fontWeight,
                                     FontStyle? fontStyle, TextDecorationCollection decorations, Brush foreground,
                                     HtmlElementDisplay display)
        {
            ID = id;
            FontFamily = fontFamily;
            FontSize = fontSize;
            FontWeight = fontWeight;
            FontStyle = fontStyle;
            TextDecoration = decorations;
            Foreground = foreground;
            Display = display;
        }

        public string ID { get; set; }
        public string FontFamily { get; set; }
        public double? FontSize { get; set; }
        public Brush Foreground { get; set; }
        public FontWeight? FontWeight { get; set; }
        public FontStyle? FontStyle { get; set; }
        public TextDecorationCollection TextDecoration { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public Thickness Margin { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public HtmlElementDisplay Display { get; set; }

        public void ApplyToElement(TextElement element)
        {
            element.FontFamily = new FontFamily(FontFamily);
            element.FontSize = FontSize.Value;
            element.FontStyle = FontStyle.Value;
            element.FontWeight = FontWeight.Value;

            if (Foreground != null)
            {
                element.Foreground = Foreground;
            }

            if (element is Run)
            {
                element.SetValue(Inline.TextDecorationsProperty, TextDecoration);
            }
        }

        /// <summary>
        /// Sets the specified property to a value
        /// </summary>
        /// <param name="key">Property name</param>
        /// <param name="value">Property value</param>
        public void SetProperty(string key, string value)
        {
            Color tempColor;

            if (value != null)
            {
                switch (key)
                {
                    case "ID":
                        ID = value;
                        break;
                    case "FontFamily":
                        FontFamily = value;
                        break;
                    case "FontSize":
                        FontSize = double.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "FontStyle":
                        switch (value.ToLower())
                        {
                            case "italic":
                                FontStyle = FontStyles.Italic;
                                break;
                            default:
                                FontStyle = FontStyles.Normal;
                                break;
                        }
                        break;
                    case "FontWeight":
                        switch (value.ToLower())
                        {
                            case "bold":
                                FontWeight = FontWeights.Bold;
                                break;
                            default:
                                FontWeight = FontWeights.Normal;
                                break;
                        }
                        break;
                    case "Decoration":
                        switch (value.ToLower())
                        {
                            case "underline":
                                TextDecoration = TextDecorations.Underline;
                                break;
                            case "line-through":
                                break;
                            default:
                                TextDecoration = null;
                                break;
                        }
                        break;
                    case "Alignment":
                        HorizontalAlignment =
                            (HorizontalAlignment) Enum.Parse(typeof (HorizontalAlignment), value, true);
                        break;
                    case "Background":
                        break;
                    case "Foreground":
                        tempColor = StringToColor(value.TrimStart('#'));
                        Foreground = new SolidColorBrush(tempColor);
                        break;
                    case "VerticalAlignment":
                        VerticalAlignment =
                            (VerticalAlignment)
                            Enum.Parse(typeof (VerticalAlignment),
                                       Regex.Replace(value, "middle", "Center", RegexOptions.IgnoreCase), true);
                        break;
                }
            }
        }

        public static Color StringToColor(string paramValue)
        {
            byte red;
            byte green;
            byte blue;
            byte alpha;
            Color result = Colors.Transparent;

            try
            {
                paramValue = paramValue.TrimStart('#');

                alpha = (byte.Parse(paramValue.Substring(0, 2), NumberStyles.AllowHexSpecifier));
                red = (byte.Parse(paramValue.Substring(2, 2), NumberStyles.AllowHexSpecifier));
                green = (byte.Parse(paramValue.Substring(4, 2), NumberStyles.AllowHexSpecifier));
                blue = (byte.Parse(paramValue.Substring(6, 2), NumberStyles.AllowHexSpecifier));

                result = Color.FromArgb(alpha, red, green, blue);
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public string ToInlineCSS(HtmlRichTextAreaStyle currentStyle)
        {
            return ToInlineCSS(currentStyle, null);
        }

        public string ToInlineCSS(HtmlRichTextAreaStyle currentStyle, UIElement element)
        {
            var result = new StringBuilder();
            Dictionary<string, string> pairs;
            Dictionary<string, string> current;
            string value;

            pairs = ToHTML(element);
            current = currentStyle.ToHTML(element);

            foreach (var pair in pairs)
            {
                if (pair.Key != "font-size" && pair.Key != "color" && pair.Key != "font-style")
                    if (current.TryGetValue(pair.Key, out value))
                    {
                        current.Remove(pair.Key);

                        if (value == pair.Value)
                        {
                            continue;
                        }
                    }
                result.Append(pair.Key + ":" + pair.Value + ";");
            }

            pairs = GetStyleDefaults(current);
            foreach (var pair in pairs)
            {
                result.Append(pair.Key + ":" + pair.Value + ";");
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets the style as an HTML style
        /// </summary>
        /// <returns>HTML CSS style tags</returns>
        public Dictionary<string, string> ToHTML()
        {
            return ToHTML(null);
        }

        /// <summary>
        /// Gets the style as an HTML style
        /// </summary>
        /// <returns>HTML CSS style tags</returns>
        public Dictionary<string, string> ToHTML(object element)
        {
            var keyValues = new Dictionary<string, string>();
            bool isText = (element is TextElement);

            //if (isText)
            {
                keyValues.Add("font-family", FontFamily);
                keyValues.Add("font-size",
                              Math.Round((double) FontSize, 2).ToString(CultureInfo.InvariantCulture) + "px");

                if (Foreground != null)
                {
                    if (((SolidColorBrush) Foreground).Color.A > 0)
                    {
                        keyValues.Add("color", Utility.BrushHex6(Foreground));
                    }
                }

                if (FontWeight == FontWeights.Bold)
                {
                    keyValues.Add("font-weight", "bold");
                }
                if (FontStyle == FontStyles.Italic)
                {
                    keyValues.Add("font-style", "italic");
                }
                if (TextDecoration == TextDecorations.Underline && !keyValues.ContainsKey("text-decoration"))
                {
                    keyValues.Add("text-decoration", "underline");
                }
            }

            if (!keyValues.ContainsKey("vertical-align"))
            {
                keyValues.Add("vertical-align", VerticalAlignment.ToString().Replace("Center", "middle"));
            }

            if (HorizontalAlignment != HorizontalAlignment.Left)
            {
                keyValues.Add("text-align", HorizontalAlignment.ToString().ToLower());
            }

            if (Margin.Bottom != 0 || Margin.Top != 0 || Margin.Left != 0 || Margin.Right != 0)
            {
                keyValues.Add("margin", Utility.ThicknessToCSS(Margin));
            }

            return keyValues;
        }

        public static Dictionary<string, string> GetStyleDefaults(Dictionary<string, string> styles)
        {
            var results = new Dictionary<string, string>();
            string newValue = string.Empty;

            foreach (var pair in styles)
            {
                switch (pair.Key)
                {
                    case "font-family":
                        newValue = string.Empty;
                        break;
                    case "font-size":
                        newValue = string.Empty;
                        break;
                    case "font-weight":
                        newValue = "normal";
                        break;
                    case "font-style":
                        newValue = "normal";
                        break;
                    case "text-decoration":
                        newValue = "none";
                        break;
                    case "text-align":
                        newValue = "left";
                        break;
                    case "color":
                        newValue = string.Empty;
                        break;
                    case "background":
                        newValue = string.Empty;
                        break;
                    case "vertical-align":
                        newValue = "middle";
                        break;
                    case "margin":
                        newValue = "0px 0px 0px 0px";
                        break;
                    default:
                        newValue = string.Empty;
                        break;
                }

                if (newValue.Length > 0)
                {
                    results.Add(pair.Key, newValue);
                }
            }

            return results;
        }

        /// <summary>
        /// Gets a dictionary of style key/values
        /// </summary>
        /// <param name="style">Text style</param>
        /// <returns>Dictionary of key/values</returns>
        public static Dictionary<string, string> GetDictionaryFromStyle(string style)
        {
            var results = new Dictionary<string, string>();
            string[] split3;
            string[] split4;

            if (style == null)
            {
                return results;
            }

            split3 = style.Split(';');
            foreach (string a in split3)
            {
                split4 = a.Trim().Split(':');

                if (split4.Length > 1)
                {
                    results.Add(split4[0].ToLower(), split4[1]);
                }
            }

            return results;
        }

        public void FromInlineStyle(string style)
        {
            Dictionary<string, string> keyValues = GetDictionaryFromStyle(style);
            string temp;

            foreach (var pair in keyValues)
            {
                switch (pair.Key)
                {
                    case "font-family":
                        SetProperty("FontFamily", pair.Value);
                        break;
                    case "font-size":
                        SetProperty("FontSize", pair.Value.Replace("px", ""));
                        break;
                    case "font-weight":
                        SetProperty("FontWeight", pair.Value);
                        break;
                    case "font-style":
                        SetProperty("FontStyle", pair.Value);
                        break;
                    case "text-decoration":
                        SetProperty("Decoration", pair.Value);
                        break;
                    case "text-align":
                        SetProperty("Alignment", pair.Value);
                        break;
                    case "color":
                        SetProperty("Foreground", pair.Value.Replace("#", "#FF"));
                        break;
                    case "background":
                        SetProperty("Background", pair.Value.Replace("#", "#FF"));
                        break;
                    case "vertical-align":
                        temp = pair.Value.ToLower();
                        if (temp == "super")
                        {
                        }
                        else if (temp == "sub")
                        {
                        }
                        else
                        {
                            SetProperty("VerticalAlignment", pair.Value);
                        }
                        break;
                    case "margin":
                        SetProperty("Margin", pair.Value);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Converts a CSS style to a RichText style
        /// </summary>
        /// <param name="style">CSS style</param>
        /// <returns>RichText styles</returns>
        public void FromHTML(string style)
        {
            string[] split2;

            split2 = style.Trim().Split('{');
            if (split2.Length > 1)
            {
                ID = split2[0].Trim().Replace(".", "");
                FromInlineStyle(split2[1]);
            }
        }

        public override string ToString()
        {
            return ID;
        }
    }
}