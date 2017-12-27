using Microsoft.VisualStudio.TestTools.UnitTesting;
using VIRConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace VIRConnect.Tests
{
    [TestClass()]
    public class DBConnectTests
    {
        [TestMethod()]
        public void ConnectionTest()
        {
            DBConnect conn = new DBConnect();
            bool connect = conn.OpenConnection();

            Assert.IsTrue(connect);
            bool disconnect = conn.CloseConnection();
            Assert.IsTrue(disconnect);
        }

        [TestMethod()]
        public void MySqlCommand()
        {
            DBConnect conn = new DBConnect();
            string query1 = "SELECT * FROM termekek;";
            MySqlDataAdapter ada;


            ada = new MySqlDataAdapter("SELECT * FROM termekek", conn.returnConnection());

            MySqlCommand cmd1 = new MySqlCommand(query1, conn.returnConnection());
            conn.OpenConnection();
            DataTable dt = new DataTable();
            ada.Fill(dt);

            Assert.IsTrue(dt.Rows.Count > 0);
        }
    }
}