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
        private string loginname = "";

        
        
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
            server = "193.164.132.164";
            database = "csaba";
            dbuid = "csaba";
            dbpassword = "DPU3wX9HYmGEL8HK";
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
            //Ha bezáródik a HomeForm, akkor állítson vissza minden control-t az alapra. (Color, Text, Enabled stb)
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;
            labelMessage.Text = "Sikeres kijelentkezés.";
            labelMessage.ForeColor = Color.RoyalBlue;
            buttonLogin.Enabled = true;
            textBoxPasswd.Text = "";
            textBoxPasswd.Enabled = true;
            textBoxUserName.Enabled = true;
            textBoxUserName.BackColor = Color.White;
            textBoxPasswd.BackColor = Color.White ;
        }

        //A bejelentkezett felhasználó nevéhez és usernevéhez getter.
        public string Fullname
        {
            get { return fullname; }
        }
        public string LoginName
        {
            get { return loginname; }
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
                textBoxUserName.BackColor = Color.LightCoral;
            }
            if (textBoxPasswd.Text.ToString() == "")
            {
                labelMessage.Text = "A jelszó mező üres. Kérlek ellenőrizd.";
                labelMessage.ForeColor = Color.Red;
                textBoxPasswd.BackColor = Color.LightCoral;
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
                        loginname = name;
                        labelMessage.Text = "Sikeres belépés!";
                        labelMessage.ForeColor = Color.Green;
                        textBoxUserName.BackColor = Color.LightGreen;
                        textBoxPasswd.BackColor = Color.LightGreen;

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
                        textBoxUserName.BackColor = Color.LightCoral;
                        textBoxPasswd.BackColor = Color.LightCoral;
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

        //Ha Entert nyom a felhasználó a jelszó vagy felhasználónév mezőben, akkor kezdje el a bejelentkeztetést.
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
