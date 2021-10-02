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
    public class GraphMover
    {
        private PointF startPt; // the mouse location at the start of the dragging
        private double startX, startY, startY2, startX2; // the scale values at the starting mouse location
        private bool isDragPoint = false; // true means dragging is underway
        private CurveItem dragCurve; // the CurveItem that contains the point being dragged
        private int dragIndex; // the index location for the dragged point
        private PointPair startPair;
        public double OffsetX = 0;
        public double OffsetY = 0;
        public string Name= "";
        public string Prefix = "";
        public class MoveCompleteEventArgs
        {
            public double OffsetX = 0;
            public double OffsetY = 0;
            public string Name = "";
            public string Prefix = "";
            public string Fullname => Prefix + Name;
            public MoveCompleteEventArgs(double offsetX, double offsetY, string prefix,string name) {
                OffsetX = offsetX;
                OffsetY= offsetY;
                Name=name;
                Prefix = prefix;
            }
        
        }
        // Declare the delegate (if using non-generic pattern).
        public delegate void MoveCompletedEventHandler(object sender, MoveCompleteEventArgs e);

        // Declare the event.
        public event MoveCompletedEventHandler MoveCompleted;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        protected virtual void RaiseMoveCompleted()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            MoveCompleted?.Invoke(this, new MoveCompleteEventArgs(OffsetX,OffsetY,Prefix,Name));
        }


        public GraphMover()
        {

        }
  
        public bool MouseDownEvent(ZedGraph.ZedGraphControl sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender == null) return false;
            // left mouse + alt key causes point dragging
            if (e.Button == MouseButtons.Left && Control.ModifierKeys.HasFlag(Keys.Alt) )
            {
                OffsetX = 0;
                OffsetY = 0;
                GraphPane myPane = sender.GraphPane;
                PointF mousePt = new PointF(e.X, e.Y);
                // find the point closest to the mouse click
                if (myPane.FindNearestPoint(mousePt, out dragCurve, out dragIndex))
                {
                    AveragePoints averagePointsClass = (AveragePoints)dragCurve.Points;
                    if (averagePointsClass ==null || myPane.CurveList.Count<=1 ) return false;

                    Name = averagePointsClass.Name;
                    Prefix= averagePointsClass.Prefix;
                    // indicate that dragging is underway
                    isDragPoint = true;

                    // save the point data
                    startPt = mousePt;
                    startPair = new PointPair(dragCurve.Points[dragIndex]);

                    // calculate the scale values that correspond to the mouse point
                    myPane.ReverseTransform(mousePt, out startX, out startX2, out startY, out startY2);
                }
                return true;
            }
            return false;
        }

        public bool MouseMoveEvent(ZedGraph.ZedGraphControl graphControl, System.Windows.Forms.MouseEventArgs e)
        {
            // if dragging is active, then handle it
            if (isDragPoint)
            {
                // move the point
                double curX, curX2, curY, curY2;
                PointF mousePt = new PointF(e.X, e.Y);

                // calculate the scale values at the current mouse location
                graphControl.GraphPane.ReverseTransform(mousePt, out curX, out curX2, out curY, out curY2);

                // the new location for the point is based on the difference between the starting
                // and ending mouse location
                PointPair newPt = new PointPair(startPair.X + curX - startX, startPair.Y + curY - startY);
                double offset = newPt.X - dragCurve[dragIndex].X;
                double offsetY = 0;
                OffsetX += offset; // Keep track of the whole movement
                if (Control.ModifierKeys.HasFlag(Keys.Control))
                {
                    offsetY = newPt.Y - dragCurve[dragIndex].Y;
                    OffsetY += offsetY; // Keep track of the whole movement
                }
                // set the new point
                for (int i = 0; i < dragCurve.Points.Count; i++)
                {
                    dragCurve[i].X += offset;
                    dragCurve[i].Y += offsetY;
                }
                graphControl.Refresh();
                return true;
            }
            return false;
        }

        public bool MouseUpEvent(ZedGraph.ZedGraphControl sender, System.Windows.Forms.MouseEventArgs e)
        {
            // finalize the move -- dragging is done
            if (isDragPoint)
            {
                isDragPoint = false;
                RaiseMoveCompleted();
                return true;
            }
            return false;
        }

    }
}
