using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Smart.UI.Panels;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Converters;

namespace Smart.UI.Panels
{
    public class DragTargetConverter : DictionaryConverter<DragTargetFunc>
    {
        private static DragTargetConverter _instance;

        public static DragTargetConverter Instance
        {
            get { return _instance ?? (_instance = new DragTargetConverter()); }
        }

        public override void Init()
        {
            Default = new SelfTarget();
            if (In == null)
                In = new Dictionary<string, DragTargetFunc>
                         {
                             {"self", Default},
                             {"parent", new ParentTarget()},
                             {"dragparent", new DragParentTarget()},
                             {"control", new ControlTarget()},
                             {"", Default},
                         };
            if (Out != null) return;
            Out = new Dictionary<DragTargetFunc, string>();
            foreach (var pair in In.Where(pair => !Out.ContainsKey(pair.Value))) Out.Add(pair.Value, pair.Key);
        }

        /*
        public FuncWithArgs<FrameworkElement,FrameworkElement> ConvertFrom(DragTarget from)
        {
            var str = Enum.GetName(typeof(DragTarget), from);
            return ConvertFrom(str.ToLowerInvariant());
        }


        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(DragTarget);
        }
         */
    }

    #region DRAGTARGETS

    public class DragTargetFunc : FuncWithArgs<FrameworkElement, FrameworkElement>
    {
        public DragTargetFunc(Func<FrameworkElement, FrameworkElement> func, FrameworkElement param0)
            : base(func, param0)
        {
        }
    }

    public class ControlTarget : DragTargetFunc
    {
        public ControlTarget() : base(FrameworkElementExtensions.GetParent<Control>, null)
        {
        }
    }

    public class SelfTarget : DragTargetFunc
    {
        public SelfTarget() : base(i => i, null)
        {
        }
    }

    public class ParentTarget : DragTargetFunc
    {
        public ParentTarget() : base(FrameworkElementExtensions.GetParent<BasicSmartPanel>, null)
        {
        }
    }

    public class DragParentTarget : DragTargetFunc
    {
        public DragParentTarget() : base(DragPanelExtensions.GetNearestDragParent, null)
        {
        }
    }

    #endregion
}