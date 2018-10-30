using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lmi3d.GoSdk;
using Lmi3d.GoSdk.Messages;
using Lmi3d.Zen;

namespace GocatorContext
{
    public class GoDevice : IDevice
    {
        public GoDevice()
        {
            KApiLib.Construct();
            GoSdkLib.Construct();
        }

        public virtual bool InitDevice()
        {

            throw new NotImplementedException();
        }

        public virtual bool ConnectDevice()
        {
            throw new NotImplementedException();
        }

        public virtual bool DisconnectDevice()
        {
            throw new NotImplementedException();
        }

        public virtual bool ReleaseDevice()
        {
            throw new NotImplementedException();
        }

        public virtual bool StartDevice()
        {
            throw new NotImplementedException();
        }

        public virtual bool StopDevice()
        {
            throw new NotImplementedException();
        }


    }
}
