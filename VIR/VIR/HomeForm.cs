using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VIRConnect;

namespace VIR
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();

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
    }
}
