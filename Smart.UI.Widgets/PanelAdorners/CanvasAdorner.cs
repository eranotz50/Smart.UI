using System;
using System.Windows;
using Smart.UI.Panels;
using Smart.Classes.Subjects;

namespace Smart.UI.Widgets.PanelAdorners
{
    public class CanvasAdorner<T> : SimpleAdorner<T>, IAdorner<T> where T : FlexCanvas
    {
        public T Panel;

        protected override void SetHost(T value)
        {
            T val = value;
            if (val == null)
            {
                string str = value.Name.Equals("") ? "" : value.Name + " is not Compartible with this Adorner";
                throw new Exception("Not compartible panel " + str);
            }
            base.SetHost(value);
            Panel = value;
        }


        protected override void OnBeforeMeasurement(Size constrains)
        {
        }

        protected override void OnAfterMeasurement(Size constrains)
        {
        }

        protected override void OnBeforeArrange(Size constrains)
        {
        }

        protected override void OnAfterArrange(Size constrains)
        {
        }
    }

    public class CanvasAdorner : CanvasAdorner<FlexCanvas>
    {
        public SimpleSubject<CanvasAdorner> OnActivate = new SimpleSubject<CanvasAdorner>();
        public SimpleSubject<CanvasAdorner> OnDeactivate = new SimpleSubject<CanvasAdorner>();

        public CanvasAdorner()
        {
            OnActivate.DoOnNext += Update;
            OnDeactivate.DoOnNext += Update;
        }

        protected override void SetActivated(bool value)
        {
            if (_activated == value) return;
            _activated = value;
            if (value) OnActivate.OnNext(this);
            else OnDeactivate.OnNext(this);
        }
    }
}