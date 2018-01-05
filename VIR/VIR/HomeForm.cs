using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using VIRConnect;
using Muveletek;
using System.Data;
using System.Drawing.Printing;

namespace VIR
{
    public partial class HomeForm : Form
    {
        Timer timer = new Timer();

        private static string kivalasztottTermeknev = "";
        private static string kivalasztottAr = "";
        private static string kivalasztottLeiras = "";
        private string selectedFilePathName;
        private string selectedFileName;
        private string selectedFilePathNameModositas;
        private string selectedFileNameModositas;
        bool[] check = new bool[6];
        //Ha már egyszer betöltöttük a számla adatokat kattintáskor, akkor true és többször nem fogja.
        bool betoltesbool = false;
        //private Rendelesek rend = new Rendelesek();
        
        public HomeForm()
        {
            InitializeComponent();
        }


        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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
            termekKep_pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            if (Program.logForm.Fullname == "" || Program.logForm.Fullname == null)
            {
                welcome_label.Text = "Üdvözöllek Felhasználó";
            }
            else
            {
                welcome_label.Text = "Üdvözöllek, " + Program.logForm.Fullname;
            }

            if (File.Exists("image/kezdo.png"))
            {
                termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
            }
            else
            {
                termekKep_pictureBox.Image = null;
            }

            Muvelet muvelet = new Muvelet();
            muvelet.Adatletoltes(listView1);
            timer.Start();
            timer.Tick += new EventHandler(frissites_btn_Click);
            timer.Interval = (10 * 1000); // 10 secs

            
          //  muvelet.getRend(rend);
            
        }


        private void hozzaadasMegnyitas_btn_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
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
                finally
                {
                    myStream.Close();
                }
            }
        }


        private void hozzadasUrites_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Biztosan kiszeretné üríteni az eddig beirtadatokat?", "Figyelmeztetés!", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists("image/kezdo.png"))
                {
                    termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
                }
                else
                {
                    termekKep_pictureBox.Image = null;
                }

                hozzaadasTermeknev_textBox.Clear();
                hozzaadasAr_textBox.Clear();
                hozzaadasMennyiseg_textBox.Clear();
                hozzaadasKategoria_textBox.Clear();
                richTextBox_LeirasHozzaad.Clear();
                hozzaadasSuly_textBox.Clear();
                checkBox_KeszletenHozzaadas.Checked = false;
                selectedFileName = "";
                selectedFilePathName = "";
            }
        }


        private void modositasUrites_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Biztosan kiszeretné üríteni az eddig beirtadatokat?", "Figyelmeztetés!", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists("image/kezdo.png"))
                {
                    termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
                }
                else
                {
                    termekKep_pictureBox.Image = null;
                }
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


        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Muvelet muveletek = new Muvelet();
                muveletek.Torles(listView1, termekKep_pictureBox);
            }
            else
            {
                if (listView1.SelectedIndices.Count > 0)
                {
                    kivalasztottTermeknev = listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text;
                    modositasTermeknev_textBox.Text = kivalasztottTermeknev;
                    kivalasztottAr = listView1.Items[listView1.SelectedIndices[0]].SubItems[1].Text.Replace("Ft", "");
                    modositasAr_textBox.Text = kivalasztottAr;
                    modositasMennyiseg_textBox.Text = listView1.Items[listView1.SelectedIndices[0]].SubItems[2].Text.Replace("db", "");
                    modositasKategoria_textBox.Text = listView1.Items[listView1.SelectedIndices[0]].SubItems[3].Text;
                    kivalasztottLeiras = listView1.Items[listView1.SelectedIndices[0]].SubItems[4].Text;
                    richTextBox_LeirasModositas.Text = kivalasztottLeiras;
                    modositasSuly_textBox.Text = listView1.Items[listView1.SelectedIndices[0]].SubItems[5].Text.Replace("g", "");

                    if (listView1.Items[listView1.SelectedIndices[0]].SubItems[6].Text == "Van")
                    {
                        checkBox_KeszletenModositas.Checked = true;
                    }
                    else checkBox_KeszletenModositas.Checked = false;

                    DBConnect conn = new DBConnect();

                    try
                    {
                        string query1 = "SELECT kep FROM termekek WHERE termeknev='" + listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text +
                           "' AND ar='" + int.Parse(listView1.Items[listView1.SelectedIndices[0]].SubItems[1].Text.Replace("Ft", "")) +
                           "' AND leiras='" + listView1.Items[listView1.SelectedIndices[0]].SubItems[4].Text + "';";

                        MySqlDataReader reader1;
                        MySqlCommand cmd1 = new MySqlCommand(query1, conn.returnConnection());

                        conn.OpenConnection();
                        reader1 = cmd1.ExecuteReader();

                        string kepnev = "";

                        while (reader1.Read())
                        {
                            kepnev = reader1.GetString(0);
                        }

                        conn.CloseConnection();

                        if (File.Exists(@"image/" + kepnev))
                        {
                            termekKep_pictureBox.Image = new Bitmap("image/" + kepnev);
                        }
                        else
                        {
                            if (File.Exists("image/kezdo.png"))
                            {
                                termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
                            }
                            else
                            {
                                termekKep_pictureBox.Image = null;
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Adatbázis hiba");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hiba történt! \n" + ex.Message, "Hiba");
                    }
                }
            }
        }

        private void hozzaadas_btn_Click(object sender, EventArgs e)
        {
            int ar;
            int mennyiseg;
            int suly;

            if (int.TryParse(hozzaadasAr_textBox.Text, out ar) == false && hozzaadasAr_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték az árnál. \n Maximum: 2147483647.", "Hiba");
                hozzaadasAr_textBox.Select();
            }
            else if (int.TryParse(hozzaadasMennyiseg_textBox.Text, out mennyiseg) == false && hozzaadasMennyiseg_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték a mennyiségnél. \n Maximum: 2147483647.", "Hiba");
                hozzaadasMennyiseg_textBox.Select();
            }
            else if (int.TryParse(hozzaadasSuly_textBox.Text, out suly) == false && hozzaadasSuly_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték a súlynál. \n Maximum: 2147483647.", "Hiba");
                hozzaadasSuly_textBox.Select();
            }
            else if (hozzaadasTermeknev_textBox.Text == string.Empty)
            {
                MessageBox.Show("Név megadása kötelező.", "Hiba");
                hozzaadasTermeknev_textBox.Select();
            }

            else
            {

                DBConnect conn = new DBConnect();

                string kep = "";

                try
                {
                    string termeknev = hozzaadasTermeknev_textBox.Text.ToString();
                    string kategoria = hozzaadasKategoria_textBox.Text.ToString();
                    string leiras = richTextBox_LeirasHozzaad.Text.ToString();

                    if (selectedFileName != null && selectedFileName != "")
                    {
                        Muvelet muvelet = new Muvelet();
                        muvelet.FileCopy(selectedFileName, selectedFilePathName);
                        kep = selectedFileName;
                    }
                    else
                    {
                        kep = "";
                    }

                    int iKeszleten;

                    if (checkBox_KeszletenHozzaadas.Checked == true)
                    {
                        iKeszleten = 1;
                    }
                    else
                    {
                        iKeszleten = 0;
                    }

                    string query = "INSERT INTO termekek(id,termeknev,ar,mennyiseg,kategoria,leiras,suly,kep,keszleten)VALUES('" + null +
                            "','" + termeknev +
                            "','" + ar +
                            "','" + mennyiseg +
                            "','" + kategoria +
                            "','" + leiras +
                            "','" + suly +
                            "','" + kep +
                            "','" + iKeszleten + "')";

                    MySqlDataReader reader;
                    MySqlCommand cmd = new MySqlCommand(query, conn.returnConnection());
                    conn.OpenConnection();
                    reader = cmd.ExecuteReader();

                    while (reader.Read()) { }

                    listView1.Items.Clear();
                    Muvelet muveletek = new Muvelet();
                    muveletek.Adatletoltes(listView1);

                    if (File.Exists("Image/kezdo.png"))
                    {
                        termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
                    }
                    else
                    {
                        termekKep_pictureBox.Image = null;
                    }

                    hozzaadasTermeknev_textBox.Clear();
                    hozzaadasAr_textBox.Clear();
                    hozzaadasMennyiseg_textBox.Clear();
                    hozzaadasKategoria_textBox.Clear();
                    richTextBox_LeirasHozzaad.Clear();
                    hozzaadasSuly_textBox.Clear();
                    checkBox_KeszletenHozzaadas.Checked = false;
                    selectedFileName = "";
                    selectedFilePathName = "";
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
        }


        private async void tableexport_btn_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "CSV File (*.csv)|*.csv|Excel munkafüzet (*.xlsx)|*.xls", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create), Encoding.UTF8))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Terméknév;Ár;Mennyiség;Kategória;Leirás;Súly;Készleten");

                        foreach (ListViewItem item in listView1.Items)
                        {
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text, item.SubItems[3].Text, item.SubItems[4].Text, item.SubItems[5].Text, item.SubItems[6].Text));
                        }

                        await sw.WriteLineAsync(sb.ToString());
                    }
                }
            }
        }


        private void modositas_btn_Click(object sender, EventArgs e)
        {
            int ar;
            int mennyiseg;
            int suly;

            if (int.TryParse(modositasAr_textBox.Text, out ar) == false && modositasAr_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték az árnál. \n Maximum: 2147483647.", "Hiba");
                modositasAr_textBox.Select();
            }
            else if (int.TryParse(modositasMennyiseg_textBox.Text, out mennyiseg) == false && modositasMennyiseg_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték a mennyiségnél. \n Maximum: 2147483647.", "Hiba");
                modositasMennyiseg_textBox.Select();
            }
            else if (int.TryParse(modositasSuly_textBox.Text, out suly) == false && modositasSuly_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték a súlynál. \n Maximum: 2147483647.", "Hiba");
                modositasSuly_textBox.Select();
            }
            else if (modositasTermeknev_textBox.Text == string.Empty)
            {
                MessageBox.Show("Név megadása kötelező.", "Hiba");
                modositasTermeknev_textBox.Select();
            }
            else
            {
                DBConnect conn = new DBConnect();
                try
                {
                    string termeknev = modositasTermeknev_textBox.Text.ToString();
                    string kategoria = modositasKategoria_textBox.Text.ToString();
                    string leiras = richTextBox_LeirasModositas.Text.ToString();
                    string query1;
                    string kepnevTorol = "";
                    int iKeszleten = 0;

                    if (checkBox_KeszletenModositas.Checked == true)
                    {
                        iKeszleten = 1;
                    }

                    if (checkBox_KeszletenModositas.Checked == false)
                    {
                        iKeszleten = 0;
                    }

                    bool torolkep = false;

                    if (selectedFileNameModositas != null && selectedFileNameModositas != "")
                    {
                        torolkep = true;
                        string query = "SELECT kep FROM termekek WHERE termeknev='" + termeknev +
                               "' AND ar='" + ar +
                               "' AND leiras='" + leiras + "';";

                        MySqlDataReader reader;
                        MySqlCommand cmd = new MySqlCommand(query, conn.returnConnection());
                        conn.OpenConnection();
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            kepnevTorol = reader.GetString(0);
                        }

                        Muvelet muvelet = new Muvelet();
                        muvelet.FileCopy(selectedFileNameModositas, selectedFilePathNameModositas);

                        conn.CloseConnection();

                        query1 = "UPDATE termekek SET " +
                            "termeknev = '" + termeknev + "'," +
                            " ar = '" + ar + "'," +
                            " mennyiseg = '" + mennyiseg + "'," +
                            " kategoria = '" + kategoria + "', " +
                            " leiras = '" + leiras + "', " +
                            " suly = '" + suly + "'," +
                            " kep = '" + selectedFileNameModositas + "'," +
                            " keszleten = '" + iKeszleten + "'" +
                        " WHERE termeknev = '" + kivalasztottTermeknev + "' AND ar = '" + kivalasztottAr + "' AND leiras = '" + kivalasztottLeiras + "';";
                    }
                    else
                    {
                        query1 = "UPDATE termekek SET " +
                            "termeknev = '" + termeknev + "'," +
                            " ar = '" + ar + "'," +
                            " mennyiseg = '" + mennyiseg + "'," +
                            " kategoria = '" + kategoria + "', " +
                            " leiras = '" + leiras + "', " +
                            " suly = '" + suly + "'," +
                            " keszleten = '" + iKeszleten + "'" +
                            " WHERE termeknev = '" + kivalasztottTermeknev + "' AND ar = '" + kivalasztottAr + "' AND leiras = '" + kivalasztottLeiras + "';";
                    }

                    MySqlDataReader reader1;
                    MySqlCommand cmd1 = new MySqlCommand(query1, conn.returnConnection());

                    conn.OpenConnection();

                    reader1 = cmd1.ExecuteReader();

                    if (torolkep)
                    {
                        if (File.Exists(@"image/" + kepnevTorol) && kepnevTorol != "kezdo")
                        {
                            //System.GC.Collect();
                            //System.GC.WaitForPendingFinalizers();
                            File.Delete(@"image/" + kepnevTorol);
                            kepnevTorol = "";
                        }
                    }

                    listView1.Items.Clear();

                    Muvelet muveletek = new Muvelet();
                    muveletek.Adatletoltes(listView1);
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
        }


        private void modositasKep_btn_Click(object sender, EventArgs e)
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
                            selectedFilePathNameModositas = openFileDialog1.FileName;
                            termekKep_pictureBox.Image = new Bitmap(selectedFilePathNameModositas);
                            selectedFileNameModositas = Path.GetFileName(selectedFilePathNameModositas);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba, nem lehet a fájlt megnyitni.\n " + ex.Message, "Hiba");
                }
                finally
                {
                    myStream.Close();
                }
            }
        }


        private void button_Keres_Click(object sender, EventArgs e)
        {

            Muvelet keres = new Muvelet();

            timer.Stop();

            for (int i = 0; i < check.Length; i++)
            {
                check[i] = false;
            }

            if (checkBox_Nev.Checked)
            {
                check[0] = true;
            }

            if (checkBox_Ar.Checked)
            {
                check[1] = true;
            }

            if (checkBox_Mennyiseg.Checked)
            {
                check[2] = true;
            }

            if (checkBox_Kategoria.Checked)
            {
                check[3] = true;
            }

            if (checkBox_Leiras.Checked)
            {
                check[4] = true;
            }

            if (checkBox_Keszleten.Checked)
            {
                check[5] = true;
            }

            keres.Kereses(textBox_Kereses.Text, check, listView1);

        }


        private void button_Vissza_Click(object sender, EventArgs e)
        {
            Muvelet keres = new Muvelet();

            for (int i = 0; i < check.Length; i++)
            {
                check[i] = false;
            }

            checkBox_Nev.Checked = false;
            checkBox_Ar.Checked = false;
            checkBox_Mennyiseg.Checked = false;
            checkBox_Kategoria.Checked = false;
            checkBox_Leiras.Checked = false;
            checkBox_Keszleten.Checked = false;
            textBox_Kereses.Text = "";
            listView1.Items.Clear();
            keres.Adatletoltes(listView1);
            timer.Start();
        }

        Rendelesek rend;
        private void tabControl_Keszlet_Selected(object sender, TabControlEventArgs e)
        {
            if (betoltesbool == false)
            {
                betoltesbool = true;
                //Le kell tölteni azt, hogy hány darab megrendelése volt a bejelentkezett felhasználónak, így ahhoz igazítja a Rendelesek osztályban lévő tömbök méretét.
                int rendelesdb = 0;
                DBConnect conn = new DBConnect();
                string qry = "SELECT count(rendelesek.id) FROM rendelesek INNER JOIN honlapusers ON rendelesek.rendelo_id=honlapusers.id WHERE honlapusers.Username='" + Program.logForm.LoginName + "';";
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(qry, conn.returnConnection());
                conn.OpenConnection();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rendelesdb = int.Parse(reader.GetString(0));
                }
                conn.CloseConnection();
                if (tabControl_Keszlet.SelectedIndex == 1)
                {
                    rend = new Rendelesek(rendelesdb);
                    Muvelet muv = new Muvelet();
                    muv.getRend(rend);
                    vevoneve_textBox.Text = rend.rendelo_nev;
                    vevocime_textBox.Text = rend.rendelo_cim;
                    textBox_VevoTel.Text = rend.rendelo_tel;
                    
                   
                    


                    //Rendelések betöltése a comboboxba
                    for (int i = 0; i < rend.rend_id.Length; i++)
                    {
                        //Ha 1 rendelés alatt több termék van, akkor többször szerepel egy ID, de ide elég csak 1x betölteni egy rendelést.
                        
                        if (i != 0)
                        {
                            if (rend.rend_id[i - 1] != rend.rend_id[i])
                            {
                                
                                string item = "Rendelés: " + rend.rend_id[i] + " - Idő: " + rend.rend_ido[i];                               
                                megrendelesek_comboBox.Items.Add(item);
                            }
                        }
                        else if (i==0)
                        {
                            string item = "Rendelés: " + rend.rend_id[i] + " - Idő: " + rend.rend_ido[i];
                            megrendelesek_comboBox.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void megrendelesek_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {            
            //Számla sorszám label
            label_Sorszam.Text = "Sorszám: " + rend.rend_ido[megrendelesek_comboBox.SelectedIndex].Year + "/" + rend.rend_id[megrendelesek_comboBox.SelectedIndex];
            //Nyomtatás engedélyezése
            button_Kiallitas.Enabled = true;
            //Sorok törlése, ha másik megrendelés lesz kiválasztva
            dGV_Rendeles.Rows.Clear();
            dgv_SzamlaTermekek.Rows.Clear();
            dGV_SzamlaOssz.Rows.Clear();
            //Datagridviewek feltöltése
            dGV_Rendeles.Rows.Add(new object[] { "Átutalás", rend.rend_ido[megrendelesek_comboBox.SelectedIndex], DateTime.Now, DateTime.Now.AddDays(10) });
            int ossznetto = 0;
            int osszbrutto = 0;
            int osszafa = 0;
            for (int i = 0; i < rend.rend_id.Length; i++)
            {
                if (rend.rend_id[i]==rend.rend_id[megrendelesek_comboBox.SelectedIndex])
                {
                    dgv_SzamlaTermekek.Rows.Add(new object[] {rend.termek_nev[i],rend.termek_ar[i], rend.termek_db[i], Math.Round((rend.termek_ar[i]*rend.termek_db[i])/1.27,0),27,Math.Round((rend.termek_ar[i]*rend.termek_db[i])-((rend.termek_ar[i]*rend.termek_db[i])/1.27),0),rend.termek_ar[i]*rend.termek_db[i]});
                    ossznetto += Convert.ToInt32((rend.termek_ar[i] * rend.termek_db[i]) / 1.27);
                    osszbrutto += rend.termek_ar[i] * rend.termek_db[i];
                    osszafa += Convert.ToInt32((rend.termek_ar[i] * rend.termek_db[i]) - ((rend.termek_ar[i] * rend.termek_db[i]) / 1.27));
                }
            }
            dGV_SzamlaOssz.Rows.Add(new object[] {ossznetto,27,osszafa,osszbrutto });

            //Alap kijelölés törlése
            dGV_Rendeles.ClearSelection();
            dGV_SzamlaOssz.ClearSelection();
            dgv_SzamlaTermekek.ClearSelection();
        }

       

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            Bitmap bmp = new Bitmap(this.tab_Szamla.Width, this.tab_Szamla.Height);
            this.tab_Szamla.DrawToBitmap(bmp, new Rectangle(0, 0, this.tab_Szamla.Width, this.tab_Szamla.Height));
           
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.DrawImage((Image)bmp,x,y);

        }
        private void button_Kiallitas_Click(object sender, EventArgs e)
        {
            int boxheight = groupBox5.Height;
            int dgvheight = dgv_SzamlaTermekek.Height;
            int osszesenpoz = groupBox6.Location.Y;
            int szamlaosszpoz = dGV_SzamlaOssz.Location.Y;

            groupBox5.Height = boxheight + (megrendelesek_comboBox.Location.Y -(groupBox5.Location.Y+boxheight));
            dgv_SzamlaTermekek.Height = groupBox5.Height - dgv_SzamlaTermekek.Location.Y-5;
            dGV_SzamlaOssz.Location = new Point(dGV_SzamlaOssz.Location.X, groupBox5.Location.Y + groupBox5.Height + 10);
            groupBox6.Location = new Point(groupBox6.Location.X, groupBox5.Location.Y + groupBox5.Height + 10);
            megrendelesek_comboBox.Visible = false;
            textBox_SzTalloz.Visible = false;
            button_Talloz.Visible = false;
            button_Kiallitas.Visible = false;


            dGV_SzamlaOssz.Parent = groupBox6;

            

            PrintDocument doc = new PrintDocument();
            doc.DefaultPageSettings.Landscape = true;
            doc.DefaultPageSettings.PrinterResolution.Kind = PrinterResolutionKind.High;
            doc.PrintPage += this.Doc_PrintPage;
           // PrintDialog pdlg = new PrintDialog();
            PrintPreviewDialog pdlg = new PrintPreviewDialog();
           
            pdlg.Document = doc;
           /* if (pdlg.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }*/
            pdlg.ShowDialog();

            groupBox5.Height = boxheight;
            dgv_SzamlaTermekek.Height = dgvheight;
            dGV_SzamlaOssz.Location = new Point(dGV_SzamlaOssz.Location.X, szamlaosszpoz);
            groupBox6.Location = new Point(groupBox6.Location.X, osszesenpoz);
            megrendelesek_comboBox.Visible = true;
            textBox_SzTalloz.Visible = true;
            button_Talloz.Visible = true;
            button_Kiallitas.Visible = true;
        }

    }

    

   
}
