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
using System.Windows.Threading;

namespace Login
{
    /// <summary>
    /// Interaction logic for Kezdolap.xaml
    /// </summary>
    public partial class Kezdolap : UserControl
    {

        public Kezdolap()
        {
            InitializeComponent();
            txtMonthly.Text = $"Havi bevétel: {DataManager.monthly()}";
            txtCount.Text = $"Dolgozók száma: {DataManager.counter()}";
            dgWorkingRN.ItemsSource = DataManager.MunkaóraGroup(true);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();          
        }

        void timer_Tick(object sender, EventArgs e)
        {
            lblTime.Content = DateTime.Now.ToLongTimeString();
        }



    }
}
