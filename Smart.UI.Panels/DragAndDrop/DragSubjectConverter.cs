using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Smart.Classes.Extensions;
using Smart.Classes.Subjects;
using Smart.UI.Classes.Converters;

namespace Smart.UI.Panels
{
    public class DragSubjectConverter : DictionaryGeneratorConverter<SimpleSubject<ObjectFly>>,
                                        IConvertFrom<DragMode, SimpleSubject<ObjectFly>>
    {
        private static DragSubjectConverter _instance;

        public static DragSubjectConverter Instance
        {
            get { return _instance ?? (_instance = new DragSubjectConverter()); }
        }

        #region IConvertFrom<DragMode,SimpleSubject<ObjectFly>> Members

        public SimpleSubject<ObjectFly> ConvertFrom(DragMode from)
        {
            string str = Enum.GetName(typeof (DragMode), from);
            return ConvertFrom(str.ToLowerInvariant());
        }

        #endregion

        public override void Init()
        {
            if (In == null) In = new Dictionary<string, Func<SimpleSubject<ObjectFly>>>();

            In.AddUnique("vertical", Generate<VerticalDrag>())
                .AddUnique("horizontal", Generate<HorizontalDrag>())
                .AddUnique("free", Generate<FreeDrag>())
                .AddUnique("", Generate<FreeDrag>())
                .AddUnique("custom", Generate<SimpleSubject<ObjectFly>>())
                .AddUnique("none", Generate<SimpleSubject<ObjectFly>>());
            ;
            Default = Generate<FreeDrag>();
            if (Out != null) return;
            Out = new Dictionary<Func<SimpleSubject<ObjectFly>>, string>();
            foreach (var pair in In.Where(pair => !Out.ContainsKey(pair.Value))) Out.AddUnique(pair.Value, pair.Key);
        }


        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof (string) || sourceType == typeof (DragMode);
        }
    }


}