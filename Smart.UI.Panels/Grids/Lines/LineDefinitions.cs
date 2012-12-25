using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Controls;
using Smart.Classes.Collections;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;
using Smart.Classes.Extensions;

namespace Smart.UI.Panels
{
    public class LineDefinitions : NumeratedCollection<LineDefinition>, IOrientable
    {
        public SmartCollection<LineDefinition> Autos;

        public ConstrainsMode ConstrainsMode = ConstrainsMode.Default; //NAPILNIK

        /// <summary>
        /// Cached stared linedefs
        /// </summary>
        public SmartCollection<LineDefinition> Stars;

        public Action Update;
        public SmartCollection<LineDefinition> Values;

        private PanelUpdateMode _panelUpdateMode = PanelUpdateMode.None;

        #region DELTAS

        public double DeltaAbs;
        public double DeltaAuto;
        public double DeltaStars;

        public double DeltaForce
        {
            get { return _deltaForce; }
            set
            {
                if (_deltaForce.Equals(value)) return;
                _deltaForce = value;
                Update();
            }
        }

        public double DeltaValues
        {
            get { return DeltaAuto + DeltaAbs /*+this.DeltaForce*/; }
        }

        public double DeltaAll
        {
            get { return DeltaAbs + DeltaStars + (GrowAuto ? DeltaAuto : 0.0); }
        }

        protected void ClearDeltas()
        {
            DeltaForce = 0.0;
            DeltaAuto = 0.0;
            DeltaAbs = 0.0;
            DeltaStars = 0.0;
        }

        #endregion

        public LineDefinitions()
        {
            Init();
        }


        public LineDefinitions(IEnumerable<LineDefinition> list) : base(list)
        {
            Init();
            SortLines();
        }

        #region LENGTH AND COORDINATES RELATED

        protected Boolean BlockNextUpdate; //NAPILNIK, нужно когда мы принуждаем панельку расти
        public Boolean BlockPanelUpdate;

        public double StarLength;

        private double _deltaForce;
        private bool _growAuto;
        private double _length;

        public double AllAutos
        {
            get { return Autos.Sum(l => l.Value); }
        }

        public Boolean GrowAuto
        {
            get { return _growAuto; }
            set
            {
                if (!value)
                {
                    DeltaAuto = 0;
                    DeltaForce -= AllAutos;
                }
                else
                    DeltaForce += AllAutos;
                _growAuto = value;
                Update();
            }
        }

        public Boolean HasStars
        {
            get { return Stars.Count > 0; }
        }

        public double Length
        {
            get { return _length; }
            set
            {
                Contract.Requires(value >= 0 && !double.IsInfinity(value), "Invalid argument for LineDefinitions.Length");
                Contract.Ensures(Math.Abs(Length - (AllStarsLength + AllValuesLength + UnUsed)) < 0.01);
                ClearDeltas();
                //if(_Equals(element)) return; //УБрал оптимизацию, так как она приводит к ряду багов
                BlockPanelUpdate = true;
                _length = value;

                AllValuesLength = Values.Sum(line => line.Value);
                AllStars = Stars.Sum(line => line.StarsLimited);
                //var autos = Autos.Sum(line => line.ValueLimited);                    
                StarLength = (value - AllValuesLength)/AllStars;
                foreach (LineDefinition line in Stars) line.StarLength = StarLength;
                if (HasStars)
                {
                    UnUsed = 0;
                }
                else
                {
                    UnUsed = _length - AllValuesLength;
                }
                BlockPanelUpdate = false;
                ClearDeltas();
            }
        }

        public double AllStarsLength
        {
            get { return Stars.Sum(line => line.AbsoluteValue); }
        }


        /// <summary>
        /// Temporal getter for real length (length used by lines. If there is at least one star it is the same as length)
        /// </summary>
        public double RealLength
        {
            get { return Length - UnUsed; }
        }

        #region TEMPORAL BUGFIX

        public double AllStars;

        /// <summary>
        /// Sum of all absolute values;
        /// </summary>
        public double AllValuesLength;

        public double UnUsed;

        #endregion

        public void AssignStart(double pool)
        {
            throw new NotImplementedException();
            /*
            StarLength = pool/AllStars;
            foreach (LineDefinition line in Stars)
            {
                line.StarLength = StarLength;
            }
             * */
        }


        public double GetCoord(int num)
        {
            if (num < 0) num = Count + num; //for negative line nums
            double retval = 0;
            for (int i = 0; i < num; i++)
                retval += this[i].Value;
            return retval;
        }


        /// <summary>
        /// Поссчитать длинну
        /// </summary>
        /// <param name="from">с рядка</param>
        /// <param name="span">спан и в Африке спан</param>
        /// <returns></returns>
        public double GetLength(int from, int span)
        {
            if (from < 0) from = Count + from; //for negative line nums
            double retval = 0;
            int max = Math.Min(from + span, Count); //выход за границы
            for (int i = from; i < max; i++)
                retval += this[i].Value;
            return retval;
        }

        /// <summary>
        /// Поссчитать длинну
        /// </summary>
        /// <param name="from">с рядка</param>
        /// <param name="span">спан и в Африке спан</param>
        /// <param name="errorLen"> </param>
        /// <returns></returns>
        public double GetLength(int from, int span, double errorLen)
        {
            if (from < 0) from = Count + from; //for negative linenums
            if (Count < from + span) return errorLen;
            double retval = 0;
            int max = Math.Min(from + span, Count); // если выход за границы
            for (int i = from; i < max; i++)
                retval += this[i].Value;
            return retval;
        }

        /// <summary>
        /// Получить длинну справа от контролза
        /// </summary>
        /// <param name="num">номер рядка или колонки</param>
        /// <returns></returns>
        public double GetRight(int num)
        {
            Contract.Requires(num >= 0 && !double.IsInfinity(num));
            double retval = 0;
            for (int i = Count - 1; i >= num; i--)
                retval += this[i].Value;
            return retval + UnUsed;
        }


        /// <summary>
        /// Gets clothest line (row or column) to the coordinate
        /// </summary>
        /// <param name="coord">coordinate to which we shall search the nearest line</param>
        /// <returns>line distance</returns>
        public LineDistance GetLineDistance(double coord)
        {
            if (coord <= 0) return new LineDistance(coord, 0, false);
            double delta = -coord;
            for (int i = 0; i < Count; i++)
            {
                if (delta.Abs() > this[i].Value)
                {
                    delta += this[i].Value;
                    continue;
                }
                if (i == Count - 1) return new LineDistance {Dist = delta, Found = true, Num = i};
                double delta2 = delta + this[i].Value;
                return delta.Abs() < delta2.Abs()
                           ? new LineDistance(delta, i, true)
                           : new LineDistance(delta2, i + 1, true);
            }
            return new LineDistance(delta + Count > 0 ? this.Last().Value : 0.0, Count, false);
        }


        public LineDistance GetLineDistance(double coord, double len)
        {
            LineDistance line1 = GetLineDistance(coord);
            LineDistance line2 = GetLineDistance(coord + len);
            if (!line1.Found) return line2;
            if (!line2.Found) return line1;
            return line1.Dist.Abs() <= line2.Dist.Abs() ? line1 : line2;
        }

        #endregion

        #region INIT AND HANDLERS

        protected virtual void Init()
        {
            Stars = new SmartCollection<LineDefinition>();
            Autos = new SmartCollection<LineDefinition>();
            Values = new SmartCollection<LineDefinition>();
            Stars.ItemsAdded.DoOnNext += StarAddedHandler;
            ItemsAdded.DoOnNext += AddHandler;
            ItemsRemoved.DoOnNext += RemoveHandler;
        }


        /// <summary>
        /// Assigns starlenth to stared linedefinition
        /// </summary>
        /// <param name="ld"></param>
        protected void StarAddedHandler(LineDefinition ld)
        {
            ld.StarLength = StarLength;
        }


        /// <summary>
        /// assignes handlers to all lines added previously
        /// </summary>
        protected void SortLines()
        {
            foreach (LineDefinition item in this) AddHandler(item);
        }

        /// <summary>
        /// On LinDef's addition to LineDefinitions
        /// </summary>
        /// <param name="ld"></param>
        protected void AddHandler(LineDefinition ld)
        {
            OnUpdate(ld);
            SortLineDef(ld);
            ld.StarsChange.Action += OnStarsChange;
            ld.ValueChange.Action += OnValueChange;
            ld.AutoChange.Action += OnAutoChange;
            UpdatePanelHandler(ld, 0.0, ld.Value); //adds row to rowlength
        }

        /// <summary>
        /// On LineDef's removal from LineDefinitions and unsubscribes it completely
        /// </summary>
        /// <param name="ld"></param>
        protected void RemoveHandler(LineDefinition ld)
        {
            OnUpdate(ld);
            ld.StarsChange.Action -= OnStarsChange;
            ld.ValueChange.Action -= OnValueChange;
            ld.AutoChange.Action -= OnAutoChange;
            if (ld.IsAuto) Autos.Remove(ld);
            if (ld.IsAbsolute) Values.Remove(ld);
            else Stars.Remove(ld);
            UpdatePanelHandler(ld, ld.Value, 0.0); //substracts deleted row
        }


        /// <summary>
        /// defines where to put line
        /// </summary>
        /// <param name="ld"></param>
        protected void SortLineDef(LineDefinition ld)
        {
            if (ld.IsAuto) Autos.Add(ld);
            if (ld.IsStar) Stars.Add(ld);
            if (ld.IsAbsolute) Values.Add(ld);
        }

        /// <summary>
        /// Changes autos handler
        /// </summary>
        /// <param name="ld"></param>
        /// <param name="oldVal"></param>
        /// <param name="newVal"></param>
        protected void OnAutoChange(LineDefinition ld, bool oldVal, bool newVal)
        {
            if (newVal)
            {
                Autos.Add(ld);
                if (ld.Stars.IsValidPositive()) Stars.Remove(ld);
            }
            else
            {
                Autos.Remove(ld);
                if (ld.IsStar) Values.Remove(ld);
            }
            OnUpdate(ld);
        }


        protected virtual void OnStarsChange(LineDefinition ld, double oldVal, double newVal)
        {
            OnUpdate(ld);
            if (ld.IsAuto) return;
            if (newVal.IsValidPositive())
            {
                if (!oldVal.IsValidPositive())
                {
                    Stars.Add(ld);
                    Values.Remove(ld);
                }
            }
            else
            {
                if (oldVal.IsValidPositive())
                {
                    Stars.Remove(ld);
                    Values.Add(ld);
                }
            }
        }

        protected virtual void OnValueChange(LineDefinition ld, double oldVal, double newVal)
        {
            OnUpdate(ld);
            UpdatePanelHandler(ld, oldVal, newVal);
        }


        protected void UpdatePanelHandler(LineDefinition ld, double oldVal, double newVal)
        {
            if (BlockNextUpdate)
            {
                BlockNextUpdate = false;
                return;
            }
            if (!oldVal.IsValid() || !newVal.IsValid()) return;
            double delta = newVal - oldVal;
            if (ld.IsStar)
                DeltaStars += delta;
            else if (ld.IsAuto)
            {
                if (GrowAuto) DeltaForce += delta;
                else DeltaAuto += delta;
            }
            else if (ld.IsAbsolute)
                DeltaAbs += delta;
        }

        #endregion

        #region GROW LINE Регион для ресайза линии

        public LineDefinitions GrowLineOnly(int num, double shift)
        {
            BlockNextUpdate = true;
            this[num].AbsoluteValue += shift;
            return this;
        }


        public LineDefinitions GrowLineWithPanel(int num, double shift)
        {
            BlockNextUpdate = true;
            this[num].AbsoluteValue += shift;
            DeltaForce += shift;
            return this;
        }

        public LineDefinitions GrowLineCutRightNeighbour(int num, double shift)
        {
            this[num].AbsoluteValue += shift;
            if (num < Count - 1) this[num + 1].AddAbsolute(-shift);
            ;
            return this;
        }


        public LineDefinitions GrowLineCutLeftNeighbour(int num, double shift)
        {
            this[num].AbsoluteValue += shift;
            if (num > 0) this[num - 1].AddAbsolute(-shift);
            ;
            return this;
        }


        public LineDefinitions GrowLineCutNeighbours(int num, double shift)
        {
            this[num].AbsoluteValue += shift;
            if (num > 0) this[num - 1].AddAbsolute(-shift/2);
            if (num < Count - 1) this[num + 1].AddAbsolute(-shift/2);
            return this;
        }

        #endregion

        #region MOVE LINES region for lines movements

        /// <summary>
        /// Split function
        /// </summary>
        /// <param name="start">Line num to grow</param>
        /// <param name="end">Line num to cut</param>
        /// <param name="shift">how much to grow</param>
        /// <returns></returns>
        public LineDefinitions SplitLines(int start, int end, double shift)
        {
            if (end >= Count) return this;
            LineDefinition source = this[start];
            LineDefinition target = this[end];
            shift = shift < 0.0 ? Math.Max(shift, -source.Value) : Math.Min(shift, target.Value);
            source.AbsoluteValue += shift;
            target.AbsoluteValue -= shift;
            OnUpdate(source);
            OnUpdate(target);
            return this;
        }


        public LineDefinitions MoveLines(int start, int end, double shift)
        {
            if (end >= Count) return this;
            if (start == 0 || end == Count - 1 || shift.Equals(0.0)) return this;
            return SplitLines(start - 1, end + 1, shift);
        }

        public LineDefinitions MoveLine(int num, double shift)
        {
            return MoveLines(num, num, shift);
        }

        #endregion

        public PanelUpdateMode PanelUpdateMode
        {
            get { return _panelUpdateMode; }
            set { _panelUpdateMode = value; }
        }

        #region IOrientable Members

        public Orientation Orientation { get; set; }

        #endregion

        public void OnUpdate(LineDefinition source)
        {
            if (BlockPanelUpdate) return;
            if (Update != null) Update();
        }
    }
}