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
    /// Interaction logic for kibejelento.xaml
    /// </summary>
    public partial class kibejelento : Window
    {
        public kibejelento()
        {
            InitializeComponent();
            
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool van = false;         
            var lista = DataManager.dolgozokLista();
            var HoursListMain = DataManager.MunkaóraGroup(false).Union(DataManager.MunkaóraGroup(true)).ToList();
            //var HourselistMain = DataManager.MunkaóraGroup(true);
            //var HourseListTemporary = DataManager.MunkaóraGroup(false);
            //foreach (var item in HourseListTemporary)
            //{
            //    HourselistMain.Add(item);
            //}           
            foreach (var item in lista)
            {
                if (item.emp_id.ToString() == bevitelID.Text) // van ilyen employee szóval mehet tovább
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
                            this.Close();
                        }
                        else if (item.emp_id == main.id_wkhrs && main.workingRN == "Dolgozik")
                        {
                            DateTime from_working = (DateTime)main.from_date;
                            DateTime myDateTime = DateTime.Now;
                            string Minutes = (myDateTime - from_working).TotalMinutes.ToString("f4");                            
                            DataManager.StartOrEnd(main.id_wkhrs.ToString(), from_working, myDateTime, double.Parse(Minutes), false);
                            MessageBox.Show($"{main.name} -nak/nek le lett állítva az óraszámlálója {myDateTime}");
                            van = true;
                            this.Close();
                        }
                        
                    }                   
                    //string ID = "asd";
                    //DataManager.StartOrEnd(ID);                   
                    break;                    
                }               
            }
            if (!van)
            {
                MessageBox.Show("Nincs ilyen dolgozó!\nEllenőrizze hogy helyesen írta e be a kódot!");
                //folytatni!
            }
            
        }

    }
}
