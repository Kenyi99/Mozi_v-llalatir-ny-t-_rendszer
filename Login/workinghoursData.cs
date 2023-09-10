using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    public class workinghoursData
    {

        //employees
        public int emp_id { get; private set; }
        public string name { get; private set; }
        public string phone_number { get; private set; }

        //workinghours
        public int id_wkhrs { get; set; }
        public string workingRN { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public double? minutes { get; set; }
        public string hours { get; set; }
        public double? monthly_salary { get; set; }


        public int Hour_pay { get; set; }
        public string munkaKör { get; private set; }

        public workinghoursData(MySqlDataReader reader)
        {
            this.emp_id = (int)reader["emp_id"];
            this.name = reader["name"].ToString();
            this.phone_number = reader["phone_number"].ToString();

            //workinghours
            this.id_wkhrs = (int)reader["id_wkhrs"];
            this.workingRN = (bool)reader["online"] == false ? "Pihen" : "Dolgozik";
            this.minutes = (double?)reader["minutes"] == null ? 0 : (double?)reader["minutes"];
            this.hours = (minutes / 60).ToString();
            this.from_date = reader["from_date"] == DBNull.Value ? null : (DateTime?)reader["from_date"];
            this.to_date = reader["to_date"] == DBNull.Value ? null : (DateTime?)reader["to_date"];
            

            //jobs
            this.munkaKör = reader["job_title"].ToString();
            this.Hour_pay = (int)reader["job_salary"];
        }
    }
}
