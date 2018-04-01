using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Registration
{
    public partial class Form2 : Form
    {
        string[] log = {
            "エラー",//0
            "認証成功",//1
            "認証失敗",//2
            "「ユーザー名」入力欄が空です",//3
            "「パスワード」入力欄が空です",//4
            "サーバーに接続しています...",//5
            "サーバーに接続されました",//6
            "サーバーへの接続に失敗しました",//7
            "ログインに成功しました",//8
            "ログインに失敗しました",//9
            "ユーザー名またはパスワードが間違っています"
        };

        int LogMaxLine;
        int status=-1;/* -1=ログインしていない, 1=ログイン中 */

        string connstr = "userid=root; password=; database=mysql; server=localhost";

        private Form1 mainForm;

        public Form2(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "1" + Environment.NewLine + "2";
            int iLogLineHeight = richTextBox1.GetPositionFromCharIndex(richTextBox1.GetFirstCharIndexFromLine(1)).Y - richTextBox1.GetPositionFromCharIndex(richTextBox1.GetFirstCharIndexFromLine(0)).Y;
            richTextBox1.Clear();
            this.LogMaxLine = richTextBox1.ClientSize.Height / iLogLineHeight;

            richTextBox1.LanguageOption = RichTextBoxLanguageOptions.UIFonts;

            this.connect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 registerForm = new Form3();
            registerForm.ShowDialog(this);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.connect();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            MySqlConnection mysql = new MySqlConnection(connstr);
            int pass = -1;
            int username = -1;

            if (string.IsNullOrEmpty(getUserName()))
            {
                InsertScreenLog(log[3], Color.Red);
            }
            else
            {
                if(textBox1.Text == "root")
                {
                    username = 1;
                }
            }

            if(string.IsNullOrEmpty(getPassword()))
            {
                InsertScreenLog(log[4], Color.Red);
                return;
            }
            else
            {
                if(textBox2.Text == "hikaru123")
                {
                    pass = 1;
                }
            }

            if (username == -1 || pass == -1)//ログイン失敗
            {
                InsertScreenLog(log[10], Color.Red);
                this.changeStatusToNotLoggined();
            }

            if (username == 1 && pass == 1)//ログイン成功
            {
                InsertScreenLog(log[8], Color.LightGreen);
                await Task.Delay(2700);
                Form1 mainForm = new Form1();
                this.changeStatusToLoggined();
                this.Close();
            }

            await Task.Delay(7000);
            richTextBox1.Clear();
        }

        private string getUserName()
        {
            return textBox1.Text;
        }

        private string getPassword()
        {
            return textBox2.Text;
        }

        private void InsertScreenLog(String strLog, Color color)
        {
            richTextBox1.SelectedText = strLog + Environment.NewLine;
        }

        private void statusChange(int status)
        {
            this.status = status;
        }

        private async void connect()
        {
            InsertScreenLog(log[5], Color.LightGreen);
            button1.Visible = false;
            button2.Visible = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button3.Enabled = false;
            button3.Visible = false;
            await Task.Delay(5000);

            if (connectToDataBase())//接続成功
            {
                button1.Visible = true;
                button2.Visible = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button3.Enabled = false;
                button3.Visible = false;
            }
            else
            {
                //接続失敗
                button1.Visible = false;
                button2.Visible = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                button3.Enabled = true;
                button3.Visible = true;
            }
        }

        private bool connectToDataBase()
        {
            MySqlConnection conn = new MySqlConnection(connstr);
            bool success = false;

            try
            {
                conn.Open();
                InsertScreenLog(log[6], Color.LightGreen);
                conn.Close();
                success = true;
            }
            catch
            {
                InsertScreenLog(log[7], Color.Red);
                success = false;
            }
            return success;
        }

        public void changeStatusToLoggined()
        {
            this.mainForm.changeStatusToLoggined();
        }

        public void changeStatusToNotLoggined()
        {
            this.mainForm.changeStatusToNotLoggined();
        }

        /*公開鍵&秘密鍵生成*/
        public static string CreateKeys(string containerName)
        {
            //CspParametersオブジェクトの作成
            System.Security.Cryptography.CspParameters cp =
                new System.Security.Cryptography.CspParameters();
            //キーコンテナ名を指定する
            cp.KeyContainerName = containerName;
            //CspParametersを指定してRSACryptoServiceProviderオブジェクトを作成
            System.Security.Cryptography.RSACryptoServiceProvider rsa =
                new System.Security.Cryptography.RSACryptoServiceProvider(cp);

            //公開鍵をXML形式で取得して返す
            return rsa.ToXmlString(false);
        }

        /*暗号化*/
        public static string Encrypt(string str, string publicKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa =
                new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] encryptedData = rsa.Encrypt(data, false);
            return System.Convert.ToBase64String(encryptedData);
        }

        /*複合化*/
        public static string Decrypt(string str, string containerName)
        {
            //CspParametersオブジェクトの作成
            System.Security.Cryptography.CspParameters cp =
                new System.Security.Cryptography.CspParameters();
            //キーコンテナ名を指定する
            cp.KeyContainerName = containerName;
            //CspParametersを指定してRSACryptoServiceProviderオブジェクトを作成
            System.Security.Cryptography.RSACryptoServiceProvider rsa =
                new System.Security.Cryptography.RSACryptoServiceProvider(cp);

            //復号化する
            byte[] data = System.Convert.FromBase64String(str);
            byte[] decryptedData = rsa.Decrypt(data, false);
            return System.Text.Encoding.UTF8.GetString(decryptedData);
        }

        /*キーコンテナ削除*/
        public static void DeleteKeys(string containerName)
        {
            //CspParametersオブジェクトの作成
            System.Security.Cryptography.CspParameters cp =
                new System.Security.Cryptography.CspParameters();
            //キーコンテナ名を指定する
            cp.KeyContainerName = containerName;
            //CspParametersを指定してRSACryptoServiceProviderオブジェクトを作成
            System.Security.Cryptography.RSACryptoServiceProvider rsa =
                new System.Security.Cryptography.RSACryptoServiceProvider(cp);

            //キーコンテナを削除
            rsa.PersistKeyInCsp = false;
            rsa.Clear();
        }
    }
}
