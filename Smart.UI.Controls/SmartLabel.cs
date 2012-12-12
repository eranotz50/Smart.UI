using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Controls
{
    /// <summary>
    /// Just a TextBox alternative but with working background
    /// </summary>
    [ContentProperty("Text")]
    public class SmartLabel : RichTextBox
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof (String), typeof (SmartLabel),
                                        new PropertyMetadata(default(String),
                                                             BasicSmartPanel.InvalidateMeasureCallback));

        public String Text
        {
            get { return (String) GetValue(TextProperty); }
            set
            {
                if (value == Text) return;
                SetValue(TextProperty, value);        
            #if WPF
                Block b = this.Document.Blocks.First();
                
                //this.Document.Blocks.First().Inlines.First().Text = value;
              
            #elif SILVERLIGHT    
                Blocks.First().Inlines.First().Text = value;
            #endif
            }
        }
    }
}