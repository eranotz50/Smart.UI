using System;
using System.Windows;
using Smart.UI.Panels;

namespace Smart.UI.Widgets.PanelAdorners
{
    public interface IAdorner<T> : IAttacheble<T> /*,IActivator*/ where T : SimplePanel
    {
        Boolean ActivateOnVisible { get; }
        Boolean Attached { get; }
        Action<IAdorner<T>> Update { get; set; }
        //  T Host { get; set; }
    }

    public interface ILayoutAttacheble
    {
        void OnBeforeMeasurement(Size constrains);
        void OnAfterMeasurement(Size constrains);
        void OnBeforeArrange(Size constrains);
        void OnAfterArrange(Size constrains);
    }

    public interface IActivator
    {
        Boolean Activated { get; set; }
        void Activate();
        void Deactivate();
    }

    public interface IAttacheble<in T> where T : SimplePanel
    {
        void Attach(T panel);
        void Detach(T panel);
    }

    public interface IHandler<in T>
    {
        void ItemAddedHandler(T item);
        void ItemRemoveHandler(T item);
    }
}