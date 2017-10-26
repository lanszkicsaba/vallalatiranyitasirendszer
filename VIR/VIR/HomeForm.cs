using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VIRConnect;
using Muveletek;

namespace VIR
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        { 
            InitializeComponent();
        }

        private void HomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.logForm.HomeClosed();
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            Program.logForm.HomeClosed();
            this.Close();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            if (Program.logForm.Fullname == "" || Program.logForm.Fullname == null)
            {
                welcome_label.Text = "Üdvözöllek Felhasználó";
            }
            else
            {
                welcome_label.Text = "Üdvözöllek, " + Program.logForm.Fullname;
            }

            termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
            termekKep_pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            Muvelet muvelet = new Muvelet();
            muvelet.Adatletoltes(listView1);

            Timer timer = new Timer();
            timer.Interval = (10 * 1000); // 10 secs
            timer.Tick += new EventHandler(frissites_btn_Click);
            timer.Start();
        }

        private void hozzaadas_btn_Click(object sender, EventArgs e)
        {
            DBConnect conn = new DBConnect();
            try
            {
                string termeknev = hozzaadasTermeknev_textBox.Text.ToString();
                string ar = hozzaadasAr_textBox.Text.ToString();
                string mennyiseg = hozzaadasMennyiseg_textBox.Text.ToString();
                string kategoria = hozzaadasKategoria_textBox.Text.ToString();
                string leiras = richTextBox_LeirasHozzaad.Text.ToString();
                string suly = hozzaadasSuly_textBox.Text.ToString();
                Muvelet muvelet = new Muvelet();
                muvelet.FileCopy(selectedFileName, selectedFilePathName);
                string kep;
                if (selectedFileName != null || selectedFileName != "")
                {
                    kep = selectedFileName;
                }
                else
                {
                    kep = "";
                }

                int iKeszleten;
                string sKeszleten;

                if (checkBox_KeszletenHozzaadas.Checked == true)
                {
                    iKeszleten = 1;
                    sKeszleten = "Van";
                }
                else
                {
                    iKeszleten = 0;
                    sKeszleten = "Nincs";
                }

                string query = "INSERT INTO sql11200750.termekek(id,termeknev,ar,mennyiseg,kategoria,leiras,suly,kep,keszleten)VALUES('" + null +
                        "','" + termeknev +
                        "','" + int.Parse(ar) +
                        "','" + int.Parse(mennyiseg) +
                        "','" + kategoria +
                        "','" + leiras +
                        "','" + int.Parse(suly) +
                        "','" + kep +
                        "','" + iKeszleten + "')";

                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(query, conn.returnConnection());
                conn.OpenConnection();
                reader = cmd.ExecuteReader();

                while (reader.Read()) { } //?????????? megvárja ameddig elküldi 

                listView1.Items.Clear();
                Muvelet muveletek = new Muvelet();
                muveletek.Adatletoltes(listView1);

                termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");

                hozzaadasTermeknev_textBox.Clear();
                hozzaadasAr_textBox.Clear();
                hozzaadasMennyiseg_textBox.Clear();
                hozzaadasKategoria_textBox.Clear();
                richTextBox_LeirasHozzaad.Clear();
                hozzaadasSuly_textBox.Clear();
                checkBox_KeszletenHozzaadas.Checked = false;
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Töltsön ki minden mezőt!", "Hiba");
            }
            catch (FormatException)
            {
                MessageBox.Show("Nem megfelelő a bevitt adat. \n Kérem ellenőrizze!", "Hiba");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Adatbázis hiba");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt! \n" + ex.Message, "Hiba");
            }
            finally
            {
                conn.CloseConnection();
            }
        }

        private string selectedFilePathName;
        private string selectedFileName;

        private void hozzaadasMegnyitas_btn_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Image files (*.jpg)|*.jpg|Image files (*.png)|*.png|All Files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            selectedFilePathName = openFileDialog1.FileName;
                            termekKep_pictureBox.Image = new Bitmap(selectedFilePathName);
                            selectedFileName = Path.GetFileName(selectedFilePathName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void hozzadasUrites_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Biztosan kiszeretné üríteni az eddig beirtadatokat?", "Figyelmeztetés!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
            termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
            hozzaadasTermeknev_textBox.Clear();
            hozzaadasAr_textBox.Clear();
            hozzaadasMennyiseg_textBox.Clear();
            hozzaadasKategoria_textBox.Clear();
            richTextBox_LeirasHozzaad.Clear();
            hozzaadasSuly_textBox.Clear();
            checkBox_KeszletenHozzaadas.Checked = false;
            }
        }

        private void modositasUrites_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Biztosan kiszeretné üríteni az eddig beirtadatokat?", "Figyelmeztetés!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
                modositasTermeknev_textBox.Clear();
                modositasAr_textBox.Clear();
                modositasMennyiseg_textBox.Clear();
                modositasKategoria_textBox.Clear();
                richTextBox_LeirasModositas.Clear();
                modositasSuly_textBox.Clear();
                checkBox_KeszletenModositas.Checked = false;
            }
        }

        private void frissites_btn_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            Muvelet muveletek = new Muvelet();
            muveletek.Adatletoltes(listView1);
        }
    }
}
