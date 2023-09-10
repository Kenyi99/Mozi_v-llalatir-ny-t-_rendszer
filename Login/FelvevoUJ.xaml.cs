using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Login
{
    /// <summary>
    /// Interaction logic for FelvevoUJ.xaml
    /// </summary>
    public partial class FelvevoUJ : Window
    {
        private DolgozoData Alany;

        public FelvevoUJ(DolgozoData d = null)
        {                
            InitializeComponent();
            cbMunka.ItemsSource = DataManager.job_title();
            cbGender.ItemsSource = DataManager.gender();
            if (d != null)
            {
                Alany = d;
                txtNev.Text = d.name;
                dtSzuletes.SelectedDate = d.birth_date;
                txtPhone.Text = d.phone_number.ToString();
                cbGender.SelectedItem = d.gender;
                dtFelvétel.SelectedDate = d.hire_date;
                txtAdó.Text = d.tax_number.ToString();
                txtTaj.Text = d.taj_number.ToString();
                txtEmail.Text = d.mail;
                cbMunka.SelectedItem = d.munkaKör;
                txtAddress.Text = d.address;
            }
        }

        private void felvesz_Click(object sender, RoutedEventArgs e)
        {
            string error = AzEllenőr();
            if (error.Equals(""))
            {
                if (Alany == null) //Ekkor új adat felvétele történik
                {
                    
                    DataManager.Insert(txtNev.Text, txtPhone.Text, dtSzuletes.SelectedDate.Value, cbGender.Text, dtFelvétel.SelectedDate.Value, Convert.ToInt64(txtAdó.Text), Convert.ToInt64(txtTaj.Text), txtEmail.Text, cbMunka.Text, txtAddress.Text);
                    this.DialogResult = true;
                    this.Close();
                    
                }

                else //Ekkor módosítás történik
                {

                    DataManager.Update(txtNev.Text, txtPhone.Text, dtSzuletes.SelectedDate.Value, cbGender.Text, dtFelvétel.SelectedDate.Value, Convert.ToInt64(txtAdó.Text), Convert.ToInt64(txtTaj.Text), txtEmail.Text, cbMunka.Text, txtAddress.Text, Alany.emp_id);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show($"{error}", "HIBA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private string AzEllenőr()
        {           
            if (string.IsNullOrWhiteSpace(txtNev.Text))
            {
                return "Kötelező nevet megadni!";
            }
            if (txtPhone.Text.Length <10)
            {
                return "Nem adta nem helyesen a telefonszámot!";
            }
            if (cbGender.SelectedItem == null)
            {
                return "Válassza ki a személy Nemét!";
            }
            if (txtAdó.Text.Length<9)
            {
                return "Adószám nem elég hosszú";
            }
            if (txtTaj.Text.Length <9)
            {
                return "Taj szám nem elég hosszú";
            }
            if (cbMunka.SelectedItem == null)
            {
                return "Válasszon ki a személy Munkakörét!";
            }          
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains('@') || !txtEmail.Text.Contains('.'))
            {
                return "Addjon meg email!";
            }
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                return "Addja meg a lakcímet!";
            }
            return "";
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
    

