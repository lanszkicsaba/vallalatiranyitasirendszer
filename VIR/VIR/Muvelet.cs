using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VIRConnect;

namespace Muveletek
{
    class Muvelet
    {
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