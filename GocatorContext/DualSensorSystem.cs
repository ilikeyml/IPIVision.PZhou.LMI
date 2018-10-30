using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lmi3d.GoSdk;
using Lmi3d.GoSdk.Messages;
using Lmi3d.Zen;
using Lmi3d.Zen.Io;
using AvlNet;
using System.Threading;

namespace GocatorContext
{
    public class DualSensorSystem : GoDevice
    {
        public string MainIPAddress { get; set; }
        public string SubIPAddress { get; set; }
        public int BufferSize { get; set; }
        public GoContext MainContext { get; set; } = new GoContext();
        public GoContext SubContext { get; set; } = new GoContext();
        public event EventHandler<object> PassResultEvent;



        private GoSystem _system;
        private GoSensor _mainSensor;
        private GoSensor _subSensor;
        private List<GoDataSet> _mainDataSet = new List<GoDataSet>();
        private List<GoDataSet> _subDataSet = new List<GoDataSet>();
        private BackgroundWorker _mainWorker;
        private BackgroundWorker _subWorker;
        private BackgroundWorker syncWorker;
        private GoMessageBundle DualMessageSet= new GoMessageBundle();
        bool mainFinishedSign = false;
        bool subFinishedSign = false;
        bool cycleCheck = true;

        public DualSensorSystem(string mainIP, string subIP, int bufferSize)
        {
            _system = new GoSystem();
            MainIPAddress = mainIP;
            SubIPAddress = subIP;
            BufferSize = bufferSize;
            _mainWorker = new BackgroundWorker();
            _mainWorker.DoWork += _mainWorker_DoWork;
            _mainWorker.RunWorkerCompleted += _mainWorker_RunWorkerCompleted;
            _subWorker = new BackgroundWorker();
            _subWorker.DoWork += _subWorker_DoWork;
            _subWorker.RunWorkerCompleted += _subWorker_RunWorkerCompleted;
            syncWorker = new BackgroundWorker();
            syncWorker.DoWork += SyncWorker_DoWork;
            syncWorker.RunWorkerAsync();

        }

        private void SyncWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (cycleCheck)
            {

                if (mainFinishedSign&&subFinishedSign)
                {
                    cycleCheck = false;
                    PassResultEvent?.Invoke(this, DualMessageSet);
                    mainFinishedSign = false;
                    subFinishedSign = false;
                    cycleCheck = true;
                }

                Thread.Sleep(50);


            }
        }

        private void _subWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            subFinishedSign = true;
            _subDataSet.Clear();

        }

        private void _subWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            DualMessageSet.SubSurfaceSet = DataSetResolve(_subDataSet, SubContext);
            DualMessageSet.SubContext = SubContext;

        }

        private void _mainWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            mainFinishedSign = true;
            _mainDataSet.Clear();


        }

        private void _mainWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DualMessageSet.MainSurfaceSet = DataSetResolve(_mainDataSet, MainContext);
            DualMessageSet.MainContext = MainContext;
        }

        public override bool InitDevice()
        {
            _mainSensor = _system.FindSensorByIpAddress(KIpAddress.Parse(MainIPAddress));
            _subSensor = _system.FindSensorByIpAddress(KIpAddress.Parse(SubIPAddress));
            _mainSensor.Connect();
            _subSensor.Connect();
            if (_mainSensor.IsConnected() && _subSensor.IsConnected())
            {
                _system.EnableData(true);
                _mainSensor.SetDataHandler(OnMainData);
                _subSensor.SetDataHandler(OnSubData);
                return true;
            }
            return false;
        }

        private void OnSubData(GoDataSet dataSet)
        {
            _subDataSet.Add(dataSet);
            if (_subDataSet.Count == BufferSize)
            {
                _subSensor.Stop();
                _subWorker.RunWorkerAsync();

            }
        }

        private void OnMainData(GoDataSet dataSet)
        {
            _mainDataSet.Add(dataSet);
            if (_mainDataSet.Count == BufferSize)
            {
                _mainSensor.Stop();
                _mainWorker.RunWorkerAsync();


            }
        }

        private  List<Surface> DataSetResolve(List<GoDataSet> dataSetList, GoContext goContext)
        {

            List<Surface> temp = new List<Surface>();
            foreach (var item in dataSetList)
            {
                temp.Add(GoDataResolve(item, ref goContext));
            }

            return temp;

        }

        private Surface GoDataResolve(GoDataSet dataSet, ref GoContext _goContext)
        {
            Surface _surface = null;
            for (UInt32 i = 0; i < dataSet.Count; i++)
            {
                GoDataMsg dataObj = (GoDataMsg)dataSet.Get(i);
                switch (dataObj.MessageType)
                {
                    case GoDataMessageType.UniformSurface:
                        {
                            GoUniformSurfaceMsg surfaceMsg = (GoUniformSurfaceMsg)dataObj;
                            long width = surfaceMsg.Width;
                            long height = surfaceMsg.Length;
                            long bufferSize = width * height;
                            _goContext.Width = (int)width;
                            _goContext.Height = (int)height;
                            _goContext.XOffset = surfaceMsg.XOffset / 1000.0;
                            _goContext.XResolution = surfaceMsg.XResolution/1000000.0;
                            _goContext.YOffset = surfaceMsg.YOffset/1000.0;
                            _goContext.YResolution = surfaceMsg.YResolution/1000000.0;
                            _goContext.ZOffset = surfaceMsg.ZOffset/1000.0;
                            _goContext.ZResolution = surfaceMsg.ZResolution/1000000.0;
                            IntPtr bufferPointer = surfaceMsg.Data;
                            short[] ranges = new short[bufferSize];
                            Marshal.Copy(bufferPointer, ranges, 0, ranges.Length);
                             _surface = new Surface(_goContext.Width, _goContext.Height, ranges);
                            _surface.SetOffsetAndScales(_goContext.XOffset, _goContext.XResolution, _goContext.YOffset, _goContext.YResolution, _goContext.ZOffset, _goContext.ZResolution);
                    
                        }
                        break;
                }
            }

            return _surface;
        }

        private void DoMessage()
        {
            throw new NotImplementedException();
        }

        public override bool StartDevice()
        {
            if (_mainSensor.State == GoState.Running)
            {
                _mainSensor.Stop();
            }

            if (_subSensor.State == GoState.Running)
            {
                _subSensor.Stop();
            }

            _mainSensor.Start();
            _subSensor.Start();
            if (_mainSensor.State == GoState.Ready && _subSensor.State == GoState.Ready)
            {
                return true;
            }
            return false;
        }

    }
}
