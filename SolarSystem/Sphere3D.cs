using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SolarSystem
{
    public abstract class RoundMesh3D
    {
        protected int _NumberOfSegments = 10;
        protected int _Radius = 20;
        protected Point3DCollection _Points;
        protected Int32Collection _TriangleIndices;

        public Int32Collection TriangleIndices
        {
            get { return _TriangleIndices; }
        }

        public virtual int Radius
        {
            get { return _Radius; }
            set
            {
                _Radius = value;
                CalculateGeometry();
            }
        }

        public virtual int NumberOfSegments
        {
            get { return _NumberOfSegments; }
            set
            {
                _NumberOfSegments = value;
                CalculateGeometry();
            }
        }

        public Point3DCollection Points
        {
            get { return _Points; }
        }

        protected abstract void CalculateGeometry();
    }

    public class DiscGeometry3D : RoundMesh3D
    {
        public DiscGeometry3D()
        {
        }

        protected override void CalculateGeometry()
        {
            int numberOfSeparators = 4 * _NumberOfSegments + 4;

            _Points = new Point3DCollection(_NumberOfSegments + 1);
            _TriangleIndices = new Int32Collection((numberOfSeparators + 1) * 3);

            _Points.Add(new Point3D(0, 0, 0));
            for (int divider = 0; divider < numberOfSeparators; divider++)
            {
                double alpha = Math.PI / 2 / (_NumberOfSegments + 1) * divider;
                _Points.Add(new Point3D(_Radius * Math.Cos(alpha), 0, -1 * Math.Sin(alpha)));

                _TriangleIndices.Add(0);
                _TriangleIndices.Add(divider + 1);
                if (divider == numberOfSeparators - 1)
                    _TriangleIndices.Add(1);
                else
                    _TriangleIndices.Add(divider + 2);
            }
        }
    }

    public class SphereGeometry3D : RoundMesh3D
    {
        protected PointCollection _TextureCoordinates;

        public PointCollection TextureCoordinates
        {
            get { return _TextureCoordinates; }
            protected set
            {
                _TextureCoordinates = value;
                CalculateGeometry();
            }
        }

        public SphereGeometry3D()
        {
        }

        protected override void CalculateGeometry()
        {
            _Points = new Point3DCollection();
            _TriangleIndices = new Int32Collection();
            _TextureCoordinates = new PointCollection();

            double dt = (Math.PI / 180) * 360.0 / _NumberOfSegments;
            double dp = (Math.PI / 180) * 180.0 / _NumberOfSegments;

            for (int pi = 0; pi <= _NumberOfSegments; pi++)
            {
                double phi = pi * dp;

                for (int ti = 0; ti <= _NumberOfSegments; ti++)
                {
                    double theta = ti * dt;

                    double x = _Radius * Math.Sin(theta) * Math.Sin(phi);
                    double y = _Radius * Math.Cos(phi);
                    double z = _Radius * Math.Cos(theta) * Math.Sin(phi);
                    _Points.Add(new Point3D(x, y, z));

                    System.Windows.Point point = new System.Windows.Point(theta / (2 * Math.PI), phi / (Math.PI));
                    _TextureCoordinates.Add(point);
                }
            }

            for (int pi = 0; pi < _NumberOfSegments; pi++)
            {
                for (int ti = 0; ti < _NumberOfSegments; ti++)
                {
                    int x0 = ti;
                    int x1 = (ti + 1);
                    int y0 = pi * (_NumberOfSegments + 1);
                    int y1 = (pi + 1) * (_NumberOfSegments + 1);

                    _TriangleIndices.Add(x0 + y0);
                    _TriangleIndices.Add(x0 + y1);
                    _TriangleIndices.Add(x1 + y0);

                    _TriangleIndices.Add(x1 + y0);
                    _TriangleIndices.Add(x0 + y1);
                    _TriangleIndices.Add(x1 + y1);
                }
            }
        }
    }
}
