using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    class AllWages
    {
        public string name { get; set; }
        public DateTime date { get; set; }
        public double hours { get; set; }
        public double monthly_salary { get; set; }

        public AllWages(MySqlDataReader reader)
        {
            this.name = reader["name"].ToString();
            this.date = (DateTime)reader["day"];
            this.hours = (double)reader["Hours_monthly"];
            this.monthly_salary = (double)reader["monthly_salary"];
        }
    }
}
