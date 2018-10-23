using AvlNet;
using Emgu.CV;
using Gocator;
using System.Collections.Generic;

namespace Calibrition
{
    public static class SurfaceMath
    {
        public static Surface ZValeToSurface(short[] zValue, GoContext mContext)
        {
            Surface surface = new Surface(mContext.Width, mContext.Height, zValue);
            surface.SetOffsetAndScales(mContext.XOffset, mContext.XResolution, mContext.YOffset, mContext.YResolution, mContext.ZOffset, mContext.ZResolution);
            return surface;
        }

        public static Point3D[] TransformSurface(Surface inSurface, Matrix<double> TMatrix)
        {
            Point3D[] point3Ds = new Point3D[0];
            AVL.SurfaceToPoint3DArray(inSurface, out point3Ds);
            List<Point3D> TPoints = new List<Point3D>();
            Point3DGrid point3DGrid = new Point3DGrid();
            Matrix<double> matrix = new Matrix<double>(point3Ds.Length, 4);
            for (int i = 0; i < point3Ds.Length; i++)
            {
                matrix[i, 0] = point3Ds[i].X;
                matrix[i, 1] = point3Ds[i].Y;
                matrix[i, 2] = point3Ds[i].Z;
                matrix[i, 3] = 1;
            }

            Matrix<double> TResult = matrix * TMatrix;

            for (int i = 0; i < TResult.Rows; i++)
            {
                Point3D temp = new Point3D((float)TResult[i, 0], (float)TResult[i, 1], (float)TResult[i, 2]);

                TPoints.Add(temp);
            }

            return TPoints.ToArray();
        }

        public static GoContext  CreateContextFromSurface(SurfaceFormat sf)
        {
            return new GoContext()
            {
                Width = sf.Width,
                Height = sf.Height,
                XOffset = sf.XOffset,
                XResolution = sf.XScale,
                YOffset = sf.YOffset,
                YResolution = sf.YScale,
                ZOffset = sf.ZOffset,
                ZResolution = sf.ZScale

            };
        }

        public static Plane3D TransPlane(Plane3D inPlane, Matrix<double> TMatrix)
        {
            Point3D point1 = new Point3D(0, 0, 0);
            Point3D p1P;
            AVL.ProjectPointOntoPlane3D(point1, inPlane, out p1P);
            Point3D p1T = TransPoint(p1P, TMatrix);

            Point3D point2 = new Point3D(1, 0, 1);
            Point3D p2P;
            AVL.ProjectPointOntoPlane3D(point2, inPlane, out p2P);
            Point3D p2T = TransPoint(p2P, TMatrix);

            Point3D point3 = new Point3D(5, 1, 5);
            Point3D p3P;
            AVL.ProjectPointOntoPlane3D(point3, inPlane, out p3P);
            Point3D p3T = TransPoint(p3P, TMatrix);

            Plane3D TransedPlane;
            AVL.PlaneThroughPoints3D(p1T, p2T, p3T, out TransedPlane);
            return TransedPlane;
        }

        public static Point3D TransPoint(Point3D inPoint, Matrix<double> TMatrix)
        {
            Matrix<double> matrix = new Matrix<double>(1, 4);
            matrix[0, 0] = inPoint.X;
            matrix[0, 1] = inPoint.Y;
            matrix[0, 2] = inPoint.Z;
            matrix[0, 3] = 1;
            var temp = matrix * TMatrix;
            return new Point3D((float)temp[0, 0], (float)temp[0, 1], (float)temp[0, 2]);
        }


    }
}
