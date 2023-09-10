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
    /// Interaction logic for Kapcsolatok.xaml
    /// </summary>
    public partial class Kapcsolatok : UserControl
    {
        public Kapcsolatok()
        {
            
            InitializeComponent();
            dgContact.ItemsSource = DataManager.dolgozokLista();
            var dolgozoklista = DataManager.job_title();
            dolgozoklista.Insert(0, "");
            cbJobTitle.ItemsSource = dolgozoklista;
        }

        private void keresBtn_Click(object sender, RoutedEventArgs e)
        {
            dgContact.ItemsSource = DataManager.dolgozokLista(txtNev.Text, cbJobTitle.SelectedValue.ToString());
        }
    }
}
