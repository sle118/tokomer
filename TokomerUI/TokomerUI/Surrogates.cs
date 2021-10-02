using ProtoBuf;
using ProtoBuf.Meta;
using ProtoBuf.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace CerealPotter
{
    [ProtoContract]
    public class PointPairSurrogate
    {
        [ProtoMember(1)]
        public double X { get; set; }
        [ProtoMember(2)]
        public double Y { get; set; }
        [ProtoMember(3)] 
        public double Z { get; set; }
        
        public static implicit operator PointPairSurrogate(PointPair value)
        {
            if (value == null) return null;
            return new PointPairSurrogate{ X=value.X, Y=value.Y, Z=value.Z};
        }

        public static implicit operator PointPair(PointPairSurrogate value)
        {
            if (value == null) return null;
            return new PointPair(value.X, value.Y, value.Z);
        }
    }
    public class RollingPointPairListSurrogate
    {
        [ProtoMember(1)]
        public List<PointPair> pointPairs;
        [ProtoMember(2)]
        public int Capacity;

        public static implicit operator RollingPointPairListSurrogate(RollingPointPairList values)
        {
            var pointPairs = new List<PointPair>();
            RollingPointPairListSurrogate surrogate = new RollingPointPairListSurrogate() { pointPairs=pointPairs,Capacity=values.Capacity };
            for (int i = 0; i < values.Count; i++)
            {
                pointPairs.Add(values[i]);
            }
            
            return surrogate;
        }

        public static implicit operator ZedGraph.RollingPointPairList(RollingPointPairListSurrogate value)
        {
            RollingPointPairList result = new RollingPointPairList(value.Capacity);
            for (int i = 0; i < value.pointPairs.Count; i++)
            {
                result.Add(value.pointPairs[i]);
            }

            return result;
        }
    }
    public class PointPairListSurrogate
    {
        [ProtoMember(1)]
        public List<PointPair> pointPairs;

        public static implicit operator PointPairListSurrogate(PointPairList values)
        {
            var pointPairs = new List<PointPair>();
            PointPairListSurrogate surrogate = new PointPairListSurrogate() { pointPairs = pointPairs };
            for (int i = 0; i < values.Count; i++)
            {
                pointPairs.Add(values[i]);
            }

            return surrogate;
        }

        public static implicit operator ZedGraph.PointPairList(PointPairListSurrogate value)
        {
            PointPairList result = new PointPairList();
            for (int i = 0; i < value.pointPairs.Count; i++)
            {
                result.Add(value.pointPairs[i]);
            }

            return result;
        }
    }

    // Before serializing or deserializing...
    
    
    public static class Surrogates
    {
        
        public static void Register()
        {
            RuntimeTypeModel.Default.Add(typeof(PointPair), false).SetSurrogate(typeof(PointPairSurrogate));
            
            RuntimeTypeModel.Default.Add(typeof(RollingPointPairList), false).SetSurrogate(typeof(RollingPointPairListSurrogate));
            RuntimeTypeModel.Default.Add(typeof(PointPairList), false).SetSurrogate(typeof(PointPairListSurrogate));
            

            //RuntimeTypeModel.Default.Add(typeof(Object), false).SetSurrogate(typeof(Object));
        }
    }
}
