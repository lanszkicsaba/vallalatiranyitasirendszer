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
using Fajlmuveletek;

namespace VIR
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
            termekKep_pictureBox.Image = new Bitmap("image/kezdo.jpg");
            termekKep_pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            DBConnect conn = new DBConnect();
            MySqlDataAdapter ada = new MySqlDataAdapter("SELECT * FROM termekek", conn.returnConnection());
            DataTable dt = new DataTable();
            ada.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];

                string[] row = {dr["ID"].ToString(),
                    dr["termeknev"].ToString(),
                    dr["ar"].ToString(),
                    dr["mennyiseg"].ToString(),
                    dr["kategoria"].ToString(),
                    dr["leiras"].ToString(),
                    dr["suly"].ToString(),
                    dr["kep"].ToString(),
                    dr["keszleten"].ToString()
                };

                var listViewItem = new ListViewItem(row);
                listView1.Items.Add(listViewItem);
                conn.CloseConnection();
            }
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
            welcome_label.Text = "Üdvözöllek, " + Program.logForm.Fullname;
        }

        private void hozzaadas_btn_Click(object sender, EventArgs e)
        {
            DBConnect conn = new DBConnect();

            string id = hozzaadasID_textBox.Text.ToString();
            string termeknev = hozzaadasTermeknev_textBox.Text.ToString();
            string ar = hozzaadasAr_textBox.Text.ToString();
            string mennyiseg = hozzaadasMennyiseg_textBox.Text.ToString();
            string kategoria = hozzaadasKategoria_textBox.Text.ToString();
            string leiras = hozzaadasLeiras_textBox.Text.ToString();
            string suly = hozzaadasSuly_textBox.Text.ToString();
            Muvelet muvelet = new Muvelet();
            muvelet.FileCopy(selectedFileName, selectedFilePathName);
            string kep = selectedFileName;
            string keszleten = hozzaadasKeszleten_textBox.Text.ToString();
            string query = "INSERT INTO sql11200750.termekek(ID,termeknev,ar,mennyiseg,kategoria,leiras,suly,kep,keszleten)VALUES('" + int.Parse(id) +
                    "','" + termeknev +
                    "','" + int.Parse(ar) +
                    "','" + int.Parse(mennyiseg) +
                    "','" + kategoria +
                    "','" + leiras +
                    "','" + int.Parse(suly) +
                    "','" + kep +
                    "','" + int.Parse(keszleten) + "')";

            MySqlDataReader reader;
            MySqlCommand cmd = new MySqlCommand(query, conn.returnConnection());
            conn.OpenConnection();
            reader = cmd.ExecuteReader();
            while (reader.Read()) { }

            string[] row = { id, termeknev, ar, mennyiseg, kategoria, leiras, suly, kep, keszleten };

            var listViewItem = new ListViewItem(row);
            listView1.Items.Add(listViewItem);

            conn.CloseConnection();

            hozzaadasID_textBox.Clear();
            hozzaadasTermeknev_textBox.Clear();
            hozzaadasAr_textBox.Clear();
            hozzaadasMennyiseg_textBox.Clear();
            hozzaadasKategoria_textBox.Clear();
            hozzaadasLeiras_textBox.Clear();
            hozzaadasSuly_textBox.Clear();
            hozzaadasKeszleten_textBox.Clear();
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
                            //MessageBox.Show(selectedFilePathName);
                            selectedFileName = Path.GetFileName(selectedFilePathName);
                            //MessageBox.Show(selectedFileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
