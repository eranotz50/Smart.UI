using System.Windows;
using System.Windows.Input;
using Smart.Classes.Bindables;
using Smart.Classes.Collections;
using Smart.UI.Classes.Other;
using Smart.Classes.Bindables;

namespace DesignData
{
    public abstract class SampleData: Bindable
    {
        private SmartCollection<FrameworkElement> _items;
        public SmartCollection<FrameworkElement> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                this.RaisePropertyChanged(i=>this.Items);
            }
        }

        protected SampleData(int count = 10)
        {
            Init(count);
        }


        public string Text { get; set; }

        public abstract void Init(int count);

        public abstract FrameworkElement Make(int num);

        public void Handler(object sender, MouseButtonEventArgs args)
        {
        }
    }

}
