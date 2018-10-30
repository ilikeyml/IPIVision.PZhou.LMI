using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GocatorContext
{
    interface IDevice
    {
        bool InitDevice();
        bool ConnectDevice();
        bool DisconnectDevice();
        bool StartDevice();
        bool StopDevice();
        bool ReleaseDevice();

    }
}
