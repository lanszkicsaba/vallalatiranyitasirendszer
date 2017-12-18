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
using System.Threading;
using System.Resources;
using System.IO;

namespace VIR
{
    public partial class LoginForm : Form
    {
        private MySqlConnection conn;
        private string server;
        private string database;
        private string dbuid;
        private string dbpassword;
        private string connstr;
        private Form homeForm = new HomeForm();
        private string fullname = "";

        
        
        public LoginForm()
        {
            InitializeComponent();
            if (Properties.Settings.Default.Username!=null || Properties.Settings.Default.Username!=textBoxUserName.Text)
            {
                textBoxUserName.Text = Properties.Settings.Default.Username;
            }
            if (textBoxUserName.Text==string.Empty)
            {
                textBoxUserName.Select();
            }
            else
            {
                textBoxPasswd.Select();
            }

            buttonLogout.Enabled = false;
            server = "sql11.freemysqlhosting.net";
            database = "sql11211489";
            dbuid = "sql11211489";
            dbpassword = "X5vvu2Mbk8";
            connstr = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + dbuid + ";" + "PASSWORD=" + dbpassword + ";"+"Connection Timeout=300;";

            conn = new MySqlConnection(connstr);
        }

        

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoggingIn();            
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            if (homeForm.Created==true)
            {
                homeForm.Dispose();
                HomeClosed();
            }
        }

        public void HomeClosed()
        {
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;
            labelMessage.Text = "Sikeres kijelentkezés.";
            labelMessage.ForeColor = Color.Black;
            buttonLogin.Enabled = true;
            textBoxPasswd.Text = "";
            textBoxPasswd.Enabled = true;
            textBoxUserName.Enabled = true;
        }

        public string Fullname
        {
            get { return fullname; }
        }

        private void LoggingIn()
        {
            if (textBoxUserName.Text.ToString() == "" && textBoxPasswd.Text.ToString() == "")
            {
                labelMessage.Text = "Kérlek add meg a belépési adatokat.";
                labelMessage.ForeColor = Color.Red;
            }

            if (textBoxUserName.Text.ToString() == "")
            {
                labelMessage.Text = "A felhasználónév mező üres. Kérlek ellenőrizd.";
                labelMessage.ForeColor = Color.Red;
            }
            if (textBoxPasswd.Text.ToString() == "")
            {
                labelMessage.Text = "A jelszó mező üres. Kérlek ellenőrizd.";
                labelMessage.ForeColor = Color.Red;
            }

            if (textBoxUserName.Text.ToString() != "" && textBoxPasswd.Text.ToString() != "")
            {
                //Név mentése
                Properties.Settings.Default.Username = textBoxUserName.Text;
                Properties.Settings.Default.Save();
                if (File.Exists("image/loading.gif"))
                {
                    pictureBox1.Image = Image.FromFile("image/loading.gif");
                }
                else
                {
                    pictureBox1.Image = null;
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                string mquerylogin = "SELECT * FROM asztaliusers where name=\"" + (textBoxUserName.Text.ToLower()) + "\"";
                MySqlCommand cmdlogin = new MySqlCommand(mquerylogin, conn);
                try
                {
                    conn.Open();


                    string name = "";
                    string passwd = "";
                    MySqlDataReader reader = cmdlogin.ExecuteReader();

                    while (reader.Read())
                    {
                        name = reader.GetString("name");
                        passwd = reader.GetString("passwd");
                        fullname = reader.GetString("fullname");
                    }
                    conn.Close();
                    if ((name != "" && name.ToLower() == textBoxUserName.Text.ToLower()) && (passwd != "" && passwd == textBoxPasswd.Text.ToString()))
                    {
                        labelMessage.Text = "Sikeres belépés!";
                        labelMessage.ForeColor = Color.Green;
                        if (homeForm.IsDisposed == true)
                        {
                            homeForm = new HomeForm();
                        }
                        homeForm.Show();
                        buttonLogin.Enabled = false;
                        buttonLogout.Enabled = true;
                        textBoxUserName.Enabled = false;
                        textBoxPasswd.Enabled = false;
                        pictureBox1.Image = null;
                    }
                    else
                    {
                        labelMessage.Text = "Sikertelen bejelentkezés. Kérjük ellenőrizze az adatokat.";
                        labelMessage.ForeColor = Color.Red;
                        pictureBox1.Image = null;
                    }
                }
                catch (MySqlException ex)
                {
                    conn.Close();
                    labelMessage.Text = "Hiba lépett fel";
                    labelMessage.ForeColor = Color.Red;
                    MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Adatbázis hiba");
                    pictureBox1.Image = null;
                }
            }
        }

        private void textBoxPasswd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoggingIn();
            }
        }

        private void textBoxUserName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoggingIn();
            }
        }
    }
}
