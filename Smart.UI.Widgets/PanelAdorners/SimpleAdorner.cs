using System;
using System.Windows;
using Smart.UI.Panels;

namespace Smart.UI.Widgets.PanelAdorners
{
    /// <summary>
    /// Базовый класс адорнера
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SimpleAdorner<T> : DependencyObject, IAdorner<T> where T : SimplePanel
    {
        private bool _activateOnVisible = true;

        protected bool _activated;
        protected T _host;

        public Boolean Activated
        {
            get { return _activated; }
            set { SetActivated(value); }
        }

        protected virtual T Host
        {
            get { return _host; }
            set { SetHost(value); }
        }

        #region IAdorner<T> Members

        public Boolean ActivateOnVisible
        {
            get { return _activateOnVisible; }
            set { _activateOnVisible = value; }
        }


        public Boolean Attached
        {
            get { return Host != null && Activated; }
        }

        public Action<IAdorner<T>> Update { get; set; }

        public virtual void Attach(T panel)
        {
            if (Attached)
                if (Host == panel) return;
                else Detach(Host);
            Host = panel;
            Activate();
        }


        public virtual void Detach(T panel)
        {
            Deactivate();
            Host = null;
        }

        #endregion

        protected virtual void SetHost(T value)
        {
            _host = value;
        }

        protected virtual void SetActivated(Boolean value)
        {
            _activated = value;
        }

        public virtual void Activate()
        {
            Host.BeforeMeasurement.DoOnNext += OnBeforeMeasurement;
            Host.BeforeArrange.DoOnNext += OnBeforeArrange;
            Host.AfterMeasurement.DoOnNext += OnAfterMeasurement;
            Host.AfterArrange.DoOnNext += OnAfterArrange;
            Activated = true;
        }

        public virtual void Deactivate()
        {
            Host.BeforeMeasurement.DoOnNext -= OnBeforeMeasurement;
            Host.BeforeArrange.DoOnNext -= OnBeforeArrange;
            Host.AfterMeasurement.DoOnNext -= OnAfterMeasurement;
            Host.AfterArrange.DoOnNext -= OnAfterArrange;
            Activated = false;
        }

        protected abstract void OnBeforeMeasurement(Size constrains);
        protected abstract void OnAfterMeasurement(Size constrains);
        protected abstract void OnBeforeArrange(Size constrains);
        protected abstract void OnAfterArrange(Size constrains);
    }
}