using ZedGraph;
using CerealPotter.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProtoBuf;

namespace CerealPotter
{
    
    public class TriggerPoints
    {
        public TriggerPoints(GraphPane pane, string label, Color fontColor, double minValue, ToolStripProgressBar progressBar)
        {
            _pane = pane;
            _fontColor = fontColor;
            _minValue = minValue;
            ZedGraph.BarItem bar = _pane.AddBar("Triggers", Points, fontColor);
            //_pane.AddCurve("Triggers", Points, fontColor, SymbolType.Star);
            //curve.IsVisible = true;
            //curve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            bar.IsVisible = true;
            Label = label;
            _bar = progressBar;
            points = new ZScore(pane, progressBar);
        }
        private ToolStripProgressBar _bar;
        [ProtoMember(1)]
        public string Label;
        private double _minValue;
        private GraphPane _pane;
        private Color _fontColor;
        private PointPairList Points = new PointPairList();
        private List<GraphObj> _graphObjects = new List<GraphObj>();
        ZScore points;

        public void Add(PointPair newPoint, string name)
        {
            if (name != Label) return;
            points.Add(newPoint);
        }
        public void SetInput(List<PointPair> newPoints)
        {
            points.Clear();
            points.AddRange(new List<PointPair>(newPoints));
        }
        public void Update(List<PointPair> pointPairs, bool replace = false)
        {
            if (points.Count != pointPairs.Count || replace)
            {
                Reset();
                SetInput(new List<PointPair>(pointPairs));
            }
            Update(false);
        }
        public void Update(bool moveWindow = true)
        {
            foreach (var obj in _graphObjects)
            {
                _pane.GraphObjList.Remove(obj);
            }
            _graphObjects.Clear();
            ZScoreOutput zScore = points.GetScores();
            //int fromIndex=0;
            //bool trig = false;
            //for (int i = 0; i < zScore.signals.Count; i++)
            //{
            //    if (!trig && zScore.signals[i].Y != 0)
            //    {
            //        trig = true;
            //        fromIndex = i;
            //    }
            //    else if (trig && zScore.signals[i] != zScore.signals[fromIndex])
            //    {
            //        AddBar(fromIndex, i, zScore.signals[fromIndex].Y);
            //        trig = false;
            //    }
            //}
            //if (moveWindow)
            //{
            //    if (trig)
            //    {
            //        points.MoveTo(fromIndex);
            //    }
            //    else
            //    {
            //        points.MoveTo(zScore.input.Count - 1);
            //    }
            //}
        }
        private void AddBar(int from, int to, double signal)
        {

            double averagemA = points.Average(from, to);
            double deltaSecs = points.Span(from, to);
            double fromX = points[from].X;
            double toX = points[to].X;
            double centerX = points[from].CenterX(points[to]);
            TextObj text = new ZedGraph.TextObj($"{averagemA.FormattedNumber()}mA/{deltaSecs.FormattedNumber()}sec ({(averagemA / (deltaSecs * 60 * 60)).FormattedNumber() }mAh)", centerX, averagemA, CoordType.AxisXYScale, AlignH.Left, AlignV.Center);
            _graphObjects.Add(text);
            List<PointD> pointDs = new List<PointD>();
            pointDs.Add(new PointD(fromX, 0));
            pointDs.Add(new PointD(fromX, averagemA));
            pointDs.Add(new PointD(toX, averagemA));
            pointDs.Add(new PointD(toX, 0));
            pointDs.Add(new PointD(fromX, 0));

            PolyObj poly = new PolyObj(pointDs.ToArray(), Color.Empty, Color.LightBlue)
            {
                IsVisible = true,
                ZOrder = ZOrder.A_InFront
            };


            text.FontSpec.FontColor = _fontColor;
            text.ZOrder = ZOrder.A_InFront;
            // Hide the border and the fill
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.Fill.IsVisible = false;
            text.FontSpec.Size = 12f;
            text.FontSpec.Angle = 45;

            //string lblString = "name";
            //Link lblLink = new Link(lblString, "#", "");
            //text.Link = lblLink;
            _pane.GraphObjList.Add(text);
            _pane.GraphObjList.Add(poly);
            _graphObjects.Add(poly);
            //_pane.AxisChange();
            // also add bar centered on the event
            // Points.Add(TriggerPoint.CenterX(point), averagemA);
        }
        public void Reset()
        {
            foreach (var obj in _graphObjects)
            {
                _pane.GraphObjList.Remove(obj);
            }
            _graphObjects.Clear();
            Points.Clear();
            points.Clear();
        }
    }

}
