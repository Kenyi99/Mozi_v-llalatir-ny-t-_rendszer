using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Login
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : UserControl
    {
        public static int days = 0;
        DateTime time = DateTime.Today;
        bool first = false;
        ObservableCollection<financy> current = new ObservableCollection<financy>();

        //public static double DailyChange = 70000;
        public static double DailyChange { get; set; }

        double dIncome = 0;
        double dOutcome = 0;
        double mIncome = 0;
        double mOutcome = 0;
        double netIncome = 0;

        public Calendar()
        {
            InitializeComponent();          
            if (!first)
            {
                calendar123.SelectedDate = time;
                current = DataManager.financy(time);
                days = DateTime.DaysInMonth(time.Year, time.Month);
                mIncome = DataManager.monthly();
                btnCheck();
                refresher();
                calculator();
                
                first = true;
            }
            else
            {
                calculator();
            }

        }
        public static void reader()
        {
            try
            {
                using (StreamReader sr = File.OpenText("DailyChange.txt"))
                {
                    DailyChange = double.Parse(sr.ReadLine());
                }
            }
            catch (FileNotFoundException)
            {
                using (StreamWriter sw = new StreamWriter("DailyChange.txt"))
                {
                    sw.WriteLine("70000");
                }               
            }
        }

        private void calculator()
        {
            netIncome = dIncome - dOutcome;
            tb6.Text = netIncome.ToString();
        }

        private void btnCheck()
        {
            if (current.Count == 0)
            {         
                btn3.IsEnabled = false;
                btn2.IsEnabled = false;
               
            }
            else
            {
                btn3.IsEnabled = true;
                btn2.IsEnabled = true;
               
            }
        }

        private void DateChanger(object sender, SelectionChangedEventArgs e)
        {                   
            if (calendar123.SelectedDate.HasValue)
            {
                time = calendar123.SelectedDate.Value;               
                current = DataManager.financy(time);
                if (current.Count == 0)
                {
                    btnCheck();
                    tb1.Text = "Üres az adatbázis!";
                    tb2.Text = "Üres az adatbázis!";
                    tb3.Text = "Üres az adatbázis!";
                    tb4.Text = "Üres az adatbázis!";
                    tb6.Text = "Üres az adatbázis!";
                }
                else
                {
                    refresher();
                    btnCheck();
                    calculator();
                }
            }
        }
       
        private void refresher()
        {
            foreach (var item in current)
            {
                tb1.Text = item.dChange.ToString();
                

                tb2.Text = item.dIncome.ToString();
                dIncome = (double)item.dIncome;

                tb3.Text = item.dOutcome.ToString();
                dOutcome = (double)item.dOutcome;

                mIncome = DataManager.monthly();
                tb4.Text = mIncome.ToString();
                
                netIncome = (double)item.netIncome;

                
            }
        }

        private void today_Click(object sender, RoutedEventArgs e)
        {
            calendar123.SelectedDate = DateTime.Today;
            time = DateTime.Today;
            current = DataManager.financy(time);
            btnCheck();
            refresher();
            calculator();
        }

        private void valueInsert_Click(object sender, RoutedEventArgs e)
        {                                
            if (current.Count == 0)
            {
                time = calendar123.SelectedDate.Value;
                
                for (int i = 1; i <= days; i++)
                {
                    string add = $"{time.Year}-{time.Month}-{i}";
                    DateTime counter = DateTime.Parse(add);
                    DataManager.basicValues(counter, DailyChange, 0, 0, 0, 0, 0);
                }
                current = DataManager.financy(time);
                btnCheck();
                refresher();
                calculator();
            }
            else
            {
                MessageBox.Show("Vannak adok bevidve!");
            }
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (btn2.Content.ToString() == "bevitel")
            {
                tb2.Visibility = Visibility.Hidden;
                tbw2.Visibility = Visibility.Visible;
                btn2.Content = "OK";
            }
            else
            {
                btn2.Content = "bevitel";
                tb2.Visibility = Visibility.Visible;
                tbw2.Visibility = Visibility.Hidden;
                if (tbw2.Text == "")
                    dIncome += 0;
                else
                    dIncome += double.Parse(tbw2.Text);

                DataManager.moneyDebit(dIncome, dOutcome, mIncome, mOutcome, netIncome, time);
                calculator();
                tb2.Text = dIncome.ToString();
                tbw2.Clear();
            }
        }
       

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            if (btn3.Content.ToString() == "bevitel")
            {
                tb3.Visibility = Visibility.Hidden;
                tbw3.Visibility = Visibility.Visible;
                btn3.Content = "OK";
            }
            else
            {
                btn3.Content = "bevitel";
                tb3.Visibility = Visibility.Visible;
                tbw3.Visibility = Visibility.Hidden;
                if (tbw3.Text == "")
                    dOutcome += 0;
                else
                    dOutcome += double.Parse(tbw3.Text);

                DataManager.moneyDebit(dIncome, dOutcome, mIncome, mOutcome, netIncome, time);
                calculator();
                tb3.Text = dOutcome.ToString();
                tbw3.Clear();
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
