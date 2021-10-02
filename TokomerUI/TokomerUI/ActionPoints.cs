using ZedGraph;
using CerealPotter.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace CerealPotter
{
    public enum UI_Action
    {
        NONE,
        REMOVE_POINTS,
        OFFSET_POINTS
    }
    public class ActionPoints<T> where T : IPointList, IPointListEdit
    {

        public ActionPoints(CustomGraphControl Control, string label, Color fontColor, ToolStripProgressBar progressBar, UI_Action action, WorkCompletedCallBack callBack)
        {
            _pane = Control.GraphPane;
            ctrl = Control;
            _fontColor = fontColor;
            _bar = progressBar;
            _action = action;
            _callBack = (callBack);
        }
        private ToolStripProgressBar _bar;
        CustomGraphControl ctrl;
        private GraphPane _pane;
        private Color _fontColor;
        private LineItem rangeLine;
        private List<GraphObj> _graphObjects = new List<GraphObj>();
        private T points;
        private PointPairList rangePoints;
        public delegate void WorkCompletedCallBack();
        
        int _fromIndex = 0;
        int _count = 0;
        private UI_Action _action;
        private WorkCompletedCallBack _callBack;
        private static double PointSize = 4;
        PointPair to;
        double offset = 0;
        private PointPair From;
        public PointPair To
        {
            get => to;
            set
            {
                to = value;
                _fromIndex = Math.Min(points.IndexOf(From), points.IndexOf(to));
                int toIndex = Math.Max(points.IndexOf(From), points.IndexOf(to));
                _count = toIndex - _fromIndex;
                offset = Math.Max(points[_fromIndex].X, points[toIndex].X) - Math.Min(points[_fromIndex].X, points[toIndex].X);
                rangePoints = new PointPairList();
                for (int i = _fromIndex; i < points.Count && i < _fromIndex + _count; i++)
                {
                    rangePoints.Add(points[i]);
                }
                ShowPoint(To);
                DrawSelection();
                GetConfirmation();

            }
        }
        public void GetConfirmation()
        {
            switch (_action)
            {
                case UI_Action.NONE:
                    break;
                case UI_Action.REMOVE_POINTS:

                    new Thread(new ThreadStart(delegate
                    {
                        if (MessageBox.Show
                        ("Are you sure you want to remove the selected points?",
                          "Confirmation",
                          MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Warning
                        ) == DialogResult.OK)
                        {
                            points.RemoveRange(_fromIndex, _count);
                            for (int i = _fromIndex; i < points.Count; i++)
                            {
                                points[i].X -= offset;
                            }
                        }
                        Cleanup();
                        _callBack();
                    })).Start();
                    break;
                case UI_Action.OFFSET_POINTS:
                    new Thread(new ThreadStart(delegate
                    {
                        if (MessageBox.Show
                        ("Are you sure you want to move the selected points?",
                          "Confirmation",
                          MessageBoxButtons.OKCancel,
                          MessageBoxIcon.Warning
                        ) == DialogResult.OK)
                        {
                            double offset = From.X - To.X;
                            for (int i = 0; i < points.Count; i++)
                            {
                                points[i].X = i + offset;
                            }
                        }
                        Cleanup();
                        _callBack();
                    })).Start();
                    break;
                default:
                    break;
            }

        }
        public void SetInput(ref T newPoints, PointPair from)
        {
            if (rangePoints != null)
            {
                rangePoints = null;
            }
            points = newPoints;
            From = from;
            ShowPoint(from);
        }

        public void ShowPoint(PointPair point)
        {
            PointObj pointObj = new PointObj(point.X, point.Y, 10, 10, SymbolType.Plus, Color.Black);
            pointObj.Fill = new ZedGraph.Fill(Color.Black);
            Graphics graphics = ctrl.CreateGraphics();

            // Draw point to graph
            pointObj.Draw(graphics, _pane, _pane.CalcScaleFactor());


            _pane.GraphObjList.Add(pointObj);
            _graphObjects.Add(pointObj);
            ReDraw();
        }

        private void DrawRectangle(PointPair from, PointPair to, double minY, double maxY)
        {
            double fromX = from.X;
            double toX = to.X;
            List<PointD> pointDs = new List<PointD>();
            pointDs.Add(new PointD(fromX, minY));
            pointDs.Add(new PointD(fromX, maxY));
            pointDs.Add(new PointD(toX, maxY));
            pointDs.Add(new PointD(toX, minY));
            pointDs.Add(new PointD(fromX, minY));
            PolyObj poly = new PolyObj(pointDs.ToArray(), Color.Empty, Color.LightBlue)
            {
                IsVisible = true,
                ZOrder = ZOrder.A_InFront
            };

            _pane.GraphObjList.Add(poly);
            _graphObjects.Add(poly);
            ReDraw();

        }
        private void DrawSelection()
        {
            //rangePoints
            LineItem baseLine = _pane.CurveList[0] as LineItem;
            rangeLine = _pane.AddCurve("selection", rangePoints, Color.Black, SymbolType.None);
            rangeLine.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            rangeLine.IsVisible = true;
            rangeLine.Line.Width = baseLine.Line.Width * 2;
            ReDraw();

        }
        void ReDraw()
        {
            ctrl.Invalidate();
            ctrl.AxisChange();
            Application.DoEvents();
        }
        public void Cleanup()
        {
            foreach (var poly in _graphObjects)
            {
                _pane.GraphObjList.Remove(poly);
            }
            if (rangeLine != null)
            {
                _pane.CurveList.Remove(rangeLine);
                rangeLine.Clear();
            }
            if (rangePoints != null)
            {
                rangePoints.Clear();
            }
            Cursor.Current = Cursors.Default;
            ReDraw();
        }
        ~ActionPoints()
        {
            Cleanup();
        }
    }

}
