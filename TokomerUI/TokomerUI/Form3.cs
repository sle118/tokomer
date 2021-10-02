using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace CerealPotter
{
    public partial class formOffsetGraphs : Form
    {
        private PointF startPt; // the mouse location at the start of the dragging
        private double startX, startY, startY2, startX2; // the scale values at the starting mouse location
        private bool isDragPoint = false; // true means dragging is underway
        private CurveItem dragCurve; // the CurveItem that contains the point being dragged
        private int dragIndex; // the index location for the dragged point
        private PointPair startPair;
        public double OffsetX = 0;
        public double OffsetY = 0;
        

        public formOffsetGraphs()
        {
            InitializeComponent();
        }

        private bool customGraphControl_MouseDownEvent(ZedGraph.ZedGraphControl sender, MouseEventArgs e)
        {
            // left mouse + alt key causes point dragging
            if (ModifierKeys == (Keys.Control | Keys.Alt) || ModifierKeys== Keys.Alt)
            {
                GraphPane myPane = zedgraphControl1.GraphPane;
                PointF mousePt = new PointF(e.X, e.Y);

                // find the point closest to the mouse click
                if (myPane.FindNearestPoint(mousePt, out dragCurve, out dragIndex))
                {
                    AveragePoints avgPts = (AveragePoints)dragCurve.Points;
                    if (avgPts != null && avgPts.Prefix == "") return false;
                        
                    
                    // indicate that dragging is underway
                    isDragPoint = true;

                    // save the point data
                    startPt = mousePt;
                    startPair = new PointPair( dragCurve.Points[dragIndex]);

                    // calculate the scale values that correspond to the mouse point
                    myPane.ReverseTransform(mousePt, out startX, out startX2, out startY, out startY2);
                }
                return true;
            }
            return false;
        }

        private bool customGraphControl_MouseMoveEvent(ZedGraph.ZedGraphControl sender, MouseEventArgs e)
        {
            // if dragging is active, then handle it
            if (isDragPoint)
            {
                // move the point
                double curX, curX2,curY, curY2;
                PointF mousePt = new PointF(e.X, e.Y);

                // calculate the scale values at the current mouse location
                zedgraphControl1.GraphPane.ReverseTransform(mousePt, out curX,out curX2, out curY, out curY2);

                // the new location for the point is based on the difference between the starting
                // and ending mouse location
                PointPair newPt = new PointPair(startPair.X + curX - startX, startPair.Y + curY - startY);
                double offset = newPt.X - dragCurve[dragIndex].X;
                double offsetY = 0;
                OffsetX += offset; // Keep track of the whole movement
                if (ModifierKeys == (Keys.Control | Keys.Alt))
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
                zedgraphControl1.Refresh();
                return true;
            }
            return false;
        }

        private bool customGraphControl_MouseUpEvent(ZedGraph.ZedGraphControl sender, MouseEventArgs e)
        {
            // finalize the move -- dragging is done
            if (isDragPoint)
            {
                isDragPoint = false;
                return true;
            }
            return false;
        }
    }
}
