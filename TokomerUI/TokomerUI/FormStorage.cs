
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ZedGraph;
using CerealPotter.Properties;
using System.Runtime.Serialization;
using System.Drawing;
using ProtoBuf;

namespace CerealPotter
{
    public partial class Form1 
    {
        [Serializable]
        [TypeConverter(typeof(FormStorage))]
        [ProtoContract]
        public class FormStorage : ISerializable
        {

            //private Dictionary<string, AveragePoints> graphLists = new Dictionary<string, AveragePoints>();

            [ProtoMember(1)] 
            public List<AveragePoints> graphLists = new List<AveragePoints>();
            public List<CustomGraphControl> GraphControls = new List<CustomGraphControl>();
            [ProtoMember(2)]
            public Dictionary<string, PointPairList> measureGraphs = new Dictionary<string, PointPairList>();
            public Dictionary<string, TriggerPoints> triggerGraphs = new Dictionary<string, TriggerPoints>();
            [ProtoMember(3)]
            public string[] curve_names = Settings.Default.curve_names.Split(',');
            [ProtoMember(4)]
            public string[] curve_titles = Settings.Default.curves_titles.Split(',');
            [ProtoMember(5)]
            public float[] curve_multipliers = Settings.Default.curve_multipliers.Split(',').Select(v => float.Parse(v)).ToArray();
            [ProtoMember(6)]
            public bool thresholdMet = false;
            private GraphMover.MoveCompleteEventArgs moveCompleteEvent;
            public void Invalidate(IPointList pointList)
            {
                
                AveragePoints pts = (AveragePoints)pointList;
                if (pointList == null) return;
                Invalidate(pts.FullName);
            }
            public void Invalidate(string curveFullName)
            {
                try
                {
                    var control = GraphControls.Select(gc => new { gc, gc.GraphPane.CurveList })?
                    .SelectMany(e => e.CurveList?.Select(cl => new { e.gc, cl.Points }))?
                    .First(tp => ((AveragePoints)tp.Points)?.FullName == curveFullName)?.gc;
                    control?.AxisChange();
                    control?.Invalidate();
                }
                catch(Exception)
                {

                }
            }

            public FormStorage()
            {

            }
            public void Reset()
            {
                //foreach (var control in tableGraphPanel.Controls)
                //{
                //    var graphControl = control as CustomGraphControl;
                //    if (graphControl == null) continue;
                //    graphControl.GraphPane.CurveList.Clear();
                //}
                foreach (var graph in graphLists)
                {
                    graph.Reset();
                }
                graphLists.Clear();
                foreach (var trig in triggerGraphs)
                {
                    trig.Value.Reset();
                }
                triggerGraphs.Clear();
                foreach (var measure in measureGraphs)
                {
                    measure.Value.Clear();
                }
                measureGraphs.Clear();
                moveCompleteEvent = null;
                foreach (var ctrl in GraphControls)
                {
                    while(ctrl.GraphPane.CurveList.Count>0)
                    {
                        ctrl.GraphPane.CurveList.RemoveAt(0);
                    }
                    while(ctrl.GraphPane.GraphObjList.Count>0)
                    {
                        ctrl.GraphPane.GraphObjList.RemoveAt(0);
                    }
                }
            }
            public void CutPoints(GraphCutter cutter)
            {
                if (!cutter.HasSelection) return;
                foreach (var range in cutter.SelectedRanges)
                {
                    for (int i = range.Value.selectedPoints.Count-1; i >=0 ; i--)
                    {
                        range.Key.RemoveAt(range.Value.selectedPoints[i]);
                    }
                }
                cutter.ClearSelection();

            }
            public int Count
            {
                get
                {
                    return graphLists.Select(g => g.Count).Sum();
                }
            }
            public AveragePoints PointsAt(string key)
            {
                return PointsAt("", key);
            }
            public AveragePoints PointsAt(ZedGraph.Label item)
            {
                return PointsAt(item.Text);
            }
            public AveragePoints PointsAt(LineItem item)
            {
                return PointsAt(item.Label);
            }

            public AveragePoints PointsAt(string prefix, string key)
            {
                foreach (var pts in graphLists)
                {
                    if (pts.Prefix == prefix && pts.Name == key)
                    {
                        return pts;
                    }
                }
                // not found. See if it is a composite key
                foreach (var pts in graphLists)
                {
                    if (pts.FullName == key && prefix=="")
                    {
                        return pts;
                    }
                }
                return null;
            }
            public void SetPoints(string prefix, string key, AveragePoints newPoints)
            {
                for (int i = 0; i < graphLists.Count; i++)
                {
                    if (graphLists[i].Prefix == prefix && graphLists[i].Name == key || graphLists[i].FullName == key)
                    {
                        graphLists[i].Input = newPoints.Input;
                        for (int j = 0; j < graphLists[i].Pane.CurveList.Count; j++)
                        {
                            if (j < newPoints.Pane.CurveList.Count)
                            {
                                graphLists[i].Pane.CurveList[j].Points = newPoints.Pane.CurveList[j].Points;
                            }
                        }
                    }
                }
            }
            public bool Contains(string prefix, string key)
            {
                return graphLists.Any(g => g.Prefix == prefix && g.Name == key);
            }

            public void AddGraph(AveragePoints averagePoints)
            {
                graphLists.Add(averagePoints);
            }

            public bool MeetsThreshold(string key, double v)
            {
                if (!thresholdMet && key == curve_names[0] && v > (double)Settings.Default.Threshold_mA)
                {
                    thresholdMet = true;
                }
                return thresholdMet;
            }
            public LineItem Curve(string key)
            {
                return Curve("", key);
            }
            public LineItem Curve(string prefix, string key)
            {
                return PointsAt(prefix, key)?.Curve;
            }
            public GraphPane GraphPane(string key)
            {
                return GraphPane("", key);
            }
            public GraphPane GraphPane(string prefix, string key)
            {
                return PointsAt(prefix, key)?.Pane;
            }
            public void Add(string key, string prefix, double timeStamp, double v, GraphPane pane, bool averaging = true)
            {
                AveragePoints pp = PointsAt(prefix, key);
                if (pp == null)
                {
                    Color newColor = new Color();
                    var matchingGraph = graphLists?.Where(gl => gl.Prefix == prefix);
                    if (matchingGraph.Any())
                    {
                        newColor = matchingGraph.Select(mg => mg.Color).First();
                    }
                    else
                    {
                        newColor.SetRandomColor();
                    }
   
                    pp = new AveragePoints(key, prefix, pane, newColor)
                    {
                        Averaging = averaging
                    };
                    graphLists.Add(pp);
                }
                pp.Add(timeStamp, v);
            }
            public void SetCurveColor(Color newcolor)
            {
                if (!HasSelectedGraph()) return;
                
                foreach (var grahp in graphLists.Where(g=>g.Prefix == SelectedGraph().Prefix ))
                {
                    grahp.Color.RemoveUsedColor();
                    grahp.Color = newcolor;
                    Invalidate(grahp.FullName);
                }
                newcolor.AddUsedColor();
            }
            public AveragePoints SelectedGraph()
            {
                foreach (var graph in graphLists)
                {
                    if(graph.Selected)
                    {
                        return graph;
                    }
                }
                return null;
            }
            public bool HasSelectedGraph()
            {
                return SelectedGraph() != null;
            }
            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                //public List<AveragePoints> graphLists = new List<AveragePoints>();
                //public Dictionary<string, string> comparedGraphLists = new Dictionary<string, string>();
                //public Dictionary<string, PointPairList> measureGraphs = new Dictionary<string, PointPairList>();
                //public Dictionary<string, TriggerPoints> triggerGraphs = new Dictionary<string, TriggerPoints>();
                //public string[] curve_names = Settings.Default.curve_names.Split(',');
                //public string[] curve_titles = Settings.Default.curves_titles.Split(',');
                //public float[] curve_multipliers = Settings.Default.curve_multipliers.Split(',').Select(v => float.Parse(v)).ToArray();
                //public bool thresholdMet = false;
                if (info == null)
                    throw new System.ArgumentNullException("info");
                info.AddValue("graphLists", graphLists);
                info.AddValue("measureGraphs", measureGraphs);
                info.AddValue("triggerGraphs", triggerGraphs);
                info.AddValue("curve_names", curve_names);
                info.AddValue("curve_titles", curve_titles);
                info.AddValue("curve_multipliers", curve_multipliers);
                info.AddValue("thresholdMet", thresholdMet);
                
            }
            public FormStorage(SerializationInfo info, StreamingContext context)
            {
                try
                {
                    graphLists = (List<AveragePoints>)info.GetValue("graphLists", graphLists.GetType());
                    measureGraphs = (Dictionary<string, PointPairList>)info.GetValue("measureGraphs", measureGraphs.GetType());
                    triggerGraphs = (Dictionary<string, TriggerPoints>)info.GetValue("triggerGraphs", triggerGraphs.GetType());
                    curve_names = (string[])info.GetValue("curve_names", curve_names.GetType());
                    curve_titles = (string[])info.GetValue("curve_titles", curve_titles.GetType());
                    curve_multipliers = (float[])info.GetValue("curve_multipliers", curve_multipliers.GetType());
                    thresholdMet = (bool)info.GetValue("thresholdMet", thresholdMet.GetType());
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message} ", "Data Read Error", MessageBoxButtons.OK);
                    throw e;
                }
            }

            public Dictionary<string, List<string>> getCurves()
            {
                Dictionary<string, List<string>> list = new Dictionary<string, List<string>>();
                foreach (var item in graphLists)
                {
                    if (!list.ContainsKey(item.Name))
                    {
                        list.Add(item.Name, new List<string>());
                    }
                    foreach (var name in item.Pane.CurveList.Select(c => c.Label.Text).Distinct())
                    {
                        if (!list[item.Name].Contains(name) && item.Name != name)
                        {
                            list[item.Name].Add(name);
                        }
                    }
                }
                return list;
            }
            public void UndoOffsetCurves()
            {
                if (moveCompleteEvent != null)
                {
                    OffsetCurves(moveCompleteEvent.Prefix, moveCompleteEvent.Name, moveCompleteEvent.OffsetX * -1, moveCompleteEvent.OffsetY * -1);
                }

            }
            public void OffsetCurves(string prefix, string Yname, double offsetX, double offsetY = 0)
            {
                OffsetCurves(new GraphMover.MoveCompleteEventArgs(offsetX, offsetY, prefix, Yname));
            }
            public void OffsetCurves(GraphMover.MoveCompleteEventArgs e)
            {
                moveCompleteEvent = e;
                if (moveCompleteEvent.OffsetX == 0 && moveCompleteEvent.OffsetY == 0) return;
                for (int i = 0; i < graphLists.Count; i++)
                {
                    if (moveCompleteEvent.Fullname == graphLists[i].FullName )
                    {
                        // Don't move the already moved curve, and don't allow moving 
                        // the base curve. 
                        continue;
                    }
                    for (int j = 0; j < graphLists[i].Curve.Points.Count; j++)
                    {
                        graphLists[i][j].X += moveCompleteEvent.OffsetX;
                        if (moveCompleteEvent.Name == graphLists[i].Name)
                        {
                            graphLists[i][j].Y += moveCompleteEvent.OffsetY;
                        }
                    }
                    for (int j = 0; j < graphLists[i].Input.Count; j++)
                    {
                        graphLists[i].Input[j].X += moveCompleteEvent.OffsetX;
                        if (moveCompleteEvent.Name == graphLists[i].Name)
                        {
                            graphLists[i].Input[j].Y += moveCompleteEvent.OffsetY;
                        }
                    }
                    Invalidate(graphLists[i]);
                }
            }
            public void SetBaseTitle(string text)
            {
                foreach (var pts in graphLists.Where(g=>g.Prefix == ""))
                {
                    pts.Prefix = text;
                    
                    Invalidate(pts);
                }
            }
        };
       
    }
}
