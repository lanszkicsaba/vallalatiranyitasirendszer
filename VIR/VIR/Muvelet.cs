﻿using MySql.Data.MySqlClient;
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
                    @"SELECT rendelesek.id, rendelesek.rend_ido, rendeles_adatok.termek_id, termekek.termeknev, termekek.ar, rendeles_adatok.termek_db, honlapusers.Fullname, honlapusers.Address, honlapusers.Phonenumer
                  FROM ((rendelesek
                    INNER JOIN honlapusers ON rendelesek.rendelo_id = honlapusers.id)
                    INNER JOIN rendeles_adatok ON rendelesek.id = rendeles_adatok.azon)
					INNER JOIN termekek ON rendeles_adatok.termek_id = termekek.id
                  WHERE
                  honlapusers.Username='" + VIR.Program.logForm.LoginName + "' ORDER BY rendelesek.id;";
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
                    rend.rendelo_nev = (string)reader["Fullname"];
                    rend.rendelo_cim = (string)reader["Address"];
                    rend.rendelo_tel = (string)reader["Phonenumer"];
                    idx++;
                }
                conn.CloseConnection();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show("Hiba történt az adatok letöltésekor \n"+ex.Message,"Adatbázis hiba");
            }
        }
        public void Torles(ListView listView1, PictureBox pb)
        {
            DBConnect conn = new DBConnect();

            try
            {
                string termeknev = listView1.SelectedItems[0].SubItems[0].Text;
                string ar = listView1.SelectedItems[0].SubItems[1].Text;
                string mennyiseg = listView1.SelectedItems[0].SubItems[2].Text;
                string kategoria = listView1.SelectedItems[0].SubItems[3].Text;
                string leiras = listView1.SelectedItems[0].SubItems[4].Text;

                DialogResult dialogResult = MessageBox.Show("Biztosan kiszeretné törölni a " + termeknev + " megnevezésű terméket?", "Figyelmeztetés!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    string query1 = "SELECT kep FROM termekek WHERE termeknev='" + termeknev +
                       "' AND ar='" + ar +
                       "' AND mennyiseg='" + mennyiseg +
                       "' AND kategoria='" + kategoria +
                       "' AND leiras='" + leiras + "';";

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

                    string query2 = "SELECT COUNT(*) FROM termekek WHERE kep='" + kepnev + "';";
                    MySqlCommand cmd2 = new MySqlCommand(query2, conn.returnConnection());
                    conn.OpenConnection();

                    int count = Convert.ToInt32(cmd2.ExecuteScalar());
                    conn.CloseConnection();

                    if (count == 1)
                    {
                        if (File.Exists(@"image/" + kepnev))
                        {
                            if (File.Exists("image/kezdo.png"))
                            {
                                pb.Image = new Bitmap("image/kezdo.png");
                            }
                            else
                            {
                                pb.Image = null;
                            }
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            File.Delete(@"image/" + kepnev);
                            kepnev = "";
                        }
                    }

                    kepnev = "";
                    
                    string query = "DELETE FROM termekek WHERE termeknev='" + termeknev +
                        "' AND ar='" + ar +
                        "' AND mennyiseg='" + mennyiseg +
                        "' AND kategoria='" + kategoria +
                        "' AND leiras='" + leiras + "';";

                    MySqlDataReader reader;
                    MySqlCommand cmd = new MySqlCommand(query, conn.returnConnection());
                    conn.OpenConnection();
                    reader = cmd.ExecuteReader();

                    while (reader.Read()) { }

                    listView1.Items.Clear();
                    Muvelet muveletek = new Muvelet();
                    muveletek.Adatletoltes(listView1);
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
            finally
            {
                conn.CloseConnection();
            }
        }


        public void Adatletoltes(ListView listView1, string optionalQuery = "query")
        {
            try
            {
                string sKeszleten;

                DBConnect conn = new DBConnect();
                MySqlDataAdapter ada;

                if (optionalQuery != "query")
                {
                    ada = new MySqlDataAdapter(optionalQuery, conn.returnConnection());
                }
                else
                {
                    ada = new MySqlDataAdapter("SELECT * FROM termekek", conn.returnConnection());
                }

                DataTable dt = new DataTable();
                ada.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    if (dr["keszleten"].ToString() == "True")
                    {
                        sKeszleten = "Van";
                    }
                    else
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
                };

                    var listViewItem = new ListViewItem(row);
                    listView1.Items.Add(listViewItem);
                    conn.CloseConnection();
                }
            }
            catch (MySqlException ex)
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
