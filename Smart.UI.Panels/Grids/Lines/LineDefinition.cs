using System;
using Smart.Classes.Collections;
using Smart.Classes.Arguments;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    /// <summary>
    /// A definition of a row or a column
    /// </summary>
    public class LineDefinition : RelativeLength, INumerated
    {
        private ActionWithArgs<LineDefinition, bool, bool> _autoChange;
        private string _length = "*";
        private int _num;
        private Action<int, int> _numChanged;

        private ActionWithArgs<LineDefinition, double, double> _starsChange;

        private ActionWithArgs<LineDefinition, double, double> _valueChange;


        public LineDefinition() : base(1.0, 0.0)
        {
        }

        public LineDefinition(String str) : base(str)
        {
        }

        public LineDefinition(double length) : base(length)
        {
        }

        public LineDefinition(double stars, double starLen) : base(stars, starLen)
        {
        }

        public LineDefinition(double stars, double starLen, double min, double max = double.PositiveInfinity)
            : base(stars, starLen, min, max)
        {
        }

        public LineDefinition(RelativeLength rel) : base(rel)
        {
        }

        #region OPERATIONS

        public LineDefinition Add(double shift)
        {
            AbsoluteValue += shift;
            return this;
        }

        public LineDefinition AddStars(double star)
        {
            Stars += star;
            return this;
        }

        public LineDefinition SetValue(double value)
        {
            AbsoluteValue = value;
            return this;
        }

        public LineDefinition SetStars(double stars)
        {
            Stars = stars;
            return this;
        }

        #endregion

        public String Length
        {
            get { return _length; }
            set
            {
                if (value == _length || value == "") return;
                if (!value.Contains("*")) _stars = double.PositiveInfinity;
                _length = value;
                FromString(value);
            }
        }

        public ActionWithArgs<LineDefinition, bool, bool> AutoChange
        {
            get
            {
                return _autoChange ??
                       (_autoChange = new ActionWithArgs<LineDefinition, bool, bool>(this, _isAuto, _isAuto));
            }
        }

        public ActionWithArgs<LineDefinition, double, double> StarsChange
        {
            get
            {
                return _starsChange ??
                       (_starsChange = new ActionWithArgs<LineDefinition, double, double>(this, _stars, _stars));
            }
        }

        public ActionWithArgs<LineDefinition, double, double> ValueChange
        {
            get
            {
                return _valueChange ??
                       (_valueChange = new ActionWithArgs<LineDefinition, double, double>(this, Value, Value));
            }
        }

        public override bool IsAuto
        {
            get { return base.IsAuto; }
            set
            {
                if (_isAuto == value) return;
                if (_autoChange != null) _autoChange.Param1 = _isAuto;
                _isAuto = value;
                if (_autoChange != null) _autoChange.Execute(value);
            }
        }


        public override double Stars
        {
            get { return base.Stars; }
            set
            {
                if (_stars.Equals(value)) return;
                if (_starsChange != null) _starsChange.Param1 = _stars;
                _stars = value;
                UpdateValueByStar();
                if (_starsChange != null) _starsChange.Execute(value);
            }
        }

        public override double AbsoluteValue
        {
            get { return Value; }
            set
            {
                if (value.Equals(Value) || !Value.IsValid()) return;
                if (_valueChange != null) _valueChange.Param1 = Value;
                base.AbsoluteValue = value;
                if (_valueChange != null) _valueChange.Execute(value);
            }
        }

        #region INumerated Members

        public int Num
        {
            get { return _num; }
            set
            {
                if (_num.Equals(value)) return;
                int old = _num;
                _num = value;
                if (_numChanged != null) _numChanged(old, value);
            }
        }

        public Action<int, int> NumChanged
        {
            get { return _numChanged; }
            set { _numChanged = value; }
        }

        #endregion
    }
}