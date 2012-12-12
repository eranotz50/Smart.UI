using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Subjects;

namespace Smart.UI.Widgets.PanelAdorners
{
    public class AdornerCollection<T> : SmartCollection<IAdorner<T>>, IAttacheble<T> where T : FlexCanvas
    {
        public SimpleSubject<IAdorner<T>> Update = new SimpleSubject<IAdorner<T>>();
        private T _host;

        public AdornerCollection()
        {
            ItemsAdded.DoOnNext += OnAdded;
            ItemsRemoved.DoOnNext += OnRemoved;
        }

        public AdornerCollection(IEnumerable<IAdorner<T>> val)
        {
            foreach (var adorner in val)
            {
                Add(adorner);
            }
        }

        #region Implementation of IAttacheble<in T>

        /// <summary>
        /// to be implemented in future
        /// </summary>
        /// <param name="panel"></param>
        public virtual void Attach(T panel)
        {
            //this.Host = panel;
        }

        /// <summary>
        /// to be implemented in future
        /// </summary>
        /// <param name="panel"></param>
        public virtual void Detach(T panel)
        {
            //this.Host = null;
        }

        #endregion

        /// <summary>
        /// Panel to wich adorner is attached
        /// </summary>
        public T Host
        {
            get { return _host; }
            set
            {
                if (_host == value) return;
                if (_host != null) Detach(_host);
                _host = value;
                if (value != null) Attach(value);
            }
        }

        public void OnUpdate(IAdorner<T> source)
        {
            // if (BlockPanelUpdate) return;
            Update.OnNext(source);
            if (Host != null) Host.InvalidateMeasure();
        }


        /// <summary>
        /// Fires when element of the adornercollection has been added
        /// </summary>
        /// <param name="item"></param>
        protected void OnAdded(IAdorner<T> item)
        {
            Contract.Ensures(Host != null);
            if (item.ActivateOnVisible && Host.Parent == null)
                Host.AfterArrange.At(0).Add(f => item.Attach(Host));
            else item.Attach(Host);
            item.Update += OnUpdate;
            OnUpdate(item);
        }

        protected void OnRemoved(IAdorner<T> item)
        {
            Contract.Ensures(Host != null && item.Update != null);
            item.Update -= OnUpdate;
            item.Detach(Host);
            OnUpdate(item);
        }
    }
}