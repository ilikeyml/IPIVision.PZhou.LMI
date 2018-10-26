using Calibrition;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaptiveVision;
using System.ComponentModel;
using System.Threading;
using AvlNet;
using Gocator;
using System.Windows;
using System.Windows.Forms;

namespace IPIVision
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IPICodeMacrofilters macro;
        SynchronizationContext _context;
        Matrix<double> MMatrix = new Matrix<double>(new double[0]);
        Matrix<double> SMatrix = new Matrix<double>(new double[0]);
        Matrix<double> TMatrix = new Matrix<double>(new double[0]);
        Matrix<double> TopTopMatrix;
        Matrix<double> TopBtmMatrix;
        Matrix<double> BtmTopMatrix;
        Matrix<double> BtmBtmMatrix;

        string indexNum = "05";
        public MainWindow()
        {
            InitializeComponent();
            _context = SynchronizationContext.Current;
            macro = IPICodeMacrofilters.Create(@"C:\Users\Administrator\Source\Repos\ilikeyml\IPIVision.PZhou.LMI\IPIVision\AVCode\IPICode.avproj");
            TopTopMatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\TopTopTrans.xml");
            BtmTopMatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\BtmTopTrans.xml");
            TopBtmMatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\TopBtmTrans.xml");
            BtmBtmMatrix = SurfaceCalibration.DeserializeXmlToMatrix<Matrix<double>>(@"C:\LMIVision\BtmBtmTrans.xml");
            _context.Post(new SendOrPostCallback((o) =>
            {
                MsgBox.Text = SurfaceCalibration.MatrixToString(TopTopMatrix) + Environment.NewLine + "+++++++++++" + Environment.NewLine + SurfaceCalibration.MatrixToString(TopBtmMatrix);
            }), null);
            TopWorker.DoWork += TopWorker_DoWork;
            TopWorker.RunWorkerCompleted += TopWorker_RunWorkerCompleted;

            BtmWorker.DoWork += BtmWorker_DoWork;
            BtmWorker.RunWorkerCompleted += BtmWorker_RunWorkerCompleted;
        }

        private void BtmWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _context.Post(new SendOrPostCallback((o) =>
            {
                MsgBox.Text = $" Finished ";
            }), null);
        }

        private void BtmWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < BtmSurfaceFile.Length; i++)
            {
                BottomRunner(BtmSurfaceFile[i], BtmSurfaceRefFile[i]);
                _context.Post(new SendOrPostCallback((o) =>
                {
                    MsgBox.Text = $" Finished {i.ToString()} ";
                }), null);
            }
        }

        private void BottomRunner(string TopBottomSurfaceName, string BottomBottomSurfaceName)
        {

            Surface TopBottomSurface = null;
            AVL.LoadSurface(TopBottomSurfaceName, out TopBottomSurface);
            Surface TopBpttomSurfaceCrop;
            float? FAI7_2;
            float? FAI8;
            macro.SubBattery(TopBottomSurface, out TopBpttomSurfaceCrop, out FAI7_2, out FAI8);

            Surface BottomBottomSurface = null;
            AVL.LoadSurface(BottomBottomSurfaceName, out BottomBottomSurface);
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

            string resultStr = $"FAI7_2,{FAI7_2.Value}, FAI8,{FAI8.Value},Dis01,{dist01},Dist02,{dist02}{Environment.NewLine}";

            File.AppendAllText($@"C:\grr_btm{indexNum}.csv", resultStr);

            ;
        }

        private void TopWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _context.Post(new SendOrPostCallback((o) =>
            {
                MsgBox.Text = "Finished";
            }), null);
        }
        private void TopWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < TopSurfaceFile.Length; i++)
            {
                TopRunner(TopSurfaceFile[i], TopSurfaceRefFile[i]);
                _context.Post(new SendOrPostCallback((o) =>
                {
                    MsgBox.Text = $" Finished {i.ToString()} ";
                }), null);
            }

        }
        private void TopRunner(string TopTopSurfaceName, string BottomTopSurfaceName)
        {
            Surface TopTopSurface = null;
            AVL.LoadSurface(TopTopSurfaceName, out TopTopSurface);
            Surface TopTopSurfaceCrop;
            float? FAI7_1;
            macro.MainBattery(TopTopSurface, out TopTopSurfaceCrop, out FAI7_1);

            Surface BottomTopSurface = null;
            AVL.LoadSurface(BottomTopSurfaceName, out BottomTopSurface);
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


            string resultStr = $"FAI7_1,{FAI7_1},D1,{dist01},D2,{dist02}{Environment.NewLine}";

            File.AppendAllText($@"C:\grr_top{indexNum}.csv", resultStr);


        }

        string fileNameM = "";
        private void ButtonLoadM_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofdM = new System.Windows.Forms.OpenFileDialog();
            if (ofdM.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                fileNameM = ofdM.FileName;
                //string[] dataM = File.ReadAllLines(fileNameM);
                //MMatrix = SurfaceCalibration.CreateMatrixFromFile(dataM, true, dataM.Length);
   
            }
            ofdM.Dispose();
            //string fileNameM = @"C:\SyncFile\Project\IPI\Airpod battery\Calib\TOP_TOP_M.csv";
            //string[] dataM = File.ReadAllLines(fileNameM);
            //MMatrix = SurfaceCalibration.CreateMatrixFromFile(dataM, true, dataM.Length);
            //Matrix<double> sss = new Matrix<double>(MMatrix.Data);
            MsgBox.Text = fileNameM;
        }

        string fileNameS = "";
        private void ButtonLoadS_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofdS = new OpenFileDialog();
            if (ofdS.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                fileNameS = ofdS.FileName;
                //string[] dataS = File.ReadAllLines(fileNameS);
                //SMatrix = SurfaceCalibration.CreateMatrixFromFile(dataS, false, dataS.Length);
                MsgBox.Text = fileNameS;
            }
        }
        private void ButtonCalc_Click(object sender, RoutedEventArgs e)
        {

            //string fileNameM = @"C:\SyncFile\Project\IPI\Airpod battery\Calib\TOP_TOP_M.csv";
            string[] dataM = File.ReadAllLines(fileNameM);
            MMatrix = SurfaceCalibration.CreateMatrixFromFile(dataM, true, dataM.Length);

            //string fileNameS = @"C:\SyncFile\Project\IPI\Airpod battery\Calib\TOP_TOP_S.csv";
            string[] dataS = File.ReadAllLines(fileNameS);
            SMatrix = SurfaceCalibration.CreateMatrixFromFile(dataS, false, dataS.Length);

            TMatrix = SurfaceCalibration.Solve(MMatrix, SMatrix);
            SurfaceCalibration.SerializeMatrixToXml(TMatrix, @"C:\LMIVision\BtmTopTrans.xml");
            MsgBox.Text = $"Solve Data:{Environment.NewLine}{SurfaceCalibration.MatrixToString(TMatrix)}";

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
            string filePath = $@"C:\SyncFile\Project\IPI\Airpod battery\dataset1025\TOP_TOP\{indexNum}";
            TopSurfaceFile = Directory.GetFiles(filePath, "*.avdata");
            _context.Post(new SendOrPostCallback((o) =>
            {
                MsgBox.Text = $" Top File Count {TopSurfaceFile.Length} ";
            }), null);
        }
        private void ButtonLoadTopRef_Click(object sender, RoutedEventArgs e)
        {
            string filePath = $@"C:\SyncFile\Project\IPI\Airpod battery\dataset1025\BTM_TOP\{indexNum}";
            TopSurfaceRefFile = Directory.GetFiles(filePath, "*.avdata");
            _context.Post(new SendOrPostCallback((o) =>
            {
                MsgBox.Text = $" Top Ref File Count {TopSurfaceRefFile.Length} ";
            }), null);
        }
        BackgroundWorker TopWorker = new BackgroundWorker();
        private void ButtonTopRun_Click(object sender, RoutedEventArgs e)
        {
            TopWorker.RunWorkerAsync();

            _context.Post(new SendOrPostCallback((o) =>
            {
                MsgBox.Text = $" Running........";
            }), null);
        }


        string[] BtmSurfaceFile = new string[0];
        string[] BtmSurfaceRefFile = new string[0];


        private void ButtonLoadBtm_Click(object sender, RoutedEventArgs e)
        {
            string filePath = $@"C:\SyncFile\Project\IPI\Airpod battery\dataset1025\TOP_BTM\{indexNum}";
            BtmSurfaceFile = Directory.GetFiles(filePath, "*.avdata");
            _context.Post(new SendOrPostCallback((o) =>
            {
                MsgBox.Text = $" Bottom File Count {BtmSurfaceFile.Length} ";
            }), null);
        }

        private void ButtonLoadBtmRef_Click(object sender, RoutedEventArgs e)
        {
            string filePath = $@"C:\SyncFile\Project\IPI\Airpod battery\dataset1025\BTM_BTM\{indexNum}";
            BtmSurfaceRefFile = Directory.GetFiles(filePath, "*.avdata");
            _context.Post(new SendOrPostCallback((o) =>
            {
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
            _context.Post(new SendOrPostCallback((o) =>
            {
                MsgBox.Text = $" Running........";
            }), null);
        }
    }
}
