using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CerealPotter.Properties;
using MathNet;
using MathNet.Numerics.Statistics;
using ZedGraph;
public class ZScoreOutput
{
    public List<PointPair> input;
    public List<PointPair> signals;
    public List<PointPair> avgFilter;
    public List<PointPair> filtered_stddev;
}

public class ZScore
{
    private List<PointPair> input = new List<PointPair>();
    private int lag => (int)(Settings.Default.PeakDetectionLag_ms / 1000 * Settings.Default.ReadsPerSec);
    private double threshold => (double)(Settings.Default.PeakDetectionThreshold);
    private double influence => (double)(Settings.Default.PeakDetectionSignalInfluence);
    private PointPairList filteredY = new PointPairList();
    private PointPairList avgFilter = new PointPairList();
    private PointPairList stdFilter = new PointPairList();
    public PointPairList signals = new PointPairList();
    private ToolStripProgressBar  progressBar;

    private GraphPane _pane;

    /*
     * 
    lag: the lag parameter determines how much your data will be smoothed and how adaptive the algorithm is to changes in the long-term average of the data. The more stationary your data is, the more lags you should include (this should improve the robustness of the algorithm). If your data contains time-varying trends, you should consider how quickly you want the algorithm to adapt to these trends. I.e., if you put lag at 10, it takes 10 'periods' before the algorithm's treshold is adjusted to any systematic changes in the long-term average. So choose the lag parameter based on the trending behavior of your data and how adaptive you want the algorithm to be.

    influence: this parameter determines the influence of signals on the algorithm's detection threshold. If put at 0, signals have no influence on the threshold, such that future signals are detected based on a threshold that is calculated with a mean and standard deviation that is not influenced by past signals. If put at 0.5, signals have half the influence of normal data points. Another way to think about this is that if you put the influence at 0, you implicitly assume stationarity (i.e. no matter how many signals there are, you always expect the time series to return to the same average over the long term). If this is not the case, you should put the influence parameter somewhere between 0 and 1, depending on the extent to which signals can systematically influence the time-varying trend of the data. E.g., if signals lead to a structural break of the long-term average of the time series, the influence parameter should be put high (close to 1) so the threshold can react to structural breaks quickly.
    
    threshold: the threshold parameter is the number of standard deviations from the moving mean above which the algorithm will classify a new datapoint as being a signal. For example, if a new datapoint is 4.0 standard deviations above the moving mean and the threshold parameter is set as 3.5, the algorithm will identify the datapoint as a signal. This parameter should be set based on how many signals you expect. For example, if your data is normally distributed, a threshold (or: z-score) of 3.5 corresponds to a signaling probability of 0.00047 (from this table), which implies that you expect a signal once every 2128 datapoints (1/0.00047). The threshold therefore directly influences how sensitive the algorithm is and thereby also determines how often the algorithm signals. Examine your own data and choose a sensible threshold that makes the algorithm signal when you want it to (some trial-and-error might be needed here to get to a good threshold for your purpose).
     */
    public ZScore(GraphPane pane, ToolStripProgressBar pBar)
    {
        _pane = pane;
        LineItem avgCurve = _pane.AddCurve("Average", avgFilter, Color.LightGray, SymbolType.None);
        avgCurve.IsVisible = true;
        avgCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        avgCurve.IsY2Axis = false;


        LineItem stdCurve = _pane.AddCurve("Std Deviation", stdFilter, Color.LightGray, SymbolType.None);
        stdCurve.IsVisible = true;
        stdCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        stdCurve.IsY2Axis = false;

        LineItem filteredCurve = _pane.AddCurve("Filtered", filteredY, Color.LightGray, SymbolType.None);
        filteredCurve.IsVisible = true;
        filteredCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Solid;
        filteredCurve.IsY2Axis = false;

        LineItem signalCurve = _pane.AddCurve("Edges", signals, Color.Blue, SymbolType.None);
        signalCurve.IsVisible = true;
        signalCurve.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
        //signalCurve.IsY2Axis = true;
        signalCurve.Line.Fill = new Fill(Color.White, Color.Blue, 90.0F);


        //pane.Y2Axis.IsVisible = true;
        //pane.Y2Axis.Scale.Min = -1;
        //pane.Y2Axis.Scale.Max = 1;
        //pane.Y2Axis.Scale.MinAuto = false;
        //pane.Y2Axis.Scale.MaxAuto = false;
        //pane.Y2Axis.Scale.MajorStep = 1;
        progressBar = pBar;

    }
    public PointPair this[int key]
    {
        get => input[key];
    }
    public int Count
    {
        get => input.Count;
    }
    public void AddRange(List<PointPair> point)
    {
        if (progressBar != null)
        {
            progressBar.Visible = true;
            progressBar.Maximum = point.Count;
            progressBar.Minimum = 0;
            progressBar.Value = 0;
            progressBar.Visible = true;

        }
        for (int i = 0; i < point.Count; i++)
        {
            Add(point[i]);
            //if(i/point.Count % 2==0)
            //{
            //    Application.DoEvents();
            //}
            if (progressBar != null && i % (int)Settings.Default.ReadsPerSec == 0)
            {
                progressBar.Value = i;
                Application.DoEvents();
            }
        }
        if (progressBar != null) progressBar.Visible = false;
    }
    public void Add(PointPair point)
    {
        input.Add(new PointPair(point));
        filteredY.Add(new PointPair(point));
        if (input.Count == lag)
        {
            avgFilter.Extend(input);
            stdFilter.Extend(input);
            avgFilter[lag - 1] = input.Mean();
            stdFilter[lag - 1] = input.StandardDeviation();
        }
        else if (input.Count > lag)
        {
            int i = input.Count - 1;
            if (Math.Abs(input[i].Y - avgFilter[i - 1].Y) > threshold * stdFilter[i - 1].Y)
            {
                // overwrite the filtered value
                filteredY[i].Y = influence * input[i].Y + (1 - influence) * filteredY[i - 1].Y;
            }
            // Update rolling average and deviation
            var slidingWindow = new List<PointPair>(filteredY).Skip(i - lag).Take(lag + 1).ToList();
            avgFilter.Add(slidingWindow.Mean());
            stdFilter.Add(slidingWindow.StandardDeviation());
        }
    }
    public double Average(int from, int to)
    {
        return input.Where(z => z.X >= input[from].X && z.X <= input[to].X).Select(p => p.Y).Average();
    }
    public double Span(int from, int to)
    {
        return input[to].X - input[from].X;
    }

    public ZScoreOutput GetScores()
    {
        signals.Extend(input);
        // init variables!
        for (int i = 0; i < input.Count; i++)
        {
            if (i < lag)
            {
                signals[i].Y = 0;
                continue;
            }
            if (Math.Abs(input[i].Y - avgFilter[i - 1].Y) > threshold * stdFilter[i - 1].Y)
            {
                signals[i].Y = (input[i].Y > avgFilter[i - 1].Y) ? input[i].Y: input[i].Y*-1;
            }
            else
            {
                signals[i].Y = 0;
            }
        }
        _pane.AxisChange();
        // Copy to convenience class 
        var result = new ZScoreOutput
        {
            input = input,
            avgFilter = avgFilter,
            signals = signals,
            filtered_stddev = stdFilter
        };
        return result;
    }
    public void MoveTo(int index)
    {
        input = input.Skip(index - lag).ToList();
        //avgFilter = avgFilter.Skip(index-lag).ToList();
        //stdFilter = stdFilter.Skip(index - lag).ToList();
    }

    public void Clear()
    {
        input.Clear();
        filteredY.Clear();
        avgFilter.Clear();
        stdFilter.Clear();
        signals.Clear();
    }
}