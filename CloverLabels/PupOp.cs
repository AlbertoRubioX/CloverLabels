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
    public partial class PupOp : Form
    {
        public int _iCant;
        public PupOp()
        {
            InitializeComponent();
        }

        private void PupOp_Load(object sender, EventArgs e)
        {
            txtCant.Text = "1";
            txtCant.SelectAll();
        }

        private void txtCant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;

            if(!string.IsNullOrEmpty(txtCant.Text))
            {
                int iCant = 0;
                if (int.TryParse(txtCant.Text.ToString(), out iCant))
                {
                    _iCant = iCant;
                    Close();
                }
                    
            }
        }
    }
}
