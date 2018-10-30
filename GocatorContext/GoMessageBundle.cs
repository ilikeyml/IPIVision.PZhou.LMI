using AvlNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GocatorContext
{
    public class GoMessageBundle
    {
        public GoContext MainContext { get; set; }
        public GoContext SubContext { get; set; }

        public List<Surface> MainSurfaceSet { get; set; } = new List<Surface>();
        public List<Surface> SubSurfaceSet { get; set; } = new List<Surface>();

    }
}
