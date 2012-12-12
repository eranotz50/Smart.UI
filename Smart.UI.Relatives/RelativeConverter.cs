using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Converters;

namespace Smart.UI.Relatives
{
    /// <summary>
    /// Converts relative positioning strings into actions
    /// </summary>
    public class RelativeConverter : DictionaryConverter<Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>>
    {
        public static Boolean OddActions = false; //true;

        private static RelativeConverter _instance;

        public static RelativeConverter Instance
        {
            get { return _instance ?? (_instance = new RelativeConverter()); }
        }

        
        /// <summary>
        /// Applies several related positioning functions
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public override Action<Args<FrameworkElement, Rect, Size>, FrameworkElement> ConvertFrom(string str)
        {
            if(!str.Contains(";")) return base.ConvertFrom(str);
            string[] strs = str.Split(';');
            return strs.Aggregate<string, Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>>(null, (current, s) => current + base.ConvertFrom(s));         
        }

        public override void Init()
        {
            if (In == null)
                In = new Dictionary<string, Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>>
                         {

                             {"sameleft", RelativeFunctions.PosSameLeft},
                             {"sametop", RelativeFunctions.PosSameTop},
                             {"samecoords", RelativeFunctions.PosSameCoords},                             
                             
                             {"left", RelativeFunctions.PosLeft},
                             {"right", RelativeFunctions.PosRight},
                             {"bottom", RelativeFunctions.PosBottom},
                             {"top", RelativeFunctions.PosTop},
                             
                             {"leftside", RelativeFunctions.PosLeftSide},
                             {"rightside", RelativeFunctions.PosRightSide},
                             {"bottomside", RelativeFunctions.PosBottomSide},                             
                             {"topside", RelativeFunctions.PosTopSide},

                           
                             //Elements bellow should be derprecated soon

                             {"lefttop", RelativeFunctions.PosLeftTop},
                             {"righttop", RelativeFunctions.PosRightTop},
                             {"leftbottom", RelativeFunctions.PosLeftBottom},
                             {"rightbottom", RelativeFunctions.PosRightBottom},
                             {"rightmiddle", RelativeFunctions.PosRightMiddle},
                             {"leftmiddle", RelativeFunctions.PosLeftMiddle},                             
                             {"centerbottom", RelativeFunctions.PosCenterBottom},
                             {"centertop", RelativeFunctions.PosCenterTop},
                        
                             
                             {"topleft", RelativeFunctions.PosLeftTop},
                             {"topright", RelativeFunctions.PosRightTop},
                             {"bottomleft", RelativeFunctions.PosLeftBottom},
                             {"bottomright", RelativeFunctions.PosRightBottom},                            
                             {"middleright", RelativeFunctions.PosRightMiddle},                             
                             {"middleleft", RelativeFunctions.PosLeftMiddle},
                             {"bottomcenter", RelativeFunctions.PosCenterBottom},
                             {"topcenter", RelativeFunctions.PosCenterTop}
                             



                         };
            if (Out != null) return;
            Out = new Dictionary<Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>, string>();
            foreach (var pair in In) if (!Out.ContainsKey(pair.Value)) Out.Add(pair.Value, pair.Key);
        }

    }
}