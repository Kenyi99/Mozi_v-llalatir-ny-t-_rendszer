using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    class UserInfo
    {
        public static string UserName = "";
        public static string email = "";
        public static string permission = "";

        public int userID { get;private set; }
        public string Username { get;private set; }



        public UserInfo(MySqlDataReader reader)
        {
            this.userID = (int)reader["emp_id"];
            this.Username = reader["name"].ToString();
        }

    }
}
