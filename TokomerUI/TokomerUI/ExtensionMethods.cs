using System;
using System.Collections.Generic;
using ZedGraph;
using System.Linq;
using CerealPotter;
using CerealPotter.Properties;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using MathNet.Numerics.Statistics;
using System.Reflection;
using System.Threading;
using System.Runtime.Serialization;
using System.IO.Ports;
using System.Management;
public static class ExtensionMethods
{
    static NumberFormat numberFormat = new NumberFormat();
    static string[] curve_names = Settings.Default.curve_names.Split(',');
    public static Color[] colorList = { Color.Red, Color.Blue, Color.Green, Color.Brown, Color.Yellow, Color.Purple, Color.Orange, Color.Pink };
    static Dictionary<Color, Color> ColorList = new Dictionary<Color, Color>();
    static List<Color> usedColors = new List<Color>();
    static KnownColor[] knownColors = (KnownColor[])Enum.GetValues(typeof(KnownColor));

    public static Color DarkColor(this Color baseColor)
    {
        FillColors();
        try
        {
            return ColorList[baseColor];
        }
        catch (Exception)
        {

            
        }
        return baseColor;
    }
    public static void RemoveUsedColor(this Color color)
    {
        FillColors();
        if (usedColors.Contains(color))
        {
            usedColors.Remove(color);
        }
    }

    public static void Set(this ref PointF point, MouseEventArgs e)
    {
        if (e == null) return;
        point.X = e.X;
        point.Y = e.Y;
    }
    public static PointF From(this ref PointF point, MouseEventArgs e)
    {
        point.Set(e);
        return point;
    }
    public static void AddUsedColor(this Color color)
    {
        FillColors();
        if (!usedColors.Contains(color))
        {
            usedColors.Add(color);
            if(!ColorList.ContainsKey(color))
            {
                ColorList.Add(color, color);
            }
        }
    }
    private static void FillColors()
    {
        if (ColorList.Count != 0) return;
        List<string> ColorNames = new List<string>(knownColors.Select(c => c.ToString()));

        foreach (KnownColor known in knownColors.Where(c => !c.ToString().StartsWith("Dark")))
        {
            try
            {
                Color dark = Color.FromKnownColor(knownColors.First(c => c.ToString() == "Dark" + known.ToString()));
                ColorList.Add(Color.FromKnownColor(known), dark);
            }
            catch (Exception)
            {


            }
        }
    }
    public static bool Contains(this RectangleF rec, PointPair pt)
    {
        return rec.Contains((float)pt.X, (float)pt.Y);
    }
    public static RectangleF From(this RectangleF rec, PointPair startPt, PointPair endPt)
    {
        float startX = (float) Math.Min(startPt.X, endPt.X);
        float endX = (float)Math.Max(startPt.X, endPt.X);
        float minY = (float)Math.Min(startPt.Y, endPt.Y);
        float maxY = (float)Math.Max(startPt.Y, endPt.Y);
        rec.X = startX;
        rec.Y = minY;
        rec.Width = endX - startX;
        rec.Height = maxY - minY;
        return rec;
    }
    public static Color SetRandomColor(this ref Color color)
    {

        FillColors();
        Color result = Color.Empty;
        do
        {
            var rand = new Random();
            var r = rand.Next(0, ColorList.Count-1);
            result = ColorList.ElementAt(r).Key;
        } while (usedColors.Contains(result));
        usedColors.Add(result);
        color = result;
        return color;
    }

    public static PointPair Mean(this List<PointPair> points)
    {
        return new PointPair(points.Last().X, points.Select(p => p.Y).Mean());
    }
    public static PointPair StandardDeviation(this List<PointPair> points)
    {
        return new PointPair(points.Last().X, points.Select(p => p.Y).StandardDeviation());
    }
    public static ulong Nanoseconds(this Stopwatch stopWatch)
    {
        return (ulong) 1000000000.0 * (ulong)stopWatch.ElapsedTicks/ (ulong)Stopwatch.Frequency;
    }
    public static double ElapsedDecimal(this Stopwatch stopwatch)
    {
        return (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
    }
    public static int CurveIndex(this string name)
    {
        return Array.IndexOf(curve_names, name);
    }
    public static double CenterX(this PointPair start,PointPair end)
    {
        return (end.X - start.X) / 2 + end.X;
    }
    public static Color GraphColor(this string name)
    {
        return colorList[name.CurveIndex()];
    }
    public static T Clone<T>(T original)
    {
        T newObject = (T)Activator.CreateInstance(original.GetType());

        foreach (var originalProp in original.GetType().GetProperties())
        {
            originalProp.SetValue(newObject, originalProp.GetValue(original));
        }

        return newObject;
    }
    public static void CopyAttributes<T,U>(this T to,U from) where T : class
                                            where U : class
    {
        var parentProperties = from.GetType().GetProperties();
        var childProperties = to.GetType().GetProperties();

        foreach (var parentProperty in parentProperties)
        {
            foreach (var childProperty in childProperties)
            {
                if (parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType && childProperty.SetMethod!=null)
                {
                    
                    
                    
                    if(parentProperty.PropertyType.IsClass )
                    {
                        var FromProperty = parentProperty.GetValue(from);
                        object newObject = Activator.CreateInstance(FromProperty.GetType());

                        foreach (var originalProp in FromProperty.GetType().GetProperties().Where(p=>p.SetMethod!=null ))
                        {
                            originalProp.SetValue(newObject, originalProp.GetValue(FromProperty));
                        }
                        childProperty.SetValue(to, newObject);
                    }
                    else
                    {
                        childProperty.SetValue(to, parentProperty.GetValue(from));
                    }

                    break;
                }
            }
        }
    }
    public static List<CustomGraphControl> GraphControls(this TableLayoutPanel panel)
    {
        List<CustomGraphControl> graphList = new List<CustomGraphControl>();
        for (int i = 0; i < panel.Controls.Count; i++)
        {
            CustomGraphControl graph = panel.Controls[i] as CustomGraphControl;
            if (graph == null) continue;
            graphList.Add(graph);
        }
        return graphList;
    }
    public static CustomGraphControl Graph(this TableLayoutPanel panel, string name)
    {
        if(panel.GraphControls().Any(p=>p.Name == name))
        {
            return panel.GraphControls().First(p => p.Name == name);
        }
        return null;
    }
    public static GraphPane GraphPane(this TableLayoutPanel panel, string name)
    {
        if (panel.Graph(name) == null) return null;
        return panel.Graph(name).GraphPane;
    }
    public static void Add(this Dictionary<string, TriggerPoints> pointsGraphs, PointPair point, string name)
    {
        foreach (var pointGraph in pointsGraphs)
        {
            pointGraph.Value.Add(new PointPair(point), name);
        }
    }
    public static int Count(this Dictionary<string, RollingPointPairList> RollingPoints)
    {
        return RollingPoints.Select(rp => rp.Value.Count).Sum();
    }
    public static void Extend<T>(this List<T> list, List<T> fromList)
    {
        if (fromList.Count <= list.Count) return;
        list.AddRange(new List<T>(fromList.Skip(list.Count)));
    }
    public static void Reset(this Dictionary<string, TriggerPoints> pointsGraphs)
    {
        foreach (var pointGraph in pointsGraphs)
        {
            pointGraph.Value.Reset();
        }
    }
    public static int IndexOf(this IPointList dataPoints, PointPair Pt)
    {
        for (int i = 0; i < dataPoints.Count; i++)
        {
            if (dataPoints[i].X == Pt.X) return i;
        }
        return -1;
    }
    public static int IndexOf(IPointList dataPoints, double X)
    {
        for (int i = 0; i < dataPoints.Count; i++)
        {
            if (dataPoints[i].X ==X) return i;
        }
        return -1;
    }
    public static int IndexOf(this RollingPointPairList dataPoints, double X)
    {
        for (int i = 0; i < dataPoints.Count; i++)
        {
            if (dataPoints[i].X == X) return i;
        }
        return -1;
    }
    public static int IndexOf(this RollingPointPairList dataPoints, PointPair Pt)
    {
        for (int i = 0; i < dataPoints.Count; i++)
        {
            if (dataPoints[i].X == Pt.X) return i;
        }
        return -1;
    }
    public static List<PointPair> ToList(this IPointList points)
    {
        List<PointPair> list = new List<PointPair>();
        for (int i = 0; i < points.Count; i++)
        {
            list.Add(points[i]);
        }
        return list;
    }
    public static PointPairList GetRange(this IPointList points, int from, int count )
    {
        PointPairList list = new PointPairList();
        for (int i = 0; i < points.Count; i++)
        {
            list.Add(points[i]);
        }
        return list;
    }
    public static void RemoveRange<T>(this  T points, int from, int count) where T : IPointList, IPointListEdit
    {
        MethodInfo RemoveAt= points.GetType().GetMethod("RemoveAt");

        for (int i = from + count>points.Count-1?points.Count-1: from + count; i >=from; i--)
        {
            RemoveAt.Invoke(points, new object[] { i });
        }
    }

    public static List<PointPair> ToList(this RollingPointPairList pointPairList)
    {
        List<PointPair> list = new List<PointPair>();
        for (int i = 0; i < pointPairList.Count; i++)
        {
            list.Add(pointPairList[i]);
        }
        return list;
    }

    public static string FormattedNumber(this double number)
    {
        return numberFormat.ToString((decimal)number);
    }

    public static string FormattedNumber(this float number)
    {
        return numberFormat.ToString((decimal)number);
    }
    //public static T GetData<T>(this T obj, SerializationInfo info)
    //{

    //    return (T)info.GetValue(obj.GetType(), _rollingPointPairList.GetType()); ;
    //}
    public static Type GetUnderlyingType(this MemberInfo member)
    {
        switch (member.MemberType)
        {
            case MemberTypes.Event:
                return ((EventInfo)member).EventHandlerType;
            case MemberTypes.Field:
                return ((FieldInfo)member).FieldType;
            case MemberTypes.Method:
                return ((MethodInfo)member).ReturnType;
            case MemberTypes.Property:
                return ((PropertyInfo)member).PropertyType;
            default:
                throw new ArgumentException
                (
                 "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                );
        }
    }
    public static bool GetValue<T,O>(this T from,  MemberInfo member, out O value)
    {
        value = (O)from.GetType().GetField(member.Name).GetValue(from);
        return value!=null;
    }
    public static List<string> controlList=new List<string>();
    public static Control FindTag(this Control.ControlCollection controls, string tag)
    {
        try
        {
            foreach (Control c in controls)
            {
                controlList.Add(c.Name);
                if (c.Tag != null)
                {
                    var values = c.Tag.ToString().Split(',');
                    foreach (string ctrlTag in values)
                    {
                        if (ctrlTag.StartsWith(tag))
                        {
                            return c;
                        }
                    }
                    
                }
                if (c.HasChildren || (c.Controls != null && c.Controls.Count > 0))
                {
                    var res= c.Controls.FindTag(tag); //Recursively check all children controls as well; ie groupboxes or tabpages
                    if (res != null) return res;
                }
            }
        }
        catch (Exception e)
        {

            Console.WriteLine(e);
        }

        return null;
    }
    public static Dictionary<string, Control> FindControlTags(this Control.ControlCollection controls, ref Dictionary<string,Control> list)
    {
        if(list==null)
        {
            list = new Dictionary<string, Control>();
        }
        try
        {
            foreach (Control c in controls)
            {
                if (c.Tag != null)
                {
                    
                    var values = c.Tag.ToString().Split(',');
                    foreach (string ctrlTag in values)
                    {
                        list.Add(ctrlTag, c);
                    }

                }
                if (c.HasChildren || (c.Controls != null && c.Controls.Count > 0))
                {
                    c.Controls.FindControlTags(ref list); //Recursively check all children controls as well; ie groupboxes or tabpages
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(null, $"Error {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        return list;
    }

    static public string GetDevice(this SerialPort serialPort)
    {
        using (var searcher = new ManagementObjectSearcher
                ("SELECT * FROM WIN32_SerialPort"))
        {
            string[] portnames = SerialPort.GetPortNames();

            var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
            var tList = ports.Where(port => port["DeviceID"].ToString() == serialPort.PortName).Select(p=>p["PNPDeviceId"].ToString()).ToList();
            if (tList.Count > 0) return tList[0];
        }
        return "";
    }


}
