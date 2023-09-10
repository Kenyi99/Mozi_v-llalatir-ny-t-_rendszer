using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    public class financy
    {
        public double? dChange { get; set; }
        public double? dIncome { get; set; }
        public double? dOutcome { get; set; }
        public double? mIncome { get; set; }
        public double? mOutcome { get; set; }
        public double? netIncome { get; set; }

        public financy(MySqlDataReader reader)
        {
            this.dChange = (double?)reader["dChange"];
            this.dIncome = (double?)reader["dIncome"];
            this.dOutcome = (double?)reader["dOutcome"];
            this.mIncome = (double?)reader["mIncome"];
            this.mOutcome = (double?)reader["mOutcome"];
            this.netIncome = (double?)reader["netIncome"];
            
        }
    }
}
