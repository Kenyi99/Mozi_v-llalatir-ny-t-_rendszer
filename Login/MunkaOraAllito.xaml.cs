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

namespace Login
{
    /// <summary>
    /// Interaction logic for MunkaOraAllito.xaml
    /// </summary>
    public partial class MunkaOraAllito : Window
    {
        private workinghoursData data;

        public MunkaOraAllito(workinghoursData w = null)
        {
            InitializeComponent();
            data = w;
            txtNev.Text = w.name;
            txtNev.IsEnabled = false;
            online.IsChecked = w.workingRN == "Pihen" ? false : true;
            if (w.from_date != null)
            {
                txtStart.Text = w.from_date.ToString();
            }
            else
            {
                kezdCheck.IsEnabled = true;
            }

            if (w.to_date != null)
            {
                txtEnd.Text = w.to_date.ToString();
                //dtVégez.SelectedDate = w.to_date;
            }
            else
            {
                végzCheck.IsEnabled = true;
            }
        }
        
        private DateTime? start()
        {
            if (kezdCheck.IsChecked == true)
            {
                return null;                              
            }
            else
            {
                return Convert.ToDateTime(txtStart.Text);
            }
        }

        private DateTime? end()
        {
            if (végzCheck.IsChecked == true)
            {
                return null;
            }
            else
            {
                return Convert.ToDateTime(txtEnd.Text);
            }
        }
        private double? hours()
        {
            if (data.minutes == null)
            {
                return null;
            }
            else
            {
                return data.minutes;
            }
        }


        private void végzCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (végzCheck.IsChecked == true)
            {
                txtEnd.IsEnabled = false;
            }
            else
            {
                txtEnd.IsEnabled = true;
            }

        }

        private void kezdCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (kezdCheck.IsChecked == true)
            {
                txtStart.IsEnabled = false;
            }
            else
            {
                txtStart.IsEnabled = true;
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void modify_Click(object sender, RoutedEventArgs e)
        {
            string error = AzEllenőr();
            if (error.Equals(""))
            {
                DataManager.StartOrEnd(data.id_wkhrs.ToString(), start(), end(), hours(), checker());
                this.DialogResult = true;
                this.Close();               
            }
            else
            {
                MessageBox.Show($"{error}", "HIBA", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string AzEllenőr()
        {
            if (string.IsNullOrWhiteSpace(txtStart.Text) && kezdCheck.IsChecked != true)
            {
                return "Pipáld be a kezdés boxot vagy adj meg neki értéket!";
            }
            if ((string.IsNullOrWhiteSpace(txtEnd.Text) && végzCheck.IsChecked != true))
            {
                return "Pipáld be a végzés boxot vagy adj neki értéket!";
            }            
            return "";
        }

        private bool checker()
        {
            if (online.IsChecked == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
