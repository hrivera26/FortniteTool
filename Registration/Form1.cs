using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Registration
{

    public partial class Form1 : Form
    {
        private Form2 logginForm;
        private int status = -1;

        public Form1()
        {
            InitializeComponent();
            this.logginForm = new Form2(this);
            this.logginForm.changeStatusToLoggined();
            this.logginForm.changeStatusToNotLoggined();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(status == 1)
            {
                button1.Enabled = false;
            }
            else
            {
                Form2 loginForm = new Form2(this);
                loginForm.ShowDialog(this);
            }
        }

        public void changeStatusToLoggined()
        {
            this.status = 1;
            this.button1.Enabled = false;
        }

        public void changeStatusToNotLoggined()
        {
            this.status = -1;
            this.button1.Enabled = true;
        }
    }
}
