using System;
using System.Windows;
using Smart.UI.Panels;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;

namespace Smart.UI.Relatives
{
    public static class ConvertExtensions
    {
        public static Action<Args<FrameworkElement, Rect, Size>, FrameworkElement> ToRelativePositionAction(
            this String source)
        {
            return RelativeConverter.Instance.ConvertFrom(source);
        }

        public static String AsString(this Action<Args<FrameworkElement, Rect, Size>, FrameworkElement> source)
        {
            return RelativeConverter.Instance.ConvertTo(source);
        }

        public static SimpleSubject<ObjectFly> ToDragSubject(this String source)
        {
            return DragSubjectConverter.Instance.ConvertFrom(source);
        }

        /*
        public static String AsString(this SimpleSubject<ObjectFly> source)
        { return DragSubjectConverter.Instance.ConvertTo(source); }
        */
    }
}