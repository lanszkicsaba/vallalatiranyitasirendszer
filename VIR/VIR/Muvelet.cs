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
        public void Torles(ListView listView1)
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
                    string query1 = "SELECT kep FROM sql11200750.termekek WHERE termeknev='" + termeknev +
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
                    if (File.Exists(@"image/" + kepnev)) File.Delete(@"image/" + kepnev);

                    conn.CloseConnection();

                    string query = "DELETE FROM sql11200750.termekek WHERE termeknev='" + termeknev +
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


        public void Adatletoltes(ListView listView1)
        {
            try
            {
                string sKeszleten;

                DBConnect conn = new DBConnect();
                MySqlDataAdapter ada = new MySqlDataAdapter("SELECT * FROM termekek", conn.returnConnection());
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
                    dr["ID"].ToString(),
                    dr["termeknev"].ToString(),
                    dr["ar"].ToString(),
                    dr["mennyiseg"].ToString(),
                    dr["kategoria"].ToString(),
                    dr["leiras"].ToString(),
                    dr["suly"].ToString(),
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
            string sourcePath = System.IO.Path.GetDirectoryName(SourcePath);
            string targetPath = @"image";

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder, if necessary.
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            // To copy a file to another location and 
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);

        }
    }

}