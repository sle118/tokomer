using ZedGraph;
using CerealPotter.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections;
using System.ComponentModel;
using ProtoBuf;

namespace CerealPotter
{
    [Serializable]
    [TypeConverter(typeof(AveragePoints))]
    [ProtoContract]
    public class AveragePoints :  ISerializable, IPointList, IPointListEdit
    {
        public AveragePoints() 
        { }
        static private int _k => (int)(Settings.Default.PeakDetectionLag_ms * Settings.Default.ReadsPerSec / 1000);
        static private int capacity => (int)Settings.Default.ReadsPerSec * (int)Settings.Default.BufferLenSeconds;
        [ProtoMember(3)]
        public string Name
        {
            get => name; set
            {
                name = value; if (Curve != null)
                {
                    Curve.Label.Text = FullName;
                }
            }
        }
        [ProtoMember(4)]
        public string Prefix
        {
            get => prefix; set
            {
                prefix = value;
                if (Curve != null)
                {
                    Curve.Label.Text = FullName;
                }
            }
        }
        public string FullName
        {
            get
            {

                if (Prefix.Length > 0 && Name.Length > 0)
                {
                    return Prefix + " - " + Name;
                }
                return Name;
            }
        }
        

        double alpha => (double)2 / (_k + 1);
        private LineBase _lineBase;
        private Fill _fillBase;
        private Symbol _symbolBase;
        [ProtoMember(1)] 
        public bool Averaging { get; set; }
        [ProtoMember(2)]
        private List<PointPair> input = new List<PointPair>();

        [ProtoMember(5)]
        private RollingPointPairList _rollingPointPairList;

        // calculate the new sum
        double result = 0;
        public bool Selected = false;
        public LineItem Curve { get; set; }
        public GraphPane Pane { get; set; }
        public List<PointPair> Input { get => input; set => input = value; }
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                if (Curve != null)
                {
                    Curve.Color = Selected ? value.DarkColor() : value;
                    Curve.Line.Color = Curve.Color;
                    Curve.Line.Fill.Color = Curve.Color;
                    Curve.Symbol.Fill.Color = Curve.Color;
                    _lineBase.Color = value;
                    _fillBase.Color = value;
                    _symbolBase.Fill.Color = value;
                }

            }
        }

        public int Count => ((IPointList)_rollingPointPairList).Count;

        PointPair IPointListEdit.this[int index] { get => ((IPointListEdit)_rollingPointPairList)[index]; set => ((IPointListEdit)_rollingPointPairList)[index] = value; }

        public PointPair this[int index] => ((IPointList)_rollingPointPairList)[index];

        private Color color;

        private object LockObject = new object();
        private string prefix;
        private string name;

        public void Reset()
        {
            input.Clear();
            if (_rollingPointPairList != null)
            {
                _rollingPointPairList.Clear();
            }
            if (Pane != null && Curve != null)
            {
                Pane.CurveList.Remove(Curve);
            }
        }
        public int IndexOfPoint(PointPair pp)
        {
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].X == pp.X) return i;
            }
            return -1;
        }
        public void RemoveAt(PointPair pp)
        {
            int idx = _rollingPointPairList.IndexOf(pp);
            if (idx >= 0)
            {
                _rollingPointPairList.RemoveAt(idx);
            }
            input.RemoveAt(IndexOfPoint(pp));
        }
        public void CreateCurve(GraphPane pane)
        {
            Pane = pane;
            CreateCurve();
        }
        public void CreateCurve()
        {
            if (Pane == null) throw new Exception("Invalid pane");
            lock (LockObject)
            {
                Curve = Pane.AddCurve(FullName, _rollingPointPairList, Color, SymbolType.None);
                Curve.IsSelectable = true;
                Curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                _lineBase = new LineBase(Curve.Line);
                _fillBase = new Fill(Curve.Line.Fill);
                _symbolBase = new Symbol(Curve.Symbol);

            }
        }

        public void ToggleSelection()
        {
            Selected = !Selected;
            if (Selected)
            {
                Curve.Line.Width = _lineBase.Width * 2;
                Curve.Line.Fill.Color = Color.DarkColor();
                Curve.Symbol.Type = SymbolType.Square;
                Curve.Symbol.Fill.IsVisible = true;
                Curve.Symbol.Fill.Color = Color.DarkColor();
            }
            else
            {
                Unselect();
            }
            Pane.AxisChange();
        }

        public void Unselect()
        {
            Curve.Line.CopyAttributes(_lineBase);
            Curve.Symbol = new Symbol(_symbolBase);
            Selected = false;
        }
        public AveragePoints(string name, string prefix, GraphPane pane, Color color)
        {
            Pane = pane;
            Name = name;
            Prefix = prefix;
            Color = color;
            _rollingPointPairList = new RollingPointPairList(capacity);
            CreateCurve();
        }
        public AveragePoints(IPointList rhs) 
        {
            _rollingPointPairList = new RollingPointPairList(rhs);
        }

        public AveragePoints(int capacity, bool preLoad) 
        {
            _rollingPointPairList = new RollingPointPairList(capacity,preLoad);
        }

        public override int GetHashCode()
        {
            return (base.GetHashCode(), Averaging, input).GetHashCode();
        }

        protected AveragePoints(SerializationInfo info, StreamingContext context) 
        {
            try
            {
                Averaging = (bool)info.GetValue("averaging", Averaging.GetType());
                input = (List<PointPair>)info.GetValue("average_list", input.GetType());
                Name = info.GetString("name");
                Prefix = info.GetString("prefix");
                _rollingPointPairList = (RollingPointPairList)info.GetValue("average_list", _rollingPointPairList.GetType()); 
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message} ", "Data Read Error", MessageBoxButtons.OK);
                throw e;
            }
        }
        //[SecurityPermission(SecurityAction.LinkDemand,    Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("rollingPointPairList", _rollingPointPairList );
            info.AddValue("averaging", Averaging);
            info.AddValue("average_list", input);
            info.AddValue("name", Name);
            info.AddValue("prefix", Prefix);
        }
        private void AddPoint(PointPair item)
        {
            lock (LockObject)
            {
                input.Add(new PointPair(item));
                result = alpha * item.Y + (1 - alpha) * result;

                if (Averaging && input.Count % _k == 0 && input.Count > 0)
                {
                    _rollingPointPairList.Add(new PointPair(item.X, result));
                }
                else if (!Averaging)
                {
                    _rollingPointPairList.Add(item);
                }
            }
        }


        public  void Add(PointPair item)
        {
            AddPoint(item);
        }

        public  void Add(double[] x, double[] y, double[] z)
        {
            int len = 0;

            if (x != null)
                len = x.Length;
            if (y != null && y.Length > len)
                len = y.Length;
            if (z != null && z.Length > len)
                len = z.Length;

            for (int i = 0; i < len; i++)
            {
                PointPair point = new PointPair();
                if (x == null)
                    point.X = (double)i + 1.0;
                else if (i < x.Length)

                    point.X = x[i];
                else
                    point.X = PointPairBase.Missing;

                if (y == null)
                    point.Y = (double)i + 1.0;
                else if (i < y.Length)

                    point.Y = y[i];
                else
                    point.Y = PointPairBase.Missing;

                if (z == null)
                    point.Z = (double)i + 1.0;
                else if (i < z.Length)

                    point.Z = z[i];
                else
                    point.Z = PointPairBase.Missing;

                AddPoint(point);
            }
        }
        public  void Add(double[] x, double[] y)
        {
            int len = 0;

            if (x != null)
                len = x.Length;
            if (y != null && y.Length > len)
                len = y.Length;

            for (int i = 0; i < len; i++)
            {
                PointPair point = new PointPair(0, 0, 0);
                if (x == null)
                    point.X = (double)i + 1.0;
                else if (i < x.Length)
                    point.X = x[i];
                else
                    point.X = PointPairBase.Missing;

                if (y == null)
                    point.Y = (double)i + 1.0;
                else if (i < y.Length)
                    point.Y = y[i];
                else
                    point.Y = PointPairBase.Missing;

                AddPoint(point);
            }
        }

        public  void Add(double x, double y, double z)
        {
            AddPoint(new PointPair(x, y, z));
        }
        public  void Add(double x, double y)
        {
            AddPoint(new PointPair(x, y));
        }
        public  void Add(double x, double y, object tag)
        {
            AddPoint(new PointPair(x, y, PointPairBase.Missing, tag));
        }
        public  void Add(IPointList pointList)
        {
            for (int i = 0; i < pointList.Count; i++)
                AddPoint(pointList[i]);
        }
        public  void Add(double x, double y, double z, object tag)
        {
            AddPoint(new PointPair(x, y, z, tag));
        }
        public object Clone()
        {
            return ((ICloneable)_rollingPointPairList).Clone();
        }

        public void RemoveAt(int index)
        {
            ((IPointListEdit)_rollingPointPairList).RemoveAt(index);
        }

        public void Clear()
        {
            ((IPointListEdit)_rollingPointPairList).Clear();
        }
    }

}
