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
    public partial class Form2 : Form
    {
        string[] log = {
            "Error.",
            "Succsesfully.",
            "Verfication Failed.",
            "Username is Empty.",
            "Password is Empty.",
            "Verfying..."
        };

        int LogMaxLine;
        int status;/* 0=ログインしていない, 1=ログイン中 */

        public Form2()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            status = 0;

            richTextBox1.Text = "1" + Environment.NewLine + "2";
            int iLogLineHeight = richTextBox1.GetPositionFromCharIndex(richTextBox1.GetFirstCharIndexFromLine(1)).Y - richTextBox1.GetPositionFromCharIndex(richTextBox1.GetFirstCharIndexFromLine(0)).Y;
            richTextBox1.Clear();
            this.LogMaxLine = richTextBox1.ClientSize.Height / iLogLineHeight;

            richTextBox1.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 registerForm = new Form3();
            registerForm.ShowDialog(this);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            InsertScreenLog(log[5]);
            await Task.Delay(3000);
            if (string.IsNullOrEmpty(getUserName()))
            {
                InsertScreenLog(log[3]);
            }
            else
            {

            }

            if(string.IsNullOrEmpty(getPassword()))
            {
                InsertScreenLog(log[4]);
            }
            else
            {

            }
        }

        private string getUserName()
        {
            return textBox1.Text;
        }

        private string getPassword()
        {
            return textBox2.Text;
        }

        private void InsertScreenLog(String strLog)
        {
            richTextBox1.SelectedText = strLog + Environment.NewLine;
        }

        private void statusChange(int status)
        {
            this.status = status;
        }
    }
}
