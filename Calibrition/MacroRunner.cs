using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvlNet;
using AdaptiveVision;
using Emgu.CV;
using System.IO;

namespace Calibrition
{
    public class MacroRunner
    {

        public static IPICodeMacrofilters macro;

        Matrix<double> TopTopMatrix;
        Matrix<double> TopBtmMatrix;
        Matrix<double> BtmTopMatrix;
        Matrix<double> BtmBtmMatrix;
        public MacroRunner()
        {
            macro = IPICodeMacrofilters.Create(@"C:\Users\Administrator\Source\Repos\ilikeyml\IPIVision.PZhou.LMI\IPIVision\AVCode\IPICode.avproj");
            TopTopMatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\TopTopTrans.xml");
            BtmTopMatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\BtmTopTrans.xml");
            TopBtmMatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\TopBtmTrans.xml");
            BtmBtmMatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\BtmBtmTrans.xml");
        }

        public  void TopRunner(Surface TopTopSurface, Surface BottomTopSurface)
        {

            Surface TopTopSurfaceCrop;
            float? FAI7_1;
            macro.MainBattery(TopTopSurface, out TopTopSurfaceCrop, out FAI7_1);

            Plane3D? VPlane;
            Plane3D? HPlane;
            macro.MainBottomRef(BottomTopSurface, out VPlane, out HPlane);
            Point3D[] transedSurface = SurfaceMath.TransformSurface(TopTopSurfaceCrop, TopTopMatrix);

            Plane3D transedVPlane = SurfaceMath.TransPlane(VPlane.Value, BtmTopMatrix);
            Plane3D transedHPlane = SurfaceMath.TransPlane(HPlane.Value, BtmTopMatrix);
            //前X取中值
            float[] dist01Array;
            macro.FlatnessCalc(transedHPlane, transedSurface, out dist01Array);
            float dist01 = SurfaceMath.GetMaxValue(dist01Array, 80);

            float[] dist02Array;
            macro.FlatnessCalc(transedVPlane, transedSurface, out dist02Array);
            float dist02 = SurfaceMath.GetMaxValue(dist02Array, 80);
        }


        public void BottomRunner(Surface TopBottomSurface, Surface BottomBottomSurface)
        {

            Surface TopBpttomSurfaceCrop;
            float? FAI7_2;
            float? FAI8;
            macro.SubBattery(TopBottomSurface, out TopBpttomSurfaceCrop, out FAI7_2, out FAI8);

            Plane3D? VPlane;
            Plane3D? HPlane;
            macro.SubBottomRef(BottomBottomSurface, out VPlane, out HPlane);
            Point3D[] transedSurface = SurfaceMath.TransformSurface(TopBpttomSurfaceCrop, TopBtmMatrix);
            Plane3D transedVPlane = SurfaceMath.TransPlane(VPlane.Value, BtmBtmMatrix);
            Plane3D transedHPlane = SurfaceMath.TransPlane(HPlane.Value, BtmBtmMatrix);
            //前X取中值
            float[] dist01Array;
            macro.FlatnessCalc(transedHPlane, transedSurface, out dist01Array);
            float dist01 = SurfaceMath.GetMaxValue(dist01Array, 80);

            float[] dist02Array;
            macro.FlatnessCalc(transedVPlane, transedSurface, out dist02Array);
            float dist02 = SurfaceMath.GetMaxValue(dist02Array, 80);
        }
    }
}
