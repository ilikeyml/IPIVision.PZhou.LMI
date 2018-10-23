using Calibrition;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AdaptiveVision;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using AvlNet;
using Gocator;
namespace IPIVision
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IPICodeMacrofilters macro;
        SynchronizationContext _context;
        Matrix<double> MMatrix;
        Matrix<double> SMatrix;
        Matrix<double> TMatrix;
        Matrix<double> topmatrix;
        Matrix<double> toprefmatrix;
        Matrix<double> btmmatrix;
        Matrix<double> btmrefmatrix;
        public MainWindow()
        {
            InitializeComponent();
            _context = SynchronizationContext.Current;
            macro = IPICodeMacrofilters.Create(@"C:\Users\ilike\Documents\YunCloud\ProjectFile\IPI\AVCode20181023\IPICode.avproj");
            topmatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\TopTrans.xml");
            toprefmatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\TopRefTrans.xml");
            btmmatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\BtmTrans.xml");
            btmrefmatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\BtmRefTrans.xml");
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = SurfaceCalibration.MatrixToString(topmatrix) + Environment.NewLine +"+++++++++++" + Environment.NewLine+ SurfaceCalibration.MatrixToString(toprefmatrix);
            }), null);
            TopWorker.DoWork += TopWorker_DoWork;
            TopWorker.RunWorkerCompleted += TopWorker_RunWorkerCompleted;

            BtmWorker.DoWork += BtmWorker_DoWork;
            BtmWorker.RunWorkerCompleted += BtmWorker_RunWorkerCompleted;
        }

        private void BtmWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
        }

        private void BtmWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < BtmSurfaceFile.Length; i++)
            {
                BottomRunner(BtmSurfaceFile[i], BtmSurfaceRefFile[i]);
                _context.Post(new SendOrPostCallback((o) => {
                    MsgBox.Text = $" Finished {i.ToString()} ";
                }), null);
            }
        }

        private void BottomRunner(string btmSurfaceName, string btmSurfaceRefName)
        {
            Surface btmSurface = new Surface();
            AVL.LoadSurface(btmSurfaceName, out btmSurface);

            Surface btmSurfaceRef = new Surface();
            AVL.LoadSurface(btmSurfaceRefName, out btmSurfaceRef);

            Surface subCropSurface;
            macro.SubBattery(btmSurface, out subCropSurface);

            Point3D[] transedSurface = SurfaceMath.TransformSurface(subCropSurface, btmmatrix);

            Plane3D? refVertical;
            Plane3D? refPlane;
            macro.SubBottomRef(btmSurfaceRef, out refVertical, out refPlane);

            Plane3D transedVPlane = SurfaceMath.TransPlane(refVertical.Value, btmrefmatrix);

            Plane3D transedPlane = SurfaceMath.TransPlane(refPlane.Value, btmrefmatrix);


            float? dist01;
            macro.FlatnessCalc(transedVPlane, transedSurface, out dist01);

            float? dist02;
            macro.FlatnessCalc(transedPlane, transedSurface, out dist02);

            string dataContent = $@"D1,{dist01.Value},D2,{dist02.Value}{Environment.NewLine}";
            File.AppendAllText(@"C:\IPI_grr.csv", dataContent);

        }

        private void TopWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = "Finished";
            }), null);
        }
        private void TopWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < TopSurfaceFile.Length; i++)
            {
                TopRunner(TopSurfaceFile[i], TopSurfaceRefFile[i]);
                _context.Post(new SendOrPostCallback((o) => {
                    MsgBox.Text = $" Finished {i.ToString()} ";
                }), null);
            }
   
        }
        private void TopRunner(string topSurfaceName, string topSurfaceRefName)
        {
            AvlNet.Surface inMainSurface = new Surface();
            AVL.LoadSurface(topSurfaceName, out inMainSurface);
            Surface outCalcSurface;
            float? FAI7_1;
            float? FAI7_2;
            float? outFAI8;
            macro.MainBattery(inMainSurface, out outCalcSurface, out FAI7_1, out FAI7_2, out outFAI8);
            Point3D[] transedSurface = SurfaceMath.TransformSurface(outCalcSurface, topmatrix);
            Surface topRefSurface = new Surface();
            AVL.LoadSurface(topSurfaceRefName, out topRefSurface);
            Plane3D? refVerticalPlane;
            Plane3D? refPlane;
            macro.MainBottomRef( topRefSurface, out refVerticalPlane, out refPlane);
            Plane3D transedPlaneVertical = SurfaceMath.TransPlane(refVerticalPlane.Value, toprefmatrix);
            Plane3D transedPlaneRef = SurfaceMath.TransPlane(refPlane.Value, toprefmatrix);
            float? dist01;
            float? dist02;
            macro.FlatnessCalc(transedPlaneVertical, transedSurface, out dist01);
            macro.FlatnessCalc(transedPlaneRef, transedSurface, out dist02);
            string dataContent = $@"D1,{dist01.Value},D2,{dist02.Value},FAI7-1,{FAI7_1.Value},FAI7-2,{FAI7_2},FAI8,{outFAI8.Value}{Environment.NewLine}";
            File.AppendAllText(@"C:\IPI_grr.csv", dataContent);
        
        }
        private void ButtonLoadM_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = ofd.FileName;
                string[] data = File.ReadAllLines(fileName);
                MMatrix = SurfaceCalibration.CreateMatrixFromFile(data, true, 8);
            }
        }
        private void ButtonLoadS_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = ofd.FileName;
                string[] data = File.ReadAllLines(fileName);
                SMatrix = SurfaceCalibration.CreateMatrixFromFile(data, false, 8);
            }
        }
        private void ButtonCalc_Click(object sender, RoutedEventArgs e)
        {
            TMatrix = SurfaceCalibration.Solve(MMatrix, SMatrix);
            SurfaceCalibration.SerializeMatrixToXml<Matrix<double>>(TMatrix, @"C:\LMIVision\BtmTrans.xml");
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = SurfaceCalibration.MatrixToString(TMatrix);
            }), null);
        }
        private void ButtonRecheck_Click(object sender, RoutedEventArgs e)
        {
            Matrix<double> recheck = MMatrix * TMatrix;
            File.AppendAllText(@"C:\recheck.csv", SurfaceCalibration.MatrixToString(recheck));
            System.Windows.Forms.MessageBox.Show("OK");
        }
        string[] TopSurfaceFile = new string[0];
        string[] TopSurfaceRefFile = new string[0];
        private void ButtonLoadTop_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Users\ilike\Documents\YunCloud\ProjectFile\IPI\LMI 1013\AVData\Main";
            TopSurfaceFile = Directory.GetFiles(filePath, "*.avdata");
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = $" Top File Count {TopSurfaceFile.Length} ";
            }), null);
        }
        private void ButtonLoadTopRef_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Users\ilike\Documents\YunCloud\ProjectFile\IPI\LMI 1013\AVData\MainBottom";
            TopSurfaceRefFile = Directory.GetFiles(filePath, "*.avdata");
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = $" Top Ref File Count {TopSurfaceRefFile.Length} ";
            }), null);
        }
        BackgroundWorker TopWorker = new BackgroundWorker();
        private void ButtonTopRun_Click(object sender, RoutedEventArgs e)
        {
            TopWorker.RunWorkerAsync();
            //string[] data = File.ReadAllLines(@"C:\Users\ilike\Desktop\test\btm.txt");
            //var ss = SurfaceCalibration.CreateMatrixFromFile(data, true, data.Length);
            //var sssss = ss * toprefmatrix;
            //File.AppendAllText(@"C:\Users\ilike\Desktop\test\btmTTT.txt", SurfaceCalibration.MatrixToString(sssss));
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = $" Running........";
            }), null);
        }


        string[] BtmSurfaceFile = new string[0];
        string[] BtmSurfaceRefFile = new string[0];


        private void ButtonLoadBtm_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Users\ilike\Documents\YunCloud\ProjectFile\IPI\LMI 1013\AVData\Sub";
            BtmSurfaceFile = Directory.GetFiles(filePath, "*.avdata");
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = $" Bottom File Count {BtmSurfaceFile.Length} ";
            }), null);
        }

        private void ButtonLoadBtmRef_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Users\ilike\Documents\YunCloud\ProjectFile\IPI\LMI 1013\AVData\SubBottom";
            BtmSurfaceRefFile = Directory.GetFiles(filePath, "*.avdata");
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = $" Bottom REf File Count {BtmSurfaceRefFile.Length} ";
            }), null);
        }

        BackgroundWorker BtmWorker = new BackgroundWorker();

        private void ButtonBtmRun_Click(object sender, RoutedEventArgs e)
        {
            BtmWorker.RunWorkerAsync();
            //string[] data = File.ReadAllLines(@"C:\Users\ilike\Desktop\test\btm.txt");
            //var ss = SurfaceCalibration.CreateMatrixFromFile(data, true, data.Length);
            //var sssss = ss * toprefmatrix;
            //File.AppendAllText(@"C:\Users\ilike\Desktop\test\btmTTT.txt", SurfaceCalibration.MatrixToString(sssss));
            _context.Post(new SendOrPostCallback((o) => {
                MsgBox.Text = $" Running........";
            }), null);
        }
    }
}
