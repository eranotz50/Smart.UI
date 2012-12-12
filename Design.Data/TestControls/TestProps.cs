using System;
using System.Windows;
using Smart.Classes.Graphs;
using Smart.Classes.Dictionaries;
using Smart.Classes.Arguments;
using Smart.UI.Relatives;

namespace Design.Data.TestControls
{
    public class TestPos : ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>
    {
        public TestPos(Action<Args<FrameworkElement, Rect, Size>, FrameworkElement> action,
                       Args<FrameworkElement, Rect, Size> param0, FrameworkElement param1)
            : base(action, param0, param1)
        {
        }

        public TestPos()
            : base(RelativeFunctions.PosRightTop, null, null)
        {
            ;
        }
    }

    public class TestDataNode : DependencyObject, IDataNode
    {
        public TestDataNode()
        {
            Properties = new SmartDictionary<string, object>
                             {
                                 {"text", "Some Test Name"},
                                 {"image", "/Smart.UI.Graphs;component/Data/portraits/14325.jpg"}
                             };
        }

        public String Text
        {
            get { return (String) Properties["text"]; }
            set { Properties["text"] = value; }
        }

        #region IDataNode Members

        public Guid Id { get; set; }

        public bool SameId(IIDentifiable other)
        {
            return Id.Equals(other.Id);
        }

        public string NodeId { get; set; }
        public SmartDictionary<string, object> Properties { get; set; }

        #endregion
    }
}