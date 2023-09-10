using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    static class DataManager
    {
        static string ConnectionString = "datasource=127.0.0.1;port=3306;username=root;password=;Initial Catalog=kenyioffice;";
        

        public static ObservableCollection<financy> financy(DateTime time)
        {
            ObservableCollection<financy> pénz = new ObservableCollection<financy>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            string sql = "SELECT dChange, dIncome, dOutcome, mIncome, mOutcome, netIncome FROM financy Where Day = @time;";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@time", time);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pénz.Add(new financy(reader));
            }
            con.Close();
            return pénz;
        }

        public static double monthly()
        {
            DateTime time = DateTime.Today;
            int totaldays = DateTime.DaysInMonth(time.Year, time.Month);      
            double monthlyIncome = 0;
            double monthlyOutcome = 0;

            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();         

            string start = $"{time.Year}-{time.Month}-01";
            DateTime starter = DateTime.Parse(start);

            string end = $"{time.Year}-{time.Month}-{totaldays}";
            DateTime ender = DateTime.Parse(end);

            MySqlCommand cmd = new MySqlCommand("SELECT dIncome, dOutcome FROM financy Where Day BETWEEN @starter AND @ender ;", con);
            cmd.Parameters.AddWithValue("@starter", starter);
            cmd.Parameters.AddWithValue("@ender", ender);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                monthlyIncome += (double)reader["dIncome"];
                monthlyOutcome += (double)reader["dOutcome"];
            }                               
            con.Close();
            double netMonthlyIncome = monthlyIncome - monthlyOutcome;
            return netMonthlyIncome;
        }
        public static int counter()
        {
            int számláló = 0;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT name FROM employees", con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                számláló++;
            }
            con.Close();
            return számláló;
        }


        public static void userInsert(string UserName, string Password, int permission, string email)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `login` (`UserName`, `Password`, `Permission`, `Email`) VALUES (@UserName, @Password, @permission, @email);", con);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@permission", permission);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static ObservableCollection<string> users()
        {
            ObservableCollection<string> users = new ObservableCollection<string>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM login", con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(reader["UserName"].ToString());
            }
            con.Close();
            return users;
        }

        public static void userDelet(string UserName)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM login WHERE UserName = @UserName", con);
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static string CreateMD5(string password)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            MD5 md5 = MD5.Create();
            byte[] hashedBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sb.Append(hashedBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static void newPass(string newpass, string Username)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE login SET Password = @NewPass Where UserName = @Username;", con);
            cmd.Parameters.AddWithValue("@newpass", newpass);
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static bool passCheck(string txtInsert, string username)
        {
            MySqlDataReader reader;
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("Select * from login where UserName=@username;", con);
            cmd.Parameters.AddWithValue("@username", username);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader["Password"].ToString().Equals(txtInsert))
                {
                    return true;
                }
            }
            reader.Close();
            reader.Dispose();
            cmd.Dispose();
            con.Close();
            return false;
        }

        public static void moneyDebit(double dIncome, double dOutcome, double mIncome, double mOutcome, double netIncome, DateTime time)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE financy SET dIncome = @dIncome, dOutcome = @dOutcome, mIncome = @mIncome, mOutcome = @mOutcome, netIncome = @netIncome Where Day = @time;", con);
            cmd.Parameters.AddWithValue("@dIncome", dIncome);
            cmd.Parameters.AddWithValue("@dOutcome", dOutcome);           
            cmd.Parameters.AddWithValue("@mIncome", mIncome);           
            cmd.Parameters.AddWithValue("@mOutcome", mOutcome);           
            cmd.Parameters.AddWithValue("@netIncome", netIncome);
            cmd.Parameters.AddWithValue("@time", time);
            cmd.ExecuteNonQuery();
            con.Close();
        } 

        public static void basicValues(DateTime day, double? dChange, double? dIncome, double? dOutcome, double? mIncome, double? mOutcome, double? netIncome)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();           
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `financy` (`day`, `dChange`, `dIncome`, `dOutcome`, `mIncome`, `mOutcome`, `netIncome`) " +
                "VALUES (@day, @dChange, @dIncome, @dOutcome, @mIncome, @mOutcome, @netIncome);", con);
            cmd.Parameters.AddWithValue("@day", day);
            cmd.Parameters.AddWithValue("@dChange", dChange);
            cmd.Parameters.AddWithValue("@dIncome", dIncome);
            cmd.Parameters.AddWithValue("@dOutcome", dOutcome);
            cmd.Parameters.AddWithValue("@mIncome", mIncome);
            cmd.Parameters.AddWithValue("@mOutcome", mOutcome);
            cmd.Parameters.AddWithValue("@netIncome", netIncome);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public static ObservableCollection<string> job_title()
        {
            ObservableCollection<string> munkák = new ObservableCollection<string>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT job_title FROM jobs", con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                munkák.Add(reader["job_title"].ToString());
            }
            con.Close();
            return munkák;
        }

        public static ObservableCollection<string> gender()
        {
            var nemek = new ObservableCollection<string>() { "Férfi", "Nő" };
            return nemek;
        }

        public static ObservableCollection<DolgozoData> dolgozokLista(string nev="", string jobTitle="")
        {
            ObservableCollection <DolgozoData> dolgozok = new ObservableCollection<DolgozoData>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            var munkák = job_title();
            con.Open();
            string sql = "SELECT e.emp_id, e.birth_date, e.name, e.phone_number, e.gender, e.hire_date, e.tax_number, e.taj_number, j.job_title, e.mail, e.address FROM employees e INNER JOIN" +
                         " jobs j ON j.job_id = e.jobs_Title_ID  Where name LIKE CONCAT('%', @name, '%') AND j.job_title LIKE CONCAT('%', @job_title);";
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@name", nev);
            cmd.Parameters.AddWithValue("@job_title", jobTitle);
            MySqlDataReader reader = cmd.ExecuteReader();           
            while (reader.Read())
            {
                dolgozok.Add(new DolgozoData(reader));
            }
            con.Close();
            return dolgozok;
        }

        public static ObservableCollection<workinghoursData> MunkaóraGroup(bool online)
        {
            var munkák = job_title();
            ObservableCollection<workinghoursData> munkaórák = new ObservableCollection<workinghoursData>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            string sql = "SELECT e.emp_id, e.name, e.phone_number, j.job_title, j.job_salary, w.id_wkhrs, w.online, w.from_date, w.to_date FROM employees e INNER JOIN" +
                         " jobs j ON j.job_id = e.jobs_Title_ID INNER JOIN workinghours w ON w.id_wkhrs = e.emp_id Where w.online = @online;"; // w.minutes, 
            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@online", online);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                munkaórák.Add(new workinghoursData(reader));
            }
            con.Close();
            return munkaórák;
        }

        public static void Delete(int id)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM employees WHERE emp_id = @emp_id", con);
            cmd.Parameters.AddWithValue("@emp_id", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void Insert(string Nev, string telefonszám, DateTime Szuletes, string gender, DateTime felvétel ,long adó_szám, long tajszám, string mail, string munka, string cim)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            var munkaID = Job_ID(munka);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `employees` (`name`, `phone_number`, `birth_date`, `gender`, `hire_date`, `tax_number`, `taj_number`, `mail`, `jobs_Title_ID`, `address`)" +
                "VALUES (@name, @phone_number, @birth_date, @gender, @hire_date, @tax_number, @taj_number, @mail, @jobs_Title_ID, @cim);", con);
            cmd.Parameters.AddWithValue("@name", Nev);
            cmd.Parameters.AddWithValue("@phone_number", telefonszám);
            cmd.Parameters.AddWithValue("@birth_date", Szuletes);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@hire_date", felvétel);
            cmd.Parameters.AddWithValue("@tax_number", adó_szám);
            cmd.Parameters.AddWithValue("@taj_number", tajszám);
            cmd.Parameters.AddWithValue("@mail", mail);
            cmd.Parameters.AddWithValue("@jobs_Title_ID", munkaID);
            cmd.Parameters.AddWithValue("@cim", cim);
            cmd.ExecuteNonQuery();
            InsertHR(Nev);
            con.Close();
        }
        public static void InsertHR(string name)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `workinghours` (`id_wkhrs`, `from_date`, `to_date`, `minutes`, `online`) VALUES((select emp_id from employees where name=@name), NULL, NULL, 0, '0');", con);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void montlyClose(int id)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE workinghours SET from_date=null, to_date=null, minutes=0, online=0 WHERE id_wkhrs=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static object Job_ID(string Munkakör)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT job_id FROM jobs WHERE job_title=@Munkakör", con);
            cmd.Parameters.AddWithValue("@Munkakör", Munkakör);
            MySqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            var MunkaID = reader["job_id"];
            con.Close();
            return MunkaID;
        }
        public static void Update(string Nev, string telefonszám, DateTime Szuletes, string gender, DateTime felvétel, long adó_szám, long tajszám, string email, string munka, string cim, int id)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            var munkaID = Job_ID(munka);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE employees SET name=@name, phone_number=@phone_number, birth_date=@birth_date, gender=@gender, hire_date=@hire_date, tax_number=@tax_number, taj_number=@taj_number, mail=@mail, jobs_Title_ID=@jobs_Title_ID, address=@cim WHERE emp_id=@id", con);
            cmd.Parameters.AddWithValue("@name", Nev);
            cmd.Parameters.AddWithValue("@phone_number", telefonszám);
            cmd.Parameters.AddWithValue("@birth_date", Szuletes);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@hire_date", felvétel);
            cmd.Parameters.AddWithValue("@tax_number", adó_szám);
            cmd.Parameters.AddWithValue("@taj_number", tajszám);
            cmd.Parameters.AddWithValue("@mail", email);
            cmd.Parameters.AddWithValue("@jobs_Title_ID", munkaID);
            cmd.Parameters.AddWithValue("@cim", cim);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public static void StartOrEnd(string ID, DateTime? from, DateTime? to, double? minutes, bool online)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE workinghours SET from_date=@from, to_date=@to, minutes=@minutes, online=@online WHERE id_wkhrs=@ID", con);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);
            cmd.Parameters.AddWithValue("@minutes", minutes);
            cmd.Parameters.AddWithValue("@online", online);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void wages(DateTime date, double minutes, double monthly_salary,int employee_id)
        {
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO `wages` (`day`, `Hours_monthly`, `monthly_salary`, `employee_id`) VALUES (@date, @minutes, @monthly_salary, @employee_id);", con);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@minutes", minutes);
            cmd.Parameters.AddWithValue("@monthly_salary", monthly_salary);
            cmd.Parameters.AddWithValue("@employee_id", employee_id);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static ObservableCollection<AllWages> allWage()
        {
            ObservableCollection<AllWages> bérek = new ObservableCollection<AllWages>();
            MySqlConnection con = new MySqlConnection(ConnectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT e.name, w.day, w.Hours_monthly, w.monthly_salary FROM wages w INNER JOIN employees e ON w.employee_id = e.emp_id ", con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                bérek.Add(new AllWages(reader));
            }
            con.Close();
            return bérek;
        }

    }
}
