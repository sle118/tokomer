using ZedGraph;
using System.Windows.Forms;

namespace CerealPotter
{
    public class CustomGraphControl : ZedGraphControl
    {
        private System.Diagnostics.Stopwatch duration = new System.Diagnostics.Stopwatch();
        
        public long DrawingTime => duration.ElapsedMilliseconds;
        private long _MaxDrawTime;
        public long MaxDrawTime => _MaxDrawTime;

        public override void AxisChange()
        {
            if(!duration.IsRunning) duration.Restart();
            base.AxisChange();
        }
        
        public new void Invalidate()
        {
            if (!duration.IsRunning) duration.Restart();
            base.Invalidate();
        }
        public void UpdateAxis()
        {
            if (!duration.IsRunning) duration.Restart();
            base.AxisChange();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            duration.Stop();
            if (_MaxDrawTime < duration.ElapsedMilliseconds)
            {
                _MaxDrawTime = duration.ElapsedMilliseconds;
            }
        }
    }
}
