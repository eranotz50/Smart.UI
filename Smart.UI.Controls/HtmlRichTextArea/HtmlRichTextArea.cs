using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Liquid
{
    public class HtmlRichTextArea : RichTextBox
    {
        #region Public Properties

        public Dictionary<string, HtmlRichTextAreaStyle> Styles { get; set; }

        #endregion

        #region Constructor

        public HtmlRichTextArea()
        {
            Styles = new Dictionary<string, HtmlRichTextAreaStyle>();
            SetDefaultStyles();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the default styles H1, H2, H3 and Normal
        /// </summary>
        public void SetDefaultStyles()
        {
            Styles.Clear();
            Styles.Add("H1",
                       new HtmlRichTextAreaStyle("H1", "Arial", 24, FontWeights.Bold, FontStyles.Normal, null,
                                                 new SolidColorBrush(Colors.Black), HtmlElementDisplay.Block));
            Styles.Add("H2",
                       new HtmlRichTextAreaStyle("H2", "Arial", 20, FontWeights.Bold, FontStyles.Normal, null,
                                                 new SolidColorBrush(Colors.Black), HtmlElementDisplay.Block));
            Styles.Add("H3",
                       new HtmlRichTextAreaStyle("H3", "Arial", 18, FontWeights.Bold, FontStyles.Normal, null,
                                                 new SolidColorBrush(Colors.Black), HtmlElementDisplay.Block));
            Styles.Add("Normal",
                       new HtmlRichTextAreaStyle("Normal", "Trebuchet MS", 16, FontWeights.Normal, FontStyles.Normal,
                                                 null, new SolidColorBrush(Colors.Black), HtmlElementDisplay.Inline));
            Styles.Add("Comment",
                       new HtmlRichTextAreaStyle("Comment", "Trebuchet MS", 16, FontWeights.Normal, FontStyles.Normal,
                                                 null, new SolidColorBrush(Colors.Black), HtmlElementDisplay.Inline));
            Styles.Add("Reply",
                       new HtmlRichTextAreaStyle("Reply", "Trebuchet MS", 12, FontWeights.Normal, FontStyles.Italic,
                                                 null, new SolidColorBrush(Colors.Gray), HtmlElementDisplay.Inline));
        }

        /// <summary>
        /// Clears the content
        /// </summary>
        public void Clear()
        {
            Blocks.Clear();
        }

        /// <summary>
        /// Inserts a block of plain text
        /// </summary>
        /// <param name="text">Text to insert</param>
        public void Insert(string text)
        {
            var run = new Run();
            run.Text = text;

            Selection.Insert(run);
        }

        /// <summary>
        /// Inserts a block of formatted text
        /// </summary>
        /// <param name="text">Text to insert</param>
        /// <param name="style">Text style</param>
        public void Insert(string text, HtmlRichTextAreaStyle style)
        {
            var run = new Run();

            run.Text = text;
            style.ApplyToElement(run);

            Selection.Insert(run);
        }

        /// <summary>
        /// Inserts an image at the cursor
        /// </summary>
        /// <param name="url">Image Url</param>
        public void InsertImage(Uri url, double width = 0, double height = 0)
        {
            var image = new Image
                            {
                                Source = new BitmapImage(url),
                                Stretch = Stretch.None
                            };
            if (width > 0 && height == 0)
            {
                image.Width = width;
                image.Stretch = Stretch.UniformToFill;
            }
            else if (width == 0 && height > 0)
            {
                image.Height = height;
                image.Stretch = Stretch.UniformToFill;
            }
            else if (width > 0 && height > 0)
            {
                image.Height = height;
                image.Width = width;
                image.Stretch = Stretch.Fill;
            }
            InsertElement(image);
        }

        /// <summary>
        /// Inserts any UIElement at the cursor
        /// </summary>
        /// <param name="element">Any element derived from UIElement</param>
        public void InsertElement(UIElement element)
        {
            var imageContainer = new InlineUIContainer();
            imageContainer.Child = element;

            Selection.Insert(imageContainer);
        }

        /// <summary>
        /// Attempts to load a block of HTML into the control
        /// </summary>
        /// <param name="html">Valid XHTML</param>
        public void Load(string html, double imageWidth = 0, double imageHeight = 0)
        {
            XmlReader reader;
            string tag;
            string inlineStyle;
            string styleID;
            var styleStack = new Stack<HtmlRichTextAreaStyle>();
            HtmlRichTextAreaStyle currentStyle;

            Clear();
            reader =
                XmlReader.Create(
                    new StringReader("<content>" + html.Replace("&nbsp;", "[RTB_HTML_SPACER]") + "</content>"));
            reader.Read();

            currentStyle = new HtmlRichTextAreaStyle(Styles["Normal"]);
            styleStack.Push(currentStyle);

            while (!reader.EOF && reader.ReadState != ReadState.Error)
            {
                tag = reader.Name.ToUpper();

                try
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            inlineStyle = reader.GetAttribute("style");
                            styleID = reader.GetAttribute("class");
                            currentStyle = styleStack.Peek();

                            if (styleID != null && Styles.ContainsKey(styleID))
                            {
                                currentStyle = new HtmlRichTextAreaStyle(Styles[styleID]);
                            }
                            if (inlineStyle != null)
                            {
                                currentStyle.FromInlineStyle(inlineStyle);
                            }

                            switch (tag)
                            {
                                case "H1":
                                case "H2":
                                case "H3":
                                    if (Styles.ContainsKey(tag))
                                    {
                                        currentStyle = new HtmlRichTextAreaStyle(Styles[tag]);
                                        if (inlineStyle != null)
                                        {
                                            currentStyle.FromInlineStyle(inlineStyle);
                                        }
                                    }
                                    CreateParagraph(currentStyle);
                                    break;
                                case "IMG":
                                    string src = string.Empty;

                                    if (reader.GetAttribute("src") != null)
                                    {
                                        src = reader.GetAttribute("src");

                                        AddImageElement(new Uri(src, UriKind.RelativeOrAbsolute), imageWidth,
                                                        imageHeight);
                                    }
                                    break;
                                case "BR":
                                    break;
                                case "HR":
                                    break;
                                case "P":
                                case "DIV":
                                    CreateParagraph(currentStyle);
                                    break;
                                case "SPAN":
                                    currentStyle = new HtmlRichTextAreaStyle(currentStyle);
                                    currentStyle.Display = HtmlElementDisplay.Inline;
                                    break;
                                case "A":
                                    string url = string.Empty;

                                    if (reader.GetAttribute("href") != null)
                                    {
                                        url = reader.GetAttribute("href");
                                    }
                                    string inxml = "";
                                    try
                                    {
                                        inxml = reader.ReadInnerXml();
                                    }
                                    catch
                                    {
                                    }
                                    AddHyperlinkElement(inxml, url, styleStack.Peek());
                                    break;
                                case "B":
                                case "STRONG":
                                    currentStyle = new HtmlRichTextAreaStyle(currentStyle);
                                    currentStyle.FontWeight = FontWeights.Bold;
                                    break;
                                case "I":
                                case "ITALIC":
                                    currentStyle = new HtmlRichTextAreaStyle(currentStyle);
                                    currentStyle.FontStyle = FontStyles.Italic;
                                    break;
                                case "U":
                                case "UNDERLINE":
                                    currentStyle = new HtmlRichTextAreaStyle(currentStyle);
                                    currentStyle.TextDecoration = TextDecorations.Underline;
                                    break;
                                case "UL":
                                    break;
                                case "OL":
                                    break;
                                case "LI":
                                    break;
                            }

                            if (tag != "IMG" && tag != "BR" && tag != "CONTENT")
                            {
                                styleStack.Push(currentStyle);
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (tag != "CONTENT")
                            {
                                styleStack.Pop();
                            }

                            switch (tag)
                            {
                                case "H1":
                                case "H2":
                                case "H3":
                                case "P":
                                case "DIV":
                                    break;
                            }
                            break;
                        case XmlNodeType.Text:
                            var style = new HtmlRichTextAreaStyle();
                            try
                            {
                                style = styleStack.Peek();
                            }
                            catch
                            {
                            }
                            try
                            {
                                AddTextElement(reader.Value.Replace("[RTB_HTML_SPACER]", " "), style);
                            }
                            catch
                            {
                            }
                            break;
                    }
                }
                catch
                {
                }
                reader.Read();
            }
        }

        /// <summary>
        /// Saves the content as HTML
        /// </summary>
        /// <returns>The control content as HTML</returns>
        public string Save()
        {
            var results = new StringBuilder();

            foreach (Block block in Blocks)
            {
                if (block is Paragraph)
                {
                    results.Append(SerializeBlock((Paragraph) block));
                }
            }

            return results.ToString();
        }

        public void SetAlignment(TextAlignment alignment)
        {
            foreach (Block block in Blocks)
            {
                block.TextAlignment = alignment;
            }
        }

        #endregion

        #region Private Methods

        private void CreateParagraph(HtmlRichTextAreaStyle style)
        {
            var p = new Paragraph();
            Blocks.Add(p);
            style.ApplyToElement(p);
        }

        private string SerializeBlock(Paragraph block)
        {
            var results = new StringBuilder();
            HtmlRichTextAreaStyle style;
            HtmlRichTextAreaStyle lastStyle = Styles["Normal"];
            string css = string.Empty;
            string attributes = string.Empty;
            string tag = "p";

            style = GetStyle(block);
            if (style != null)
            {
                lastStyle = style;

                if (style.ID == "H1" || style.ID == "H2" || style.ID == "H3")
                {
                    tag = style.ID.ToLower();
                }
            }
            results.Append("<" + tag + ">");

            ProcessInlines(block.Inlines, ref results, lastStyle, attributes);

            results.Append("</" + tag + ">" + Environment.NewLine);

            return results.ToString();
        }

        private void ProcessInlines(InlineCollection inlines, ref StringBuilder results, HtmlRichTextAreaStyle lastStyle,
                                    string attributes)
        {
            Hyperlink link;
            Image image;
            BitmapImage bitmapImage;
            InlineUIContainer container;
            Run span;
            Span _span;
            string css;
            HtmlRichTextAreaStyle style;
            foreach (Inline inline in inlines)
            {
                style = GetStyle(inline);
                if (style == null)
                {
                    style = new HtmlRichTextAreaStyle("custom", inline.FontFamily.ToString(), inline.FontSize,
                                                      inline.FontWeight,
                                                      inline.FontStyle, inline.TextDecorations, inline.Foreground,
                                                      HtmlElementDisplay.Inline);
                }

                css = style.ToInlineCSS(lastStyle);
                lastStyle = style;

                if (css.Length > 0)
                {
                    attributes = " style=\"" + css + "\"";
                }

                switch (inline.GetType().ToString())
                {
                    case "System.Windows.Documents.LineBreak":
                        results.Append(Environment.NewLine);
                        break;
                    case "System.Windows.Documents.Span":
                        ProcessInlines(((Span) inline).Inlines, ref results, lastStyle, attributes);
                        break;
                    case "System.Windows.Documents.Run":
                        span = (Run) inline;
                        results.Append("<span" + attributes + ">" + span.Text + "</span>" + Environment.NewLine);
                        break;
                    case "System.Windows.Documents.Hyperlink":
                        link = (Hyperlink) inline;
                        results.Append("<a" + attributes + " href=\"" + link.NavigateUri + "\">" +
                                       ((Run) link.Inlines[0]).Text + "</a>" + Environment.NewLine);
                        break;
                    case "System.Windows.Documents.InlineUIContainer":
                        container = (InlineUIContainer) inline;
                        switch (container.Child.GetType().ToString())
                        {
                            case "System.Windows.Controls.Image":
                                image = (Image) container.Child;
                                if (image.Source is BitmapImage)
                                {
                                    bitmapImage = (BitmapImage) image.Source;
                                    results.Append("<img" + attributes + " src=\"" + bitmapImage.UriSource + "\" />" +
                                                   Environment.NewLine);
                                }
                                break;
                        }
                        break;
                }
            }
        }

        private HtmlRichTextAreaStyle GetStyle(TextElement element)
        {
            HtmlRichTextAreaStyle result = null;

            foreach (var style in Styles)
            {
                if (element.FontFamily.ToString() == style.Value.FontFamily &&
                    element.FontSize == style.Value.FontSize.Value &&
                    element.FontStyle == style.Value.FontStyle && element.FontWeight == style.Value.FontWeight &&
                    Utility.CompareBrush(element.Foreground, style.Value.Foreground))
                {
                    if (element is Run)
                    {
                        if (element.GetValue(Inline.TextDecorationsProperty) != style.Value.TextDecoration)
                        {
                            continue;
                        }
                    }

                    result = style.Value;
                    break;
                }
            }

            return result;
        }

        private void AddTextElement(string text, HtmlRichTextAreaStyle style)
        {
            Run inline;

            inline = new Run();
            inline.Text = text;
            style.ApplyToElement(inline);

            if (Blocks.Count > 0)
            {
                ((Paragraph) Blocks[Blocks.Count - 1]).Inlines.Add(inline);
            }
        }

        private void AddHyperlinkElement(string text, string url, HtmlRichTextAreaStyle style)
        {
            Paragraph p;
            Hyperlink inline;

            switch (style.Display)
            {
                case HtmlElementDisplay.Block:
                    p = new Paragraph();
                    Blocks.Add(p);
                    style.ApplyToElement(p);
                    break;
            }

            inline = new Hyperlink();
            inline.NavigateUri = new Uri(url, UriKind.Absolute);
            inline.TargetName = "_blank";
            inline.Inlines.Add(text);
            style.ApplyToElement(inline);

            if (Blocks.Count > 0)
            {
                ((Paragraph) Blocks[Blocks.Count - 1]).Inlines.Add(inline);
            }
        }

        private void AddImageElement(Uri src, double width = 0, double height = 0)
        {
            Paragraph p;
            InlineUIContainer container;
            Image image;

            image = new Image
                        {
                            Source = new BitmapImage(src),
                            Stretch = Stretch.None
                        };
            if (width > 0 && height == 0)
            {
                image.Width = width;
                image.Stretch = Stretch.UniformToFill;
            }
            else if (width == 0 && height > 0)
            {
                image.Height = height;
                image.Stretch = Stretch.UniformToFill;
            }
            else if (width > 0 && height > 0)
            {
                image.Height = height;
                image.Width = width;
                image.Stretch = Stretch.Fill;
            }
            container = new InlineUIContainer
                            {
                                Child = image
                            };

            if (Blocks.Count > 0)
            {
                ((Paragraph) Blocks[Blocks.Count - 1]).Inlines.Add(container);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                string text = Clipboard.GetText();
                //var lines = text.Split("\r\n".ToCharArray());
                //foreach (var line in lines)
                //{
                //    AddTextElement(line, Styles["Normal"]);
                //}
                //this.Selection.Text = text;

                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        #endregion

        //#region bindings

        ///// <summary>
        ///// 
        ///// </summary>
        //public string Text
        //{
        //    get { SetValue(TextProperty, Save()); return (string)GetValue(TextProperty); }
        //    set { SetValue(TextProperty, value); Load(value); }
        //}

        //// Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty TextProperty =
        //    DependencyProperty.Register("Text", typeof(string), typeof(HtmlRichTextArea), null);


        //#endregion
    }
}