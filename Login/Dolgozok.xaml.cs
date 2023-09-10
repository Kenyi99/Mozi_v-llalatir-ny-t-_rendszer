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
    /// Interaction logic for Dolgozok.xaml
    /// </summary>
    public partial class DolgozokData : UserControl 
    {

        public DolgozokData()
        {
            InitializeComponent();
            dgDolgozok.ItemsSource = DataManager.dolgozokLista();
            var dolgozoklista = DataManager.job_title();
            dolgozoklista.Insert(0, "");
            cbJobTitle.ItemsSource = dolgozoklista;
         
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgDolgozok.SelectedItem != null)
            {
                if (MessageBox.Show("Biztosan törli?", "Megerősítés", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DataManager.Delete(((DolgozoData)dgDolgozok.SelectedItem).emp_id);
                    MessageBox.Show($"Sikeresen törölve!");
                    dgDolgozok.ItemsSource = DataManager.dolgozokLista();
                }                
            }
            else
            {
                MessageBox.Show("Nincs kijelölt törlendő adat!");
            }
        }

        private void keresBtn_Click(object sender, RoutedEventArgs e)
        {
            dgDolgozok.ItemsSource = DataManager.dolgozokLista(txtNev.Text, cbJobTitle.SelectedValue.ToString());
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            FelvevoUJ f = new FelvevoUJ();
            AdminSurface a = new AdminSurface();
            f.ShowDialog();          
            dgDolgozok.ItemsSource = DataManager.dolgozokLista();

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgDolgozok.SelectedItem != null)
            {
                FelvevoUJ r = new FelvevoUJ((DolgozoData)dgDolgozok.SelectedItem);
                r.ShowDialog();
                dgDolgozok.ItemsSource = DataManager.dolgozokLista();
            }
            else
            {
                MessageBox.Show("Nincs kijelölt módítandó adat!");
            }
        }
    }
}
