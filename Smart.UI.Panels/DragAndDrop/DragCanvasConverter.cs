using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Converters;

namespace Smart.UI.Panels
{
    public class DragCanvasFunc : FuncWithArgs<FrameworkElement, DragPanel>
    {
        public DragCanvasFunc(Func<FrameworkElement, DragPanel> func, FrameworkElement param0)
            : base(func, param0)
        {
        }
    }

    public class DragCanvasConverter : DictionaryConverter<DragCanvasFunc>
        /*, IConvertFrom<DragCanvas, FuncWithArgs<FrameworkElement, DragPanel>>*/
    {
        private static DragCanvasConverter _instance;

        public static DragCanvasConverter Instance
        {
            get { return _instance ?? (_instance = new DragCanvasConverter()); }
        }

        public override void Init()
        {
            Default = new HighestPanelFunc();
            if (In == null)
                In = new Dictionary<string, DragCanvasFunc>
                         {
                             {"", new HighestPanelFunc()},
                             {"default", new HighestPanelFunc()},
                             {"nearestparent", new NearestPanelFunc()},
                             {"highestparent", new HighestPanelFunc()},
                             {"nearestpanel", new NearestPanelFunc()},
                             {"highestpanel", new HighestPanelFunc()},
                             {"control", new ControlParentFunc()}
                         };
            if (Out != null) return;
            Out = new Dictionary<DragCanvasFunc, string>();
            foreach (var pair in In) if (!Out.ContainsKey(pair.Value)) Out.Add(pair.Value, pair.Key);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (String); // || sourceType == typeof (DragCanvasFunc);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof (DragCanvasFunc);
        }
    }

    #region FUNCS

    public class NearestPanelFunc : DragCanvasFunc
    {
        public NearestPanelFunc() : base(DragPanelExtensions.GetNearestDragEnabledParent<DragPanel>, null)
        {
        }
    }

    public class HighestPanelFunc : DragCanvasFunc
    {
        public HighestPanelFunc() : base(DragPanelExtensions.GetHighestDragParent<DragPanel>, null)
        {
        }
    }

    public class ControlParentFunc : DragCanvasFunc
    {
        public ControlParentFunc() : base(Handler, null)
        {
        }

        public static DragPanel Handler(FrameworkElement element)
        {
            return element.GetNearestControlParent<DragPanel>();
        }
    }

    #endregion
}