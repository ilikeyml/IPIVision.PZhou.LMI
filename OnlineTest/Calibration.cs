using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Calibrition;
using Emgu;
using Emgu.CV;

namespace OnlineTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }

        Matrix<double> MMatrix = new Matrix<double>(new double[0]);
        Matrix<double> SMatrix = new Matrix<double>(new double[0]);
        Matrix<double> TMatrix = new Matrix<double>(new double[0]);

        private void buttonStart_Click(object sender, EventArgs e)
        {
            TMatrix = SurfaceCalibration.Solve(MMatrix, SMatrix);
            richTextBox1.Text = SurfaceCalibration.MatrixToString(TMatrix);
        }

        

        private void buttonTest01_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;

                string[] data = File.ReadAllLines(fileName);
                MMatrix = SurfaceCalibration.CreateMatrixFromFile(data, true, data.Length);

            }
        }

        private void buttonTest02_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;

                string[] data = File.ReadAllLines(fileName);
                SMatrix = SurfaceCalibration.CreateMatrixFromFile(data, false, data.Length);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonRe_Click(object sender, EventArgs e)
        {
            Matrix<double> matrix = MMatrix * TMatrix;
            File.AppendAllText(@"C:\recheck.csv", SurfaceCalibration.MatrixToString(matrix));
        }
    }
}
