using System.Windows.Markup;
using System.Windows.Media;
using Smart.UI.Classes.Utils;
using Smart.UI.Controls;

namespace Design.Data.TestControls
{
    [ContentProperty("Text")]
    public class BlueLabel : SmartLabel
    {
        public BlueLabel()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 14, 170, 220));
            Foreground = new SolidColorBrush(Colors.White);
        }
    }

    [ContentProperty("Text")]
    public class Title : SmartLabel
    {
        public Title()
        {
            Background = new SolidColorBrush(ColorsDesign.GetRandomColor());
            Foreground = new SolidColorBrush(Colors.White);
        }
    }
}