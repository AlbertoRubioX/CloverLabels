using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CloverLabels
{
    public partial class ViewLabel : Form
    {
        public ViewLabel()
        {
            InitializeComponent();
        }

        private void ViewLabel_Load(object sender, EventArgs e)
        {
            this.reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
            this.reportViewer1.RefreshReport();
        }
    }
}
