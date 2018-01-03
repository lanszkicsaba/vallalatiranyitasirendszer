using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIR
{
    class Rendelesek
    {
        public int[] rend_id { get; set; }
        public DateTime[] rend_ido { get; set; }
        public int[] termek_id { get; set; }
        public string[] termek_nev { get; set; }
        public int[] termek_ar { get; set; }
        public int[] termek_db { get; set; }
        public string rendelo_nev { get; set; }
        public string rendelo_cim { get; set; }
        public string rendelo_tel { get; set; }

        public Rendelesek(int elemek)
        {
            rend_id = new int[elemek];
            rend_ido = new DateTime[elemek];
            termek_id = new int[elemek];
            termek_nev = new string[elemek];
            termek_ar = new int[elemek];
            termek_db = new int[elemek];
        }
    }
}
