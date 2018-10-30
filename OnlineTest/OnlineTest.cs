using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GocatorContext;
using AvlNet;

namespace OnlineTest
{
    public partial class OnlineTest : Form
    {
        public OnlineTest()
        {
            InitializeComponent();
            dss.PassResultEvent += Dss_PassResultEvent;
            dss.InitDevice();


        }

        private void Dss_PassResultEvent(object sender, object e)
        {
            GoMessageBundle goMessageBundle = (GoMessageBundle)e;
            AVL.SaveSurface(goMessageBundle.MainSurfaceSet[0], @"C:\main.avdata");
            AVL.SaveSurface(goMessageBundle.SubSurfaceSet[0], @"C:\sub.avdata");
            MessageBox.Show("OK");
        }

        DualSensorSystem dss = new DualSensorSystem("192.168.1.10", "192.168.1.20", 1);

        private void buttonStart_Click(object sender, EventArgs e)
        {
            dss.StartDevice();
        }
    }
}
