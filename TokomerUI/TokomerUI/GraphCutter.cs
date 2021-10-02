using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ZedGraph;
namespace CerealPotter
{
    public class GraphCutter
    {
        public class GraphSelection
        {
            public PointPairList selectedPoints = new PointPairList();
            public LineItem lineItem;
        }
        private PointPair startPt = new PointPair();
        private PointPair endPt = new PointPair(); // the mouse location at the start of the dragging
        public bool IsSelectingArea = false; // true means dragging is underway
        public Keys ModifierKeys = Keys.Shift;
        public Keys AppendModifierKeys = Keys.Control;
        public MouseButtons MouseButton = MouseButtons.Left;
        private GraphPane _pane;
        private PointF pt = new PointF();
        public string Name = "";
        public string Prefix = "";
        ZedGraphControl control;

        public Dictionary<AveragePoints, GraphSelection> SelectedRanges = new Dictionary<AveragePoints, GraphSelection>();

        public bool HasSelection => SelectedRanges.Count > 0;
        public void ClearSelection()
        {
            if (SelectedRanges.Count == 0) return;
            foreach (var rp in SelectedRanges)
            {
                GraphSelection graphSelection = rp.Value;
                _pane.CurveList.Remove(graphSelection.lineItem);
                graphSelection.selectedPoints.Clear();
            }
            SelectedRanges.Clear();
            control.Invalidate();
        }

        public void Selection_SelectionChangedEvent(object sender, EventArgs e)
        {
            if (IsSelectingArea)
            {
                Selection selection = sender as Selection;
                if (selection == null) return;

                IsSelectingArea = false;
                foreach (var curveItem in selection)
                {
                    AveragePoints points = curveItem.Points as AveragePoints;
                    if (points == null) continue;
                    DrawSelection(points);
                }
            }
        }
        private void DrawSelection(AveragePoints points)
        {
            //rangePoints
            RectangleF rect = new RectangleF().From(startPt, endPt);
            if(points.Selected)
            {
                points.Unselect();
            }
            if (!SelectedRanges.ContainsKey(points))
            {
                SelectedRanges.Add(points, new GraphSelection());
            }
            GraphSelection graphSelection = SelectedRanges[points];
            for (int i = 0; i < points.Count; i++)
            {
                if (rect.Contains(points[i]))
                {
                    graphSelection.selectedPoints.Add(points[i]);
                }
            }
            LineItem baseLine = _pane.CurveList[0] as LineItem;
            if (SelectedRanges == null || SelectedRanges.Count == 0) return;
            graphSelection.lineItem = _pane.AddCurve("selection", graphSelection.selectedPoints, Color.Black, SymbolType.Circle);
            graphSelection.lineItem.Label.IsVisible = false;
            graphSelection.lineItem.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            graphSelection.lineItem.IsVisible = true;
            graphSelection.lineItem.Line.Width = baseLine.Line.Width * 3;
            control.Invalidate();
        }
        public class CutCompletedEventArgs
        {
            public Dictionary<AveragePoints, GraphSelection> SelectedRanges;
            public CutCompletedEventArgs(Dictionary<AveragePoints, GraphSelection> selectedRanges)
            {
                SelectedRanges = selectedRanges;
            }

        }
        // Declare the delegate (if using non-generic pattern).
        public delegate void CutCompletedEventHandler(object sender, CutCompletedEventArgs e);

        // Declare the event.
        public event CutCompletedEventHandler CutCompleted;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseCutCompleted()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            CutCompleted?.Invoke(this, new CutCompletedEventArgs(SelectedRanges));
        }


        public GraphCutter()
        {

        }

        public bool MouseDownEvent(ZedGraph.ZedGraphControl sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender == null) return false;
            // left mouse + alt key causes point dragging
            if (Control.ModifierKeys.HasFlag(ModifierKeys) && e.Button == MouseButton)
            {
                _pane = sender.GraphPane;
                if (!Control.ModifierKeys.HasFlag(AppendModifierKeys))
                {
                    ClearSelection();
                }
                _pane.ReverseTransform(pt.From(e), out startPt.X, out startPt.Y);
                // indicate that dragging is underway
                IsSelectingArea = true;
                control = sender;
            }
            else if (HasSelection)
            {
                ClearSelection();
            }
            return false;
        }

        public bool MouseMoveEvent(ZedGraph.ZedGraphControl graphControl, System.Windows.Forms.MouseEventArgs e)
        {
            // if dragging is active, then handle it
            if (IsSelectingArea)
            {
                _pane.ReverseTransform(pt.From(e), out endPt.X, out endPt.Y);
            }
            return false;
        }


    }
}
