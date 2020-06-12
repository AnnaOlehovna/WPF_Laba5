using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Laba5
{
    class Star : Panel
    {

        public Path path;

        public double X;
        public double Y;
        public Brush BackgroundColor;
        public Brush StrokeColor;
        public double Thickness;

        public Star() : this(0, 0, null, null, 0) { }

        public Star(double X, double Y, Brush BackgroundColor, Brush StrokeColor, double Thickness)
        {
            this.X = X;
            this.Y = Y;
            this.BackgroundColor = BackgroundColor;
            this.StrokeColor = StrokeColor;
            this.Thickness = Thickness;
            PaintFigure();

        }


        public void PaintFigure()
        {
            PathFigure pathFigure = new PathFigure();

            pathFigure.StartPoint = new Point(X - 100, Y);

            PathSegmentCollection segmentCollection = new PathSegmentCollection();

            ObservableCollection<Point> _points = new ObservableCollection<Point>() {
                new Point(X - 25, Y - 25),
                new Point(X, Y - 100),
                new Point(X + 25, Y - 25),
                new Point(X + 100, Y),
                new Point(X + 25, Y + 25),
                new Point(X, Y + 100),
                new Point(X - 25, Y + 25),
                new Point(X - 100, Y),
                new Point(X - 25, Y - 25)
            };

            PolyLineSegment line = new PolyLineSegment(_points, true);

            segmentCollection.Add(line);

            pathFigure.Segments = segmentCollection;

            PathFigureCollection figureCollection = new PathFigureCollection();

            figureCollection.Add(pathFigure);

            PathGeometry geometry = new PathGeometry();
            geometry.Figures = figureCollection;

            path = new Path();
            path.Stroke = StrokeColor;
            path.StrokeThickness = Thickness;
            path.Data = geometry;
            path.Fill = BackgroundColor;
        }

        public void GetPathStrokeThickness(double i)
        {
            this.path.StrokeThickness = i;

        }
    }
}
