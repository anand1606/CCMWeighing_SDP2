using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCMDataCapture
{
    public partial class frmPass : Form
    {
        public frmPass()
        {
            InitializeComponent();
            
        }

        
        public string Password
        {
            get { return txtPassword.Text.ToString(); }
            set { txtPassword.Text = value; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

    }
}
