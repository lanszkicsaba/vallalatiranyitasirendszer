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
using VIRConnect;

namespace VIR
{
    public partial class LoginForm : Form
    {
        DBConnect conn = new DBConnect(); //Adatbázis csatlakozás
        private Form homeForm = new HomeForm(); //HomeForm deklarálása
        private string fullname = ""; //A getter része
        private string loginname = ""; //A getter része

        
        
        public LoginForm()
        {
            InitializeComponent();
            //Ha már 1x bejelentkezett, akkor Propertiesből töltse be a felhasználónevét. Ha még nem, vagy másik név, akkor töltse be a Propertiesba.
            if (Properties.Settings.Default.Username!=null || Properties.Settings.Default.Username!=textBoxUserName.Text)
            {
                textBoxUserName.Text = Properties.Settings.Default.Username;
            }
            //Ha még nincs felh. név beírva, akkor a fókusz azon a textboxon legyen
            if (textBoxUserName.Text==string.Empty)
            {
                textBoxUserName.Select();
            }
            else   //Ha már van felh. név betöltve, akkor a jelszó bevitelen legyen a fókusz
            {
                textBoxPasswd.Select();
            }
            //Kijelentkezés gomb beállítása letiltottra.
            buttonLogout.Enabled = false;         
        }

        
        //Bejelentkezés gomb
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            LoggingIn();            
        }

        //Kijelentkezés gomb
        private void buttonLogout_Click(object sender, EventArgs e)
        {
            //Ha a homeform létre volt már hozva, akkor zárja be és állítson mindent vissza az eredetire.
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
        //Bejelentkezés void
        public void LoggingIn()
        {
            //Bevitt adatok ellenőrzése és a fomr hozzá igazítása (Színek, szövegek)
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
                textBoxPasswd.BackColor = Color.White;
            }
            if (textBoxPasswd.Text.ToString() == "")
            {
                labelMessage.Text = "A jelszó mező üres. Kérlek ellenőrizd.";
                labelMessage.ForeColor = Color.Red;
                textBoxPasswd.BackColor = Color.LightCoral;
                if (textBoxUserName.Text!="")textBoxUserName.BackColor = Color.White; //Ha már közben beírta a nevét, akkor ne maradjon piros az username mező.
            }

            if (textBoxUserName.Text.ToString() != "" && textBoxPasswd.Text.ToString() != "")
            {
                //Név mentése a Propertiesbe
                Properties.Settings.Default.Username = textBoxUserName.Text;
                Properties.Settings.Default.Save();
                //Loading gif betöltése a formra.
                if (File.Exists("image/loading.gif"))
                {
                    pictureBox1.Image = Image.FromFile("image/loading.gif");
                }
                else
                {
                    pictureBox1.Image = null;
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                //Bejelentkezési adatok ellenőrzése.
                try
                {
                    string mquerylogin = "SELECT * FROM asztaliusers where name=\"" + (textBoxUserName.Text.ToLower()) + "\"";
                    MySqlCommand cmdlogin = new MySqlCommand(mquerylogin, conn.returnConnection());
                    conn.OpenConnection();
                    
                    //Visszakapott adatok kinyerése
                    string name = "";
                    string passwd = "";
                    MySqlDataReader reader = cmdlogin.ExecuteReader();

                    while (reader.Read())
                    {
                        name = reader.GetString("name");
                        passwd = reader.GetString("passwd");
                        fullname = reader.GetString("fullname");
                    }
                    conn.CloseConnection();
                    //Megegyeznek-e az adatok a lekértekkel
                    if ((name != "" && name.ToLower() == textBoxUserName.Text.ToLower()) && (passwd != "" && passwd == textBoxPasswd.Text.ToString()))
                    {
                        loginname = name;
                        labelMessage.Text = "Sikeres belépés!";
                        labelMessage.ForeColor = Color.Green;
                        textBoxUserName.BackColor = Color.LightGreen;
                        textBoxPasswd.BackColor = Color.LightGreen;
                        //Ha már egyszer be volt zárva a HomeForm, akkor hozza létre újra.
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
                    conn.CloseConnection();
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
