using System;
using System.Windows;
using System.Windows.Controls;
using Smart.UI.Panels;
using Smart.Classes.Interfaces;
using Smart.Classes.Subjects;


namespace Smart.UI.Controls
{
    public static class ControlExtensions
    {
        public static FlexCanvas GetRoot(this SmartControl source)
        {
            return source.FromTemplate<FlexCanvas>("Root");
        }

        public static T GetRoot<T>(this SmartControl source) where T : SimplePanel
        {
            return source.FromTemplate<T>("Root");
        }
    }

    public abstract class SmartControl : Control, IChildElement, INamed
    {
        protected SmartControl()
        {
            OnAddedToStage = new SimpleSubject<SimplePanel>();
            OnRemovedFromStage = new SimpleSubject<SimplePanel>();
            OnAddedToStage.DoOnNext += AddedToStageHandler;
            OnRemovedFromStage.DoOnNext += RemovedFromStageHandler;
        }

        #region IChildElement Members

        public SimpleSubject<SimplePanel> OnAddedToStage { get; set; }
        public SimpleSubject<SimplePanel> OnRemovedFromStage { get; set; }

        #endregion

        public T FromTemplate<T>(String elementName) where T : FrameworkElement
        {
            return GetTemplateChild(elementName) as T;
        }

        public FrameworkElement FromTemplate(String elementName)
        {
            return GetTemplateChild(elementName) as FrameworkElement;
        }

        protected virtual void AddedToStageHandler(SimplePanel panel)
        {
        }

        protected virtual void RemovedFromStageHandler(SimplePanel panel)
        {
        }
    }
}