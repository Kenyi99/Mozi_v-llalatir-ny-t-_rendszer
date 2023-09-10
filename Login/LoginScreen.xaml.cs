using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace Login
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {

        MySqlConnection con;
        MySqlCommand cmd;
        MySqlDataReader reader;
        static string ConnectionString = "datasource=127.0.0.1;port=3306;username=root;password=;Initial Catalog=kenyioffice;";
        static string databasetrier = "datasource=127.0.0.1;port=3306;username=root;password=;";
        /// <summary>
        /// ///////////////////////////////////////////////////////
        /// </summary>
        public LoginScreen()
        {
            InitializeComponent();
            databaseTest();
        }

        private void databaseTest()
        {
            con = new MySqlConnection(databasetrier);
            con.Open();
            string s0 = "CREATE DATABASE IF NOT EXISTS `KenyiOffice`;";
            cmd = new MySqlCommand(s0, con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            con.Dispose();

            con = new MySqlConnection(ConnectionString);
            con.Open();
            using (cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS `login` (" +
              "`UserID` INT(11) AUTO_INCREMENT, " +
              "`UserName` VARCHAR(50) NOT NULL, " +
              "`Password` LONGTEXT NOT NULL, " +
              "`Permission` INT(11) UNSIGNED NOT NULL, " +
              "`Email` VARCHAR(255) NOT NULL, " +
              "PRIMARY KEY(UserID));", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            using (cmd = new MySqlCommand("insert into login (`UserName`, `Password`, `Permission`, `Email`) Select 'admin', md5('admin'), '1', 'admin@admin.com' Where not exists(select * from login where UserName='admin');", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose(); 
            }
            using (cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS `jobs`(" +
              "`job_id` INT(11) AUTO_INCREMENT, " +
              "`job_title` VARCHAR(50) NOT NULL, " +
              "`job_salary` INT(11) NOT NULL, " +
              "PRIMARY KEY(job_id))", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            using (cmd = new MySqlCommand("insert into jobs (`job_title`, `job_salary`) Select 'Büfés', 1200 Where not exists(select * from jobs where job_title='Büfés');", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            using (cmd = new MySqlCommand("insert into jobs (`job_title`, `job_salary`) Select 'Manager', 1500 Where not exists(select * from jobs where job_title='Manager');", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            using (cmd = new MySqlCommand("insert into jobs (`job_title`, `job_salary`) Select 'Kasszás', 1300 Where not exists(select * from jobs where job_title='Kasszás');", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            using (cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS `financy`(" +
              "`Day` DATE NOT NULL, " +
              "`dChange` DOUBLE DEFAULT NULL, " +
              "`dIncome` DOUBLE DEFAULT NULL, " +
              "`dOutcome` DOUBLE DEFAULT NULL, " +
              "`mIncome` DOUBLE DEFAULT NULL, " +
              "`mOutcome` DOUBLE DEFAULT NULL, " +
              "`netIncome` DOUBLE DEFAULT NULL, " +
              "PRIMARY KEY(Day))", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }

            using (cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS `employees` (" +
              "`emp_id` INT(11) AUTO_INCREMENT, " +
              "`name` VARCHAR(50) NOT NULL, " +
              "`phone_number` VARCHAR(12) DEFAULT NULL, " +
              "`birth_date` DATE NOT NULL, " +
              "`gender` VARCHAR(50) NOT NULL, " +
              "`hire_date` DATE NOT NULL," +
              "`tax_number` BIGINT(20) DEFAULT NULL, " +
              "`taj_number` BIGINT(20) NOT NULL, " +
              "`jobs_Title_ID` INT(11) DEFAULT NULL, " +
              "`mail` VARCHAR(50) DEFAULT NULL, " +
              "`address` VARCHAR(100) DEFAULT NULL, " +
              "PRIMARY KEY(emp_id))", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            using (cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS `wages` (" +
              "day DATE NOT NULL, " +
              "Hours_monthly DOUBLE NOT NULL, " +
              "monthly_salary DOUBLE NOT NULL," +
              "employee_id INT(11) NOT NULL," +
              "PRIMARY KEY(day))", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            using (cmd = new MySqlCommand("CREATE TABLE IF NOT EXISTS `workinghours` (" +
              "id_wkhrs INT(11) NOT NULL, " +
              "from_date DATETIME(6) DEFAULT NULL, " +
              "to_date DATETIME(6) DEFAULT NULL, " +
              "hours DOUBLE DEFAULT NULL, " +
              "online TINYINT(1) NOT NULL DEFAULT 0, " +
              "PRIMARY KEY(id_wkhrs))", con))
            {
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }       
            con.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (btnLogin.Content.ToString() == "Belép")
            {
                String message = "Biztos jók az adatok?!";
                try
                {
                    con = new MySqlConnection(ConnectionString);
                    con.Open();
                    cmd = new MySqlCommand("Select * from login where UserName=@UserName ", con);
                    cmd.Parameters.AddWithValue("@UserName", txtUsername.Text.ToString());
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader["Password"].ToString().Equals(DataManager.CreateMD5(txtPassword.Password).ToLower()))
                        {
                            UserInfo.UserName = txtUsername.Text.ToString();
                            UserInfo.permission = reader["Permission"].ToString();
                            UserInfo.email = reader["Email"].ToString();
                            if (UserInfo.permission == "1")
                            {
                                message = "1";
                            }
                            else
                            {
                                message = "0";
                            }
                        }
                    }

                    reader.Close();
                    reader.Dispose();
                    cmd.Dispose();
                    con.Close();

                }
                catch (Exception ex)
                {
                    message = ex.Message.ToString();
                }
                if (message == "1")
                {
                    AdminSurface AdminSurface = new AdminSurface();
                    AdminSurface.Show();
                    this.Close();
                }
                else if (message == "0")
                {
                    MessageBox.Show("Ez a funkció még nem elérhető!");
                }
                else
                    MessageBox.Show(message, "Info");
            }
            ////////////////////////////////////////////////////////////////BEVITEL/////////////////////////////////////////////////////////////////////////////
            else
            {
                bool van = false;
                var lista = DataManager.dolgozokLista();
                var HoursListMain = DataManager.MunkaóraGroup(false).Union(DataManager.MunkaóraGroup(true)).ToList();          
                foreach (var item in lista)
                {
                    if (item.emp_id.ToString() == txtWage.Text) // van ilyen employee szóval mehet tovább
                    {
                        // vagy lezár vagy elindítja a számlálóját a személynek!!!
                        foreach (var main in HoursListMain)
                        {
                            if (item.emp_id == main.id_wkhrs && main.workingRN == "Pihen")
                            {
                                DateTime myDateTime = DateTime.Now;
                                string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                                DataManager.StartOrEnd(main.id_wkhrs.ToString(), myDateTime, null, main.minutes.Value, true);
                                MessageBox.Show($"{main.name} -nak/nek el lett indítva az óraszámlálója {myDateTime}");
                                van = true;
                                txtWage.Clear();
                            }
                            else if (item.emp_id == main.id_wkhrs && main.workingRN == "Dolgozik")
                            {
                                DateTime from_working = (DateTime)main.from_date;
                                DateTime myDateTime = DateTime.Now;
                                string Minutes = (myDateTime - from_working).TotalMinutes.ToString("f4");
                                DataManager.StartOrEnd(main.id_wkhrs.ToString(), from_working, myDateTime, double.Parse(Minutes), false);
                                MessageBox.Show($"{main.name} -nak/nek le lett állítva az óraszámlálója {myDateTime}");
                                van = true;
                                txtWage.Clear();
                            }
                        }               
                        break;
                    }
                }
                if (!van)
                {
                    MessageBox.Show("Nincs ilyen dolgozó!\nEllenőrizze hogy helyesen írta e be a kódot!");
                }
            }
        }       

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (changerBtn.Content.ToString() == "munka")
            {
                changerBtn.Content = "belépés";
                logintxt.Text = "Bér indító";
                btnLogin.Content = "bevitel";
                pass.Visibility = Visibility.Collapsed;
                user.Visibility = Visibility.Collapsed;
                wage.Visibility = Visibility.Visible;               
            }
            else
            {
                changerBtn.Content = "munka";
                logintxt.Text = "Belépés";
                btnLogin.Content = "Belép";
                pass.Visibility = Visibility.Visible;
                user.Visibility = Visibility.Visible;
                wage.Visibility = Visibility.Collapsed;
            }
        }
    }
}
