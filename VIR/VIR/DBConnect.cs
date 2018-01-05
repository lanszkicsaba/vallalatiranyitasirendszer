using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace VIRConnect
{
    public class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "193.164.132.164";   //MySQL szerver elérési cime
            database = "csaba";           //Adatbázis neve
            uid = "csaba";               //Felhasználói név
            password = "DPU3wX9HYmGEL8HK";   //Jelszó
            string connectionString;         //Kapcsolódási parancs
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);  //Kapcsolat létrehozása
        }

        public MySqlConnection returnConnection()
        {
            server = "193.164.132.164";   //MySQL szerver elérési cime
            database = "csaba";           //Adatbázis neve
            uid = "csaba";               //Felhasználói név
            password = "DPU3wX9HYmGEL8HK";   //Jelszó
            string connectionString;         //Kapcsolódási parancs
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            return connection = new MySqlConnection(connectionString);  //Kapcsolat létrehozása
        }

        /// <summary>
        /// Kapcsolat megnyitása
        /// </summary>
        /// <returns>igen ha sikerült, nem ha nem sikerült</returns>
        public bool OpenConnection()
        {
            try
            {
                connection.Open(); //Megnyitja a kapcsolatot
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number) //milyen számú hiba jön
                {
                    case 0:
                        MessageBox.Show("Nincs kapcsolat az adatbázissal. Kérjük vegye fel a kapcsolatot az adminisztrátorral!");
                        break; //0 akkor nincs kapcsolat
                }
                return false;
            }
        }

        /// <summary>
        /// Kapcsolat lezárása
        /// </summary>
        /// <returns>igen ha sikerült, nem ha nem sikerült</returns>
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
