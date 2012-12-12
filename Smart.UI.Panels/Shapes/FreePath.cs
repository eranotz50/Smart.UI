using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Smart.UI.Panels
{
    public class FreePath:Shape
    {

        
            /// <summary>
        /// Data property
        /// </summary> 
    
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register( 
            "Data", 
            typeof(Geometry),
            typeof(FreePath), 
            new PropertyMetadata(
                null
                /*DataChangeCallback*/)); 

        public static void DataChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null) return;
            var p = fe.Parent as BasicSmartPanel;
            if (p == null) return;
            p.Dirty = Dirtiness.Measure;
            p.InvalidateMeasure();
        }
        

        /// <summary> 
        /// Data property 
        /// </summary>
        public Geometry Data 
        {
            get
            {
                return (Geometry)GetValue(DataProperty); 
            }
            set 
            { 
                SetValue(DataProperty, value);
            } 
        }
     
#if WPF
        #region Protected Methods and Properties 

        /// <summary> 
        /// Get the path that defines this shape 
        /// </summary>
        protected override Geometry DefiningGeometry 
        {
            get
            {
                Geometry data = Data; 

                if (data == null) 
                { 
                    data = Geometry.Empty;
                } 

                return data;
            }
        } 

   
 
        #endregion 
#endif
    }
}
