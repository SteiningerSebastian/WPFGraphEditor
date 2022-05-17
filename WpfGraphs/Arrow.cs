using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfGraphs
{
    public class Arrow : Shape
    {
        public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(Double), typeof(Arrow));
        public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(Double), typeof(Arrow));
        public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(Double), typeof(Arrow));
        public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(Double), typeof(Arrow));
        public static readonly DependencyProperty ArrowConnectedProperty = DependencyProperty.Register("ArrowConnected", typeof(Boolean), typeof(Arrow));

        public double X1
        {
            get { return (double)this.GetValue(X1Property); }
            set { this.SetValue(X1Property, value); }
        }

        public double X2
        {
            get { return (double)this.GetValue(X2Property); }
            set { this.SetValue(X2Property, value); }
        }

        public double Y1
        {
            get { return (double)this.GetValue(Y1Property); }
            set { this.SetValue(Y1Property, value); }
        }

        public double Y2
        {
            get { return (double)this.GetValue(Y2Property); }
            set { this.SetValue(Y2Property, value); }
        }

        public bool ArrowConnected
        {
            get { return (bool)this.GetValue(ArrowConnectedProperty); }
            set { this.SetValue(ArrowConnectedProperty, value); }
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                double k = (Y1 - Y2) / (X1 - X2);
                (double, double) v1 = (X1 - X2, Y1 - Y2);
                double len = Math.Sqrt(Math.Pow(v1.Item2, 2) + Math.Pow(v1.Item1, 2));
                (double, double) dv = (v1.Item1 / len * 10, v1.Item2 / len * 10);
                (double, double) nv = (dv.Item2 / 2, -dv.Item1 / 2);

                Point p1 = new Point(this.X2, this.Y2);
                Point p2 = new Point(this.X2 + dv.Item1 - nv.Item1, this.Y2 + dv.Item2 - nv.Item2);
                Point p3 = new Point(this.X2 + dv.Item1 + nv.Item1, this.Y2 + dv.Item2 + nv.Item2);

                GeometryGroup geometryGroup = new GeometryGroup();

                if (!ArrowConnected)
                {
                    Point p4 = new Point(this.X1, this.Y1);
                    Point p5 = new Point(this.X1 - dv.Item1 - nv.Item1, this.Y1 - dv.Item2 - nv.Item2);
                    Point p6 = new Point(this.X1 - dv.Item1 + nv.Item1, this.Y1 - dv.Item2 + nv.Item2);

                    List<PathSegment> seg = new List<PathSegment>(3);
                    seg.Add(new LineSegment(p4, true));
                    seg.Add(new LineSegment(p5, true));
                    seg.Add(new LineSegment(p6, true));

                    List<PathFigure> fig = new List<PathFigure>(1);
                    PathFigure pathfig = new PathFigure(p4, seg, true);
                    fig.Add(pathfig);
                    Geometry g1 = new PathGeometry(fig, FillRule.EvenOdd, null);

                    geometryGroup.Children.Add(g1);
                }

                List<PathSegment> segments = new List<PathSegment>(3);
                segments.Add(new LineSegment(p1, true));
                segments.Add(new LineSegment(p2, true));
                segments.Add(new LineSegment(p3, true));

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(p1, segments, true);
                figures.Add(pf);
                Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);

                LineGeometry lineGeometry = new LineGeometry();
                lineGeometry.StartPoint = new Point(X1, Y1);
                lineGeometry.EndPoint = new Point(X2, Y2);

                geometryGroup.Children.Add(g);
                geometryGroup.Children.Add(lineGeometry);

                return geometryGroup;
            }
        }
    }
}
