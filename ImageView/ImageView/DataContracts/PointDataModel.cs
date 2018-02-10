using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract]
    public class PointDataModel
    {
        [DataMember(Name = "X", Order = 1)]
        public int X { get; set; }

        [DataMember(Name = "Y", Order = 2)]
        public int Y { get; set; }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public PointDataModel()
        {

        }

        public PointDataModel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static PointDataModel CreateFromPoint(Point point)
        {
            return new PointDataModel { X = point.X, Y = point.Y };
        }
    }
}
