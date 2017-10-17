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

        public LoginForm()
        {
            InitializeComponent();
          
            server = "localhost";
        
           database = "sys";
        
           dbuid = "root";
            dbpassword = "";
            connstr = "SERVER=" + server + ";" + "DATABASE=" +
        database + ";" + "UID=" + dbuid + ";" + "PASSWORD=" + dbpassword + ";"+"Connection Timeout=300;";

            conn = new MySqlConnection(connstr);
        }

       

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text.ToString() == "" && textBoxPasswd.Text.ToString() == "")
            {
                labelMessage.Text = "Kérlek add meg a belépési adatokat.";
                labelMessage.ForeColor = Color.Red;
            }

            if (textBoxUserName.Text.ToString()=="")
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
                string mquerylogin = "SELECT * FROM sys.asztali where name=\"" + (textBoxUserName.Text.ToLower()) + "\"";               
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
                    }
                    conn.Close();
                    if ((name != "" && name.ToLower() == textBoxUserName.Text.ToLower()) && (passwd != "" && passwd == textBoxPasswd.Text.ToString()))
                    {
                        labelMessage.Text = "Sikeres belépés!";
                        labelMessage.ForeColor = Color.Green;
                        var homeForm = new HomeForm();
                        homeForm.Show();
                    }
                    else
                    {
                        labelMessage.Text = "Sikertelen bejelentkezés. Kérjük ellenőrizze az adatokat.";
                        labelMessage.ForeColor = Color.Red;
                    }
                }
                catch (MySqlException ex)
                {
                    labelMessage.Text = "Hiba lépett fel";
                    labelMessage.ForeColor = Color.Red;
                    MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message,"Adatbázis hiba");
                }

                
            }
        }
    }
}
