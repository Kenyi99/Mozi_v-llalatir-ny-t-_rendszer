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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Login
{
    /// <summary>
    /// Interaction logic for Munkaóra.xaml
    /// </summary>
    public partial class Munkaóra : UserControl
    {
       

        bool dolgozik = true;

        DateTime time = DateTime.Today;

        public Munkaóra()
        {
            InitializeComponent();
            dolgozikmost.Foreground = Brushes.Green;
            dolzozike.Content = "Dolgozik";
            groupby();
        }

        private void dolzozike_Click(object sender, RoutedEventArgs e)
        {
            if (dolgozik.Equals(true))
            {
                dolgozik = false;
                dolzozike.Content = "Pihen";
                dolgozikmost.Foreground = Brushes.Red;
                groupby();
            }
            else
            {
                dolgozik = true;
                dolzozike.Content = "Dolgozik";
                dolgozikmost.Foreground = Brushes.Green;
                groupby();
            }
        }
        public void groupby()
        {
            dgMunkaora.ItemsSource = DataManager.MunkaóraGroup(dolgozik);
        }


        private void bekijelent_Click(object sender, RoutedEventArgs e)
        {
            kibejelento a = new kibejelento();
            a.ShowDialog();
            groupby();
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            if (dgMunkaora.SelectedItem != null)
            {
                MunkaOraAllito M = new MunkaOraAllito((workinghoursData)dgMunkaora.SelectedItem);
                M.ShowDialog();
                
            }
            else
            {
                MessageBox.Show("Nincs kijelölt módítandó adat!");
            }
        }

        private void monthlyClose_Click(object sender, RoutedEventArgs e)
        {           
            if (dgMunkaora.SelectedItem != null)
            {
                workinghoursData data = dgMunkaora.SelectedItem as workinghoursData;
                
                double days = DateTime.DaysInMonth(time.Year, time.Month);
                string lastDayString = $"{time.Year}-{time.Month}-{days}";
                string lastDayString2 = $"{time.Year}-{time.Month}-{days-1}";
                DateTime lastday = DateTime.Parse(lastDayString);
                DateTime lastday2 = DateTime.Parse(lastDayString2);
                if (DateTime.Today == lastday || DateTime.Today == lastday2)
                {
                    double hours = (double)data.minutes / 60;
                    double monthlySalary = data.Hour_pay * hours;
                    DataManager.wages(lastday, (double)data.minutes, monthlySalary, data.id_wkhrs);
                    DataManager.wages(lastday, (double)data.minutes, monthlySalary, data.id_wkhrs);
                    DataManager.montlyClose(data.id_wkhrs);
                }
                else
                {
                    MessageBox.Show("Ma még nem tudod lezárni a bérét!");                    
                }
            }
            else
            {
                MessageBox.Show("Jelöld ki kit szeretnél lezárni!");
            }
        }

        private void monthlyDatas_Click(object sender, RoutedEventArgs e)
        {
            monthlyWageAll M = new monthlyWageAll();
            M.ShowDialog();
        }
    }
}
