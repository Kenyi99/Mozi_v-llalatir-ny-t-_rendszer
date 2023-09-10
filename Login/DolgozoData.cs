using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Login
{
    public class DolgozoData
    {
        //login
        public static string UserName = "";
        public static string permission = "";      

        //dolgozok
        public int emp_id { get;private set; }
        public DateTime birth_date { get;private set; }
        public string name { get;private set; }
        public string phone_number { get;private set; } 
        public string gender { get;private set; }
        public DateTime hire_date { get; private set; }
        public long tax_number { get; private set; }
        public long taj_number { get; private set; }
        public string mail { get;private set; }
        public string address { get;private set; }

        //jobs
        public string job { get; private set; }
        public string munkaKör { get; private set; }

        //salaries            
        public int work_hours_mounthly { get; set; }       
        public int salary { get; set; }
        public int total_salary { get; set; }

        

        public DolgozoData(MySqlDataReader reader)
        {
            //emloyees
            this.emp_id = (int)reader["emp_id"];
            this.birth_date = Convert.ToDateTime(reader["birth_date"]);          
            this.name = reader["name"].ToString();
            this.phone_number = reader["phone_number"].ToString();
            this.gender = reader["gender"].ToString();
            this.hire_date = Convert.ToDateTime(reader["hire_date"]);
            this.mail = reader["mail"].ToString();
            this.tax_number = (long)reader["tax_number"];
            this.taj_number = (long)reader["taj_number"];
            this.address = reader["address"].ToString();
            
            //jobs
            this.munkaKör = reader["job_title"].ToString();
        }
    }
}
