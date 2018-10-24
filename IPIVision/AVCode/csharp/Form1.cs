using System;
using System.Windows.Forms;

using AvlNet;
using AdaptiveVision;

namespace IPICodeApplication
{
    public partial class Form1 : Form
    {

    /// <summary>Object that provides access to the macrofilters defined in the Adaptive Vision Studio project.</summary>
    /// <remarks>
    /// When deploying this application, assure that there is appropriate Microsoft Visual C++ Redistributable installed
    /// on the target machine. The Microsoft Visual C++ Redistributable version must be the same as the version of
    /// Microsoft Visual Studio used to generate the Macrofilter .Net Interface. For more information see
    /// http://docs.adaptive-vision.com/current/studio/technical_issues/NetMacrofilterInterface.html
    ///</remarks>
    private readonly IPICodeMacrofilters macros;

        public Form1()
        {
            InitializeComponent();
            try 
            {
#warning No AVS project is going to be loaded. Define path to the Adaptive Vision Project file (either avproj or avexe). Consider linking all neccessary project files in the C# project
                string avsProjectPath = string.Empty;
                macros = IPICodeMacrofilters.Create(avsProjectPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        protected override void OnClosed(System.EventArgs e)
        {
            //Release resources held by the Macrofilter .NET Interface object
            if (macros != null)
                macros.Dispose();

            base.OnClosed(e);
        }
    }
}
