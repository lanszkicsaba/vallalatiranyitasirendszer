using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VIRConnect;

namespace Muveletek
{
    class Muvelet
    {

        //A függvény, mely segítségével kigyűjtjük az adatokat az adatbázisból.
        public void getRend(VIR.Rendelesek rend)
        {
            try
            {
                //  Rendelesek rend = new Rendelesek();
                DBConnect conn = new DBConnect();
                //INNER JOIN segítségével összekötjük a 4 szükséges táblát. Így tudjuk a rendelés ID-ja alapján a hozzá tartozó termékek, rendelés, rendelő adatait kinyerni.
                string qry =
                    @"SELECT rendelesek.id, rendelesek.rend_ido, rendeles_adatok.termek_id, termekek.termeknev, termekek.ar, rendeles_adatok.termek_db, honlapusers.Fullname, honlapusers.Address, honlapusers.Phonenumer, honlapusers.Taxnumber
                  FROM ((rendelesek
                    INNER JOIN honlapusers ON rendelesek.rendelo_id = honlapusers.id)
                    INNER JOIN rendeles_adatok ON rendelesek.id = rendeles_adatok.azon)
					INNER JOIN termekek ON rendeles_adatok.termek_id = termekek.id
                    ORDER BY rendelesek.id;";
                //DataReader deklarálása az adatok olvasásához
                MySqlDataReader reader;
                MySqlCommand cmd = new MySqlCommand(qry, conn.returnConnection());
                conn.OpenConnection();
                reader = cmd.ExecuteReader();
                //Tömb indexelő
                int idx = 0;
                //Adatok beolvasása a megfelelő helyre
                while (reader.Read())
                {
                    rend.rend_id[idx] = (int)reader["id"];
                    rend.rend_ido[idx] = (DateTime)reader["rend_ido"];
                    rend.termek_id[idx] = (int)reader["termek_id"];
                    rend.termek_nev[idx] = (string)reader["termeknev"];
                    rend.termek_ar[idx] = (int)reader["ar"];
                    rend.termek_db[idx] = (int)reader["termek_db"];
                    rend.rendelo_nev[idx] = (string)reader["Fullname"];
                    rend.rendelo_cim[idx] = (string)reader["Address"];
                    rend.rendelo_tel[idx] = (string)reader["Phonenumer"];
                    rend.rendelo_tax[idx] = (string)reader["Taxnumber"];
                    idx++;
                }
                conn.CloseConnection();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show("Hiba történt az adatok letöltésekor \n"+ex.Message,"Adatbázis hiba");
            }
        }

        /// <summary>
        /// Adatok törlése
        /// </summary>
        /// <param name="listView1">A megjeleníteni kívánt ListView helye</param>
        /// <param name="pb">A képet megjelenítőbox</param>
        public void Torles(ListView listView1, PictureBox pb)
        {
            //Adatbázos kapcsolat
            DBConnect conn = new DBConnect();

            try
            {
                //Adatok szöveggé alakítása
                string termeknev = listView1.SelectedItems[0].SubItems[0].Text;
                string ar = listView1.SelectedItems[0].SubItems[1].Text;
                string mennyiseg = listView1.SelectedItems[0].SubItems[2].Text;
                string kategoria = listView1.SelectedItems[0].SubItems[3].Text;
                string leiras = listView1.SelectedItems[0].SubItems[4].Text;
                
                //Figyelmeztetés
                DialogResult dialogResult = MessageBox.Show("Biztosan kiszeretné törölni a " + termeknev + " megnevezésű terméket?", "Figyelmeztetés!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                //ha igen
                if (dialogResult == DialogResult.Yes)
                {
                    //MySQL query
                    string query1 = "SELECT kep FROM termekek WHERE termeknev='" + termeknev +
                       "' AND ar='" + ar +
                       "' AND mennyiseg='" + mennyiseg +
                       "' AND kategoria='" + kategoria +
                       "' AND leiras='" + leiras + "';";

                    MySqlDataReader reader1;         //adatolvasó
                    MySqlCommand cmd1 = new MySqlCommand(query1, conn.returnConnection()); //parancsá alakítás
                    conn.OpenConnection();       //kapcsolat megnyitása
                    reader1 = cmd1.ExecuteReader();  //parancs lefutatása

                    string kepnev = "";   //kép neve

                    while (reader1.Read())
                    {
                        kepnev = reader1.GetString(0); //a lekérdezett kép neve
                    }

                    conn.CloseConnection();   //kapcsolat lezárása

                    string query2 = "SELECT COUNT(*) FROM termekek WHERE kep='" + kepnev + "';"; //hányszor van a kép query
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn.returnConnection());  //parancs
                    conn.OpenConnection();                 //kapcsolat megnyitása

                    int count = Convert.ToInt32(cmd2.ExecuteScalar()); //érték konvertálása
                    conn.CloseConnection();               //kapcsolat lezárása

                    //ha az érték 1
                    if (count == 1) 
                    {
                        //ha a kép létezik 
                        if (File.Exists(@"image/" + kepnev))
                        {
                            //ha a kezdőkép létezik
                            if (File.Exists("image/kezdo.png"))
                            {
                                pb.Image = new Bitmap("image/kezdo.png"); //betölti a kezdőképet
                            }
                            else
                            {
                                pb.Image = null; 
                            }
                            GC.Collect();
                            GC.WaitForPendingFinalizers(); //kép elvetése memóriából
                            File.Delete(@"image/" + kepnev); //kép törlése
                            kepnev = ""; //képnev
                        }
                    }

                    kepnev = "";     //képnev

                    //Törlő MySQL query
                    string query = "DELETE FROM termekek WHERE termeknev='" + termeknev +
                        "' AND ar='" + ar +
                        "' AND mennyiseg='" + mennyiseg +
                        "' AND kategoria='" + kategoria +
                        "' AND leiras='" + leiras + "';";

                    MySqlDataReader reader; //adatolvasó
                    MySqlCommand cmd = new MySqlCommand(query, conn.returnConnection());   //parancsá alakítás
                    conn.OpenConnection(); //kapcsolat megnyitása
                    reader = cmd.ExecuteReader(); //parancs lefutatása

                    while (reader.Read()) { } //megvárja amíg lefut

                    listView1.Items.Clear(); //Listview űrítése
                    Muvelet muveletek = new Muvelet(); 
                    muveletek.Adatletoltes(listView1); //Listview feltöltése friss adatokkal
                }
            }
            catch (MySqlException ex) //Adatbázis hiba
            {
                MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Adatbázis hiba");
            }
            catch (Exception ex) //Egyéb hiba
            {
                MessageBox.Show("Hiba történt! \n" + ex.Message, "Hiba");
            }
            finally
            {
                conn.CloseConnection(); //kapcsolat lezárása
            }
        }

        /// <summary>
        /// Adatok letöltése az adatbázis termekek táblájából
        /// </summary>
        /// <param name="listView1">A megjeleníteni kívánt ListView helye</param>
        /// <param name="optionalQuery">Az SQL query helye</param>
        public void Adatletoltes(ListView listView1, string optionalQuery = "query")
        {
            try
            {
                string sKeszleten;

                DBConnect conn = new DBConnect();     //Kapcsolat létrehozása
                MySqlDataAdapter ada;                //Adapter létrehozása

                if (optionalQuery != "query")        //Ha van query akkor ez fut le
                {
                    ada = new MySqlDataAdapter(optionalQuery, conn.returnConnection()); //Adapter létrehoza a query által lekérni kívánt adatokat
                }
                else //Ha nincs query
                {
                    ada = new MySqlDataAdapter("SELECT * FROM termekek", conn.returnConnection());  // Adapter létrehoza a query által lekérni kívánt adatokat
                }

                DataTable dt = new DataTable(); //Adattáblák osztály létrehozása
                ada.Fill(dt);                   //Az adatok felbontása 

                for (int i = 0; i < dt.Rows.Count; i++)    //Végig megy a sorokon
                {
                    DataRow dr = dt.Rows[i]; //Az adatok sorainak lekérése

                    if (dr["keszleten"].ToString() == "True") //Ha készleten van
                    {
                        sKeszleten = "Van";
                    }
                    else                                     //Ha nincs készleten
                    {
                        sKeszleten = "Nincs";
                    }

                    string[] row = {
                    dr["termeknev"].ToString(),
                    dr["ar"].ToString() +"Ft",
                    dr["mennyiseg"].ToString() + "db",
                    dr["kategoria"].ToString(),
                    dr["leiras"].ToString(),
                    dr["suly"].ToString()+"g",
                    sKeszleten
                };     //Adatok kinyerése a sorokból és ellátni a megfelelő rövidítésekkel

                    var listViewItem = new ListViewItem(row); //Sorok elhelyezése Listviewben
                    listView1.Items.Add(listViewItem);       // feltölteni a Listviewet
                    conn.CloseConnection(); //Adatbázis kapcsolat lezárása
                }
            }
            catch (MySqlException ex)   //Ha hiba lépfel
            {
                MessageBox.Show("Kérjük mutassa meg ezt a fejlesztőnek:\n" + ex.Message, "Adatbázis hiba");
            }
        }

        public void FileCopy(string Filename, string SourcePath)
        {
            string fileName = Filename;
            string sourcePath = Path.GetDirectoryName(SourcePath);
            string targetPath = @"image";

            // Use Path class to manipulate file and directory paths.
            string sourceFile = Path.Combine(sourcePath, fileName);
            string destFile = Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            File.Copy(sourceFile, destFile, true);
        }

        public void Kereses(string szoveg, bool[] check, ListView listView1)
        {

            DBConnect conn = new DBConnect();

            string connstring = "SELECT * FROM termekek";
            bool vancheck = false;
            bool vanhiba = false;

            if (szoveg != null && szoveg != "")
            {
                if (check[0] == true)
                {
                    vancheck = true;
                    connstring += " WHERE termeknev=\'" + szoveg + "\'";
                }

                if (check[1] == true)
                {
                    try
                    {
                        if (vancheck == false)
                        {
                            vancheck = true;
                            connstring += " WHERE ar=" + int.Parse(szoveg);
                        }
                        else
                        {
                            connstring += " OR ar=" + int.Parse(szoveg);
                        }
                    }
                    catch
                    {
                        if (vanhiba == false)
                        {
                            MessageBox.Show("Hibás formátum.\nEbben az esetben csak szám lehet a keresés mezőben.", "Hiba");
                            vanhiba = true;
                        }
                    }
                }

                if (check[2] == true)
                {
                    try
                    {
                        if (vancheck == false)
                        {
                            vancheck = true;
                            connstring += " WHERE mennyiseg=" + int.Parse(szoveg);
                        }
                        else
                        {
                            connstring += " OR mennyiseg=" + int.Parse(szoveg);
                        }
                    }
                    catch
                    {
                        if (vanhiba == false)
                        {
                            MessageBox.Show("Hibás formátum. Csak szám lehet a keresés mezőben.", "Hiba");
                            vanhiba = true;
                        }
                    }
                }

                if (check[3] == true)
                {
                    if (vancheck == false)
                    {
                        vancheck = true;
                        connstring += " WHERE kategoria like \'%" + szoveg + "%\'";
                    }
                    else
                    {
                        connstring += " OR kategoria like \'%" + szoveg + "%\'";
                    }
                }

                if (check[4] == true)
                {

                    if (vancheck == false)
                    {
                        vancheck = true;
                        connstring += " WHERE leiras like \'%" + szoveg + "%\'";
                    }
                    else
                    {
                        connstring += " OR leiras like \'%" + szoveg + "%\'";
                    }

                }

                if (check[5] == true)
                {
                    if (vancheck == false)
                    {
                        vancheck = true;
                        if (szoveg.ToLower() == "van" || szoveg == "1")
                        {
                            connstring += " WHERE keszleten=1";
                        }
                        else if (szoveg.ToLower() == "nincs" || szoveg == "0")
                        {
                            connstring += " WHERE keszleten=0";
                        }
                    }
                    else
                    {
                        if (szoveg.ToLower() == "van" || szoveg == "1")
                        {
                            connstring += " OR keszleten=1";
                        }
                        else if (szoveg.ToLower() == "nincs" || szoveg == "0")
                        {
                            connstring += " OR keszleten=0";
                        }
                    }
                }
            }

            connstring += ";";

            if (vanhiba == false)
            {
                listView1.Items.Clear();
                Adatletoltes(listView1, connstring);
            }
            else
            {
                vanhiba = false;
            }
        }
    }
}
