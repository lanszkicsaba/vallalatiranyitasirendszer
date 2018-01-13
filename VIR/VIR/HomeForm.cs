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
using System.Xml;

namespace VIR
{
    public partial class HomeForm : Form
    {
        Timer timer = new Timer(); //Automata listafrissítéshez
        private static string kivalasztottTermeknev = "";
        private static string kivalasztottAr = "";
        private static string kivalasztottLeiras = "";
        private string selectedFilePathName;
        private string selectedFileName;
        private string selectedFilePathNameModositas;
        private string selectedFileNameModositas;
        bool[] check = new bool[6];  //button_Keres_Click voidhoz változó. Ebben tárolódnak a Keresés résznél bejelölt kritériumok státusza.
        private bool Resizing = false; //Bool változó a listView1_SizeChanged Voidhoz.
        bool betoltesbool = false; //Ha már egyszer betöltöttük a számla adatokat kattintáskor, akkor true és többször nem fogja.
        //private Rendelesek rend = new Rendelesek();
        public HomeForm()
        {
            InitializeComponent();
        }


        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
         
        //HomeForm bezárása közbeni trigger
        private void HomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.logForm.HomeClosed(); //LoginForm alaphelyzetbe állítása
        }

        //Kijelentkezés gomb
        private void logout_btn_Click(object sender, EventArgs e)
        {
            this.Close(); //HomeForm bezárása
        }


        private void HomeForm_Load(object sender, EventArgs e)
        {
            
                DBConnect conn = new DBConnect();
                conn.OpenConnection();

                //Kép megjelenítő beállítása, üdvözlő szöveg megadása
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

                //Adatok frissítés
                Muvelet muvelet = new Muvelet();

                muvelet.Adatletoltes(listView1); //adatok feltöltése
                timer.Start();  //időzítő indítása
                timer.Tick += new EventHandler(frissites_btn_Click); //meghívja a frissítést
                timer.Interval = (10 * 1000); // 10 secs frissít  
        }

        /// <summary>
        /// Kép megnyitása
        /// </summary>
        private void hozzaadasMegnyitas_btn_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog(); //Tallózás
            
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog1.Filter = "Image files (*.jpg)|*.jpg|Image files (*.png)|*.png|All Files (*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            //ha igen
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            selectedFilePathName = openFileDialog1.FileName; //fájl neve
                            termekKep_pictureBox.Image = new Bitmap(selectedFilePathName); //fájl megnyitása megnézőben
                            selectedFileName = Path.GetFileName(selectedFilePathName);
                        }
                    }
                }
                catch (Exception ex) //ha nem található a fájl
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                finally
                {
                    myStream.Close(); //Streamer lezárása
                }
            }
        }

        /// <summary>
        /// Eddigi adatok kiűrítése
        /// </summary>
        private void hozzadasUrites_btn_Click(object sender, EventArgs e)
        {
            //figyelmeztetés
            DialogResult dialogResult = MessageBox.Show("Biztosan kiszeretné üríteni az eddig beirtadatokat?", "Figyelmeztetés!", MessageBoxButtons.YesNo);
            // ha igen
            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists("image/kezdo.png"))
                {
                    termekKep_pictureBox.Image = new Bitmap("image/kezdo.png"); //kezdőkép visszaállítássa
                }
                else
                {
                    termekKep_pictureBox.Image = null;
                }
                //Textboxok és checkboxok ürítése
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

        /// <summary>
        /// Eddigi adatok kiűrítése
        /// </summary>
        private void modositasUrites_btn_Click(object sender, EventArgs e)
        {
            //figyelmeztetés
            DialogResult dialogResult = MessageBox.Show("Biztosan kiszeretné üríteni az eddig beirtadatokat?", "Figyelmeztetés!", MessageBoxButtons.YesNo);
            // ha igen
            if (dialogResult == DialogResult.Yes)
            {
                if (File.Exists("image/kezdo.png"))
                {
                    termekKep_pictureBox.Image = new Bitmap("image/kezdo.png"); //kezdőkép visszaállítássa
                }
                else
                {
                    termekKep_pictureBox.Image = null;
                }
                //Textboxok és checkboxok ürítése
                modositasTermeknev_textBox.Clear();
                modositasAr_textBox.Clear();
                modositasMennyiseg_textBox.Clear();
                modositasKategoria_textBox.Clear();
                richTextBox_LeirasModositas.Clear();
                modositasSuly_textBox.Clear();
                checkBox_KeszletenModositas.Checked = false;
            }
        }

        /// <summary>
        /// A ListView lista frissítés gombja
        /// </summary>


        private void frissites_btn_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();      //ListView Kiürítése
                Muvelet muveletek = new Muvelet(); //Muvelet osztály meghívása        
                muveletek.Adatletoltes(listView1); //Adatletöltése és átadása ListViewnek
            }
            catch (MySqlException ex)
            {
                timer.Stop();
                MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Adatbázis hiba");
            }
            catch (Exception ex)
            {
                timer.Stop();
                MessageBox.Show("Hiba történt! \n" + ex.Message, "Hiba");
            }
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

        /// <summary>
        /// Termékhozzáadása gomb
        /// </summary>
        private void hozzaadas_btn_Click(object sender, EventArgs e)
        {
            int ar; 
            int mennyiseg;
            int suly;
            //Ha nem megfelelő adatot próbálunk bevinni
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

                DBConnect conn = new DBConnect();  //Adatbázis kapcsolat

                string kep = ""; //kép elérési útja

                try
                {
                    //Szöveggé alakítások
                    string termeknev = hozzaadasTermeknev_textBox.Text.ToString(); 
                    string kategoria = hozzaadasKategoria_textBox.Text.ToString();
                    string leiras = richTextBox_LeirasHozzaad.Text.ToString();

                    //ha megfelelő a tallózott fájl elérési útja
                    if (selectedFileName != null && selectedFileName != "")
                    {
                        Muvelet muvelet = new Muvelet(); 
                        muvelet.FileCopy(selectedFileName, selectedFilePathName); //képmásolása a "szerverre"
                        kep = selectedFileName;                                   //a kép elérési útja
                    }
                    else
                    {
                        kep = "";   //ha nem megfelelő a tallózott fájl elérési útja
                    }

                    //készleten van-e
                    int iKeszleten;

                    if (checkBox_KeszletenHozzaadas.Checked == true)
                    {
                        iKeszleten = 1;
                    }
                    else
                    {
                        iKeszleten = 0;
                    }

                    //Beszúró query
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
                    MySqlCommand cmd = new MySqlCommand(query, conn.returnConnection()); //query parancsá alakítása 
                    conn.OpenConnection(); //kapcsolat megnyitása
                    reader = cmd.ExecuteReader();                                      //query lefuttatása

                    while (reader.Read()) { }      //megvárja míg lefut a query

                    listView1.Items.Clear(); //Listview űrítése
                    Muvelet muveletek = new Muvelet();
                    muveletek.Adatletoltes(listView1); //Listview feltöltése

                    if (File.Exists("Image/kezdo.png"))         // Kezdőkép beállítása
                    {
                        termekKep_pictureBox.Image = new Bitmap("image/kezdo.png");
                    }
                    else
                    {
                        termekKep_pictureBox.Image = null;
                    }

                    //Textboxok, checkboxok alaphelyzetre állítása
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
                catch (ArgumentNullException) //nincs kitöltve valami
                {
                    MessageBox.Show("Töltsön ki minden mezőt!", "Hiba");
                }
                catch (FormatException) //hibás a formátuma
                {
                    MessageBox.Show("Nem megfelelő a bevitt adat. \n Kérem ellenőrizze!", "Hiba");
                }
                catch (MySqlException ex) //Adatbázis elérési hiba
                {
                    MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Adatbázis hiba");
                }
                catch (Exception ex) //egyébhiba
                {
                    MessageBox.Show("Hiba történt! \n" + ex.Message, "Hiba");
                }
                finally
                {
                    conn.CloseConnection(); //Kapcsolat lezárása
                }
            }
        }

        /// <summary>
        /// A táblát kiexportálja excel és CSV file-ba
        /// </summary>
        private async void tableexport_btn_Click(object sender, EventArgs e)
        {
            try
            {
                //Tallózás megnyitása
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "CSV File (*.csv)|*.csv|Excel munkafüzet (*.xlsx)|*.xls", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {    //fájl létrehozása
                        using (StreamWriter sw = new StreamWriter(new FileStream(sfd.FileName, FileMode.Create), Encoding.UTF8))
                        {   //fájl kiírása
                            StringBuilder sb = new StringBuilder();
                            //Első sor
                            sb.AppendLine("Terméknév;Ár;Mennyiség;Kategória;Leirás;Súly;Készleten");
                            //elemek kiírása
                            foreach (ListViewItem item in listView1.Items)
                            {
                                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text, item.SubItems[3].Text, item.SubItems[4].Text, item.SubItems[5].Text, item.SubItems[6].Text));
                            }

                            await sw.WriteLineAsync(sb.ToString()); //kiírás
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Hiba történt! \n" + ex.Message, "Hiba");
            }

        }

        //Termék frissítése
        private void modositas_btn_Click(object sender, EventArgs e)
        {
            int ar;
            int mennyiseg;
            int suly;
            //Ha az int maximumánál nagyobb érték van megadva
            if (int.TryParse(modositasAr_textBox.Text, out ar) == false && modositasAr_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték az árnál. \n Maximum: 2147483647.", "Hiba");
                modositasAr_textBox.Select(); //Bejelöli a hibás beviteli részt
            }
            //Ha az int maximumánál nagyobb érték van megadva
            else if (int.TryParse(modositasMennyiseg_textBox.Text, out mennyiseg) == false && modositasMennyiseg_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték a mennyiségnél. \n Maximum: 2147483647.", "Hiba");
                modositasMennyiseg_textBox.Select(); //Bejelöli a hibás beviteli részt
            }
            else if (int.TryParse(modositasSuly_textBox.Text, out suly) == false && modositasSuly_textBox.Text != string.Empty)
            {
                MessageBox.Show("Túl nagy érték a súlynál. \n Maximum: 2147483647.", "Hiba");
                modositasSuly_textBox.Select();
            }
                //Név nélkül nem lehet feltölteni.
            else if (modositasTermeknev_textBox.Text == string.Empty)
            {
                MessageBox.Show("Név megadása kötelező.", "Hiba");
                modositasTermeknev_textBox.Select();
            } //Ha minden jó
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

        //HomeForm Keresés rész
        private void button_Keres_Click(object sender, EventArgs e)
        {

            Muvelet keres = new Muvelet();
            //Automata frissítés megállítása
            timer.Stop();
            //A check tömb alapra állítása
            for (int i = 0; i < check.Length; i++)
            {
                check[i] = false;
            }

            //Mely keresési kritériumok lettek bepipálva.
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
            //Keresés függvény meghívása.
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

        

#region SzámlaForm
        Rendelesek rend; //Rendelések adataihoz
        private void tabControl_Keszlet_Selected(object sender, TabControlEventArgs e)
        {
            if (betoltesbool == false)
            {
                betoltesbool = true;
                //Le kell tölteni azt, hogy hány sor van a rendeles_Adatok táblában, így ahhoz igazítja a Rendelesek osztályban lévő tömbök méretét.
                int rendelesdb = 0;
                try
                {
                    DBConnect conn = new DBConnect();
                    string qry = "SELECT count(id) FROM rendeles_adatok;";
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

                        //Rendelések betöltése a comboboxba
                        for (int i = 0; i < rend.rend_id.Length; i++)
                        {
                            //Ha 1 rendelés alatt több termék van, akkor többször szerepel egy ID, de ide elég csak 1x betölteni egy rendelést.
                            //A rendelések ID szerint vannak sorbarendezve.
                            if (i != 0)
                            {
                                if (rend.rend_id[i - 1] != rend.rend_id[i]) //Ha nem egyezik az előző ID-val, akkor adja hozzá.
                                {

                                    ComboBoxItem item = new ComboBoxItem();
                                    item.Text = "Rendelés: " + rend.rend_id[i] + " - Idő: " + rend.rend_ido[i];
                                    item.Value = i;
                                    megrendelesek_comboBox.Items.Add(item);


                                }
                            }
                            else if (i == 0) //Az első elemet nem hasonlítjuk az előzőhöz, mert nincs előző.
                            {

                                ComboBoxItem item = new ComboBoxItem();
                                item.Text = "Rendelés: " + rend.rend_id[i] + " - Idő: " + rend.rend_ido[i];
                                item.Value = i;
                                megrendelesek_comboBox.Items.Add(item);

                            }
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

        private void megrendelesek_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Számla sorszámának kinyerése
                int rend_idx = (megrendelesek_comboBox.SelectedItem as ComboBoxItem).Value;

                label_Sorszam.Text = "Sorszám: " + rend.rend_ido[rend_idx].Year + "/" + rend.rend_id[rend_idx];
                //Nyomtatás engedélyezése
                button_Kiallitas.Enabled = true;
                //Sorok törlése, ha másik megrendelés lesz kiválasztva
                dGV_Rendeles.Rows.Clear();
                dgv_SzamlaTermekek.Rows.Clear();
                dGV_SzamlaOssz.Rows.Clear();
                //Textboxok feltöltése:
                vevoneve_textBox.Text = rend.rendelo_nev[rend_idx];
                vevocime_textBox.Text = rend.rendelo_cim[rend_idx];
                textBox_VevoTel.Text = rend.rendelo_tel[rend_idx];
                textBox_Vevoado.Text = rend.rendelo_tax[rend_idx];

                //Datagridviewek feltöltése
                dGV_Rendeles.Rows.Add(new object[] { "Átutalás", rend.rend_ido[rend_idx], DateTime.Now, DateTime.Now.AddDays(10) });
                int ossznetto = 0;
                int osszbrutto = 0;
                int osszafa = 0;
                for (int i = 0; i < rend.rend_id.Length; i++)
                {
                    if (rend.rend_id[i] == rend.rend_id[rend_idx])
                    {
                        dgv_SzamlaTermekek.Rows.Add(new object[] { rend.termek_nev[i], rend.termek_ar[i], rend.termek_db[i], Math.Round((rend.termek_ar[i] * rend.termek_db[i]) / 1.27, 0), 27, Math.Round((rend.termek_ar[i] * rend.termek_db[i]) - ((rend.termek_ar[i] * rend.termek_db[i]) / 1.27), 0), rend.termek_ar[i] * rend.termek_db[i] });
                        //Rendelt termékek összárának számolása
                        ossznetto += Convert.ToInt32((rend.termek_ar[i] * rend.termek_db[i]) / 1.27);
                        osszbrutto += rend.termek_ar[i] * rend.termek_db[i];
                        osszafa += Convert.ToInt32((rend.termek_ar[i] * rend.termek_db[i]) - ((rend.termek_ar[i] * rend.termek_db[i]) / 1.27));
                    }
                }
                //Összár adatok feltöltése
                dGV_SzamlaOssz.Rows.Add(new object[] { ossznetto, 27, osszafa, osszbrutto });

                //Alap kijelölések törlése
                dGV_Rendeles.ClearSelection();
                dGV_SzamlaOssz.ClearSelection();
                dgv_SzamlaTermekek.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Hiba");
            }
        }

       

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            //A számla Tabon lévő Panel3 rárajzolása egy Bitmapra.
            try
            {
                //Az oldal margójának mentése
                float x = e.MarginBounds.Left;
                float y = e.MarginBounds.Top;
                Bitmap bmp = new Bitmap(this.panel3.Width, this.panel3.Width);

                this.panel3.DrawToBitmap(bmp, new Rectangle(0, 0, panel3.Width + 2, panel3.Height + 2));
                e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                e.Graphics.DrawImage((Image)bmp, x, y);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Hiba");
            }
        }

        private void button_Kiallitas_Click(object sender, EventArgs e)
        {
            try
            {
                //Eredeti pozíciók mentése a Termékek dataGridvievének és az Összesen résznek::
                int boxheight = groupBox5.Height;
                int dgvheight = dgv_SzamlaTermekek.Height;
                int osszesenpoz = groupBox6.Location.Y;
                int szamlaosszpoz = dGV_SzamlaOssz.Location.Y;
                //A Termékek DataGridviewjének expandolása lefelé és az Összesen rész eltolása vele együtt lefelé.
                groupBox5.Height = boxheight + (megrendelesek_comboBox.Location.Y - (groupBox5.Location.Y + boxheight));
                dgv_SzamlaTermekek.Height = groupBox5.Height - dgv_SzamlaTermekek.Location.Y - 5;
                dGV_SzamlaOssz.Location = new Point(dGV_SzamlaOssz.Location.X, groupBox5.Location.Y + groupBox5.Height + 10);
                groupBox6.Location = new Point(groupBox6.Location.X, groupBox5.Location.Y + groupBox5.Height + 10);
                //A formon lévő fölösleges gombok, textboxok rejtése
                megrendelesek_comboBox.Visible = false;
                textBox_SzTalloz.Visible = false;
                button_Talloz.Visible = false;
                button_Kiallitas.Visible = false;
                button_szamlafriss.Visible = false;
                //Textboxok keretének eltűntetése
                vevoneve_textBox.BorderStyle = BorderStyle.None;
                vevocime_textBox.BorderStyle = BorderStyle.None;
                textBox_VevoTel.BorderStyle = BorderStyle.None;
                textBox_Vevoado.BorderStyle = BorderStyle.None;

                //dGV_SzamlaOssz.Parent = groupBox6;

                //Számla nyomtatása.
                PrintDocument doc = new PrintDocument();
                doc.DefaultPageSettings.Landscape = true; //Fektetett nézet
                //doc.OriginAtMargins = false; 
                doc.DefaultPageSettings.PrinterResolution.Kind = PrinterResolutionKind.High;

                doc.PrintPage += this.Doc_PrintPage;

                // PrintDialog pdlg = new PrintDialog(); //Rögtön PDF-be menti/nyomtatja ezzel.            
                PrintPreviewDialog pdlg = new PrintPreviewDialog(); //Először van egy preview és onnan lehet nyomtatni.           
                pdlg.Document = doc;
                pdlg.ShowDialog();

                //Minden visszaállítása az eredetire
                groupBox5.Height = boxheight;
                dgv_SzamlaTermekek.Height = dgvheight;
                dGV_SzamlaOssz.Location = new Point(dGV_SzamlaOssz.Location.X, szamlaosszpoz);
                groupBox6.Location = new Point(groupBox6.Location.X, osszesenpoz);
                megrendelesek_comboBox.Visible = true;
                textBox_SzTalloz.Visible = true;
                button_Talloz.Visible = true;
                button_Kiallitas.Visible = true;
                button_szamlafriss.Visible = true;
                vevoneve_textBox.BorderStyle = BorderStyle.Fixed3D;
                vevocime_textBox.BorderStyle = BorderStyle.Fixed3D;
                textBox_VevoTel.BorderStyle = BorderStyle.Fixed3D;
                textBox_Vevoado.BorderStyle = BorderStyle.Fixed3D;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Hiba");
            }
        }

        //XML tallózása a számlába
        private void button_Talloz_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.InitialDirectory = "."; //Asztal, mint kezdő mappa
            openFileDialog1.Filter = "Xml files (*.xml)|*.xml"; //Szűrő beállítása XML fájlokra
            openFileDialog1.FilterIndex = 2; //Az xml filterje legyen az alapértelmezett
            openFileDialog1.RestoreDirectory = true; //Újra megnyitáskor visszatölti a legutóbbi mappát

            if (openFileDialog1.ShowDialog() == DialogResult.OK) //Tallózó megnyitás, majd az if ágba lépés, ha OK gombra nyomtak
            {
                try
                {
                textBox_SzTalloz.Text = Path.GetDirectoryName(openFileDialog1.FileName); //Textboxba írja be a tallózott fájl mappáját.
               //XML beolvasása
                XmlDocument doc = new XmlDocument();
                doc.Load(openFileDialog1.FileName);
                    //XML adataiból a labelek, textboxok feltöltése
                label_Sorszam.Text = "Sorszám: " + doc.SelectSingleNode("rendeles/rendelesadatai/rendid").InnerText;
                vevoneve_textBox.Text = doc.SelectSingleNode("rendeles/rendeloadatai/nev").InnerText;
                vevocime_textBox.Text = doc.SelectSingleNode("rendeles/rendeloadatai/cim").InnerText;
                textBox_VevoTel.Text = doc.SelectSingleNode("rendeles/rendeloadatai/telefonszam").InnerText;
                textBox_Vevoado.Text = doc.SelectSingleNode("rendeles/rendeloadatai/adoszam").InnerText;
                XmlNodeList termekek = doc.SelectNodes("rendeles/termekekadatai/termek");
                //Régi adatok törlése a dataGridViewekből
                dGV_Rendeles.Rows.Clear();
                dgv_SzamlaTermekek.Rows.Clear();
                dGV_SzamlaOssz.Rows.Clear();
                //Rendelt termékek összárai
                int ossznetto = 0;
                int osszbrutto = 0;
                int osszafa = 0;
                    //Több termék betöltése a termékeket tartalmazó dataGridViewbe
                    foreach (XmlNode termek in termekek)
                    {
                      
                        int ar = int.Parse(termek["ara"].InnerText.Replace(" Ft",""));
                        int db = int.Parse(termek["mennyiseg"].InnerText.Replace(" db",""));
                        //Termék hozzáadása a dgv-hez
                        dgv_SzamlaTermekek.Rows.Add(new object[] {termek["termekneve"].InnerText,ar,db,Math.Round((ar*db)/1.27,0),27,Math.Round((ar*db)-((ar*db)/1.27),0),ar*db });
                           //Összárak számolása
                        ossznetto += Convert.ToInt32((ar * db) / 1.27);
                        osszbrutto += ar * db;
                        osszafa += Convert.ToInt32((ar * db) - ((ar * db) / 1.27));
                    }
                    //Összesen dgv feltöltése
                    dGV_SzamlaOssz.Rows.Add(new object[] { ossznetto, 27, osszafa, osszbrutto });
                //Alap kijelölés törlése
                dGV_Rendeles.ClearSelection();
                dGV_SzamlaOssz.ClearSelection();
                dgv_SzamlaTermekek.ClearSelection();
                button_Kiallitas.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nem sikerült kiolvasni az adatokat.\n" + ex.Message,"Hiba");
                }

            }
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            //A Resizing bool segtségével megakadályozzuk, hogy nagyon sokszor meghívódjon a triggerben lévő kód.
            if (!Resizing)
            {
                //Elkezdjük a számítást
                Resizing = true;
              
                    float totalColumnWidth = 0;

                    //Az oszlop tag-ekben szereplő számok összeadása.
                    //Ezzel segítségével tudjuk, hogy hány százalékos legyen egy oszlop. Minél nagyobb a tagben lévő szám, annál több helyet számol majd neki.
                    for (int i = 0; i < this.listView1.Columns.Count; i++)
                    {
                        totalColumnWidth += Convert.ToInt32(this.listView1.Columns[i].Tag);
                    }

                    // Az oszlopok szélességének kiszámolása                  
                    for (int i = 0; i < this.listView1.Columns.Count; i++)
                    {
                        // Elosztjuk az adott oszlop tagjét az össz taggel .Így megkapjuk, hogy hány százalékot kell elfoglalnia.
                        float colPercentage = (Convert.ToInt32(this.listView1.Columns[i].Tag) / totalColumnWidth);
                        // Kiszámoljuk a szélességét az oszlopnak: Megszorozzuk a százalékkal az egész Listview szélességét.
                        this.listView1.Columns[i].Width = (int)(colPercentage * this.listView1.ClientRectangle.Width);
                    }
            }
            //Kivonunk 2 pixelt az utolsó oszlop szélességéből, így nem fog vertikális slider megjelenni.
            this.listView1.Columns[this.listView1.Columns.Count - 1].Width = this.listView1.Columns[this.listView1.Columns.Count - 1].Width - 2;
            // Végeztünk a szélességek számításával
            Resizing = false;
        }

        private void button_szamlafriss_Click(object sender, EventArgs e)
        {
            megrendelesek_comboBox.Items.Clear();
            int rendelesdb = 0;
            try
            {
                DBConnect conn = new DBConnect();
                string qry = "SELECT count(id) FROM rendeles_adatok;";
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

                    for (int i = 0; i < rend.rend_id.Length; i++)
                    {
                        if (i != 0)
                        {
                            if (rend.rend_id[i - 1] != rend.rend_id[i])
                            {
                                ComboBoxItem item = new ComboBoxItem();
                                item.Text = "Rendelés: " + rend.rend_id[i] + " - Idő: " + rend.rend_ido[i];
                                item.Value = i;
                                megrendelesek_comboBox.Items.Add(item);
                            }
                        }
                        else if (i == 0)
                        {
                            ComboBoxItem item = new ComboBoxItem();
                            item.Text = "Rendelés: " + rend.rend_id[i] + " - Idő: " + rend.rend_ido[i];
                            item.Value = i;
                            megrendelesek_comboBox.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Hiba");
            }
        }
#endregion
    }

}
