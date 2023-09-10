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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Login
{
    /// <summary>
    /// Interaction logic for AdminSurface.xaml
    /// </summary>
    public partial class AdminSurface : Window
    {
        //public static event EventHandler<EventArgs> ClosingWindows;

        bool MenuClosed = false;

        public AdminSurface()
        {
            InitializeComponent();
            Calendar.reader();
            
            if (UserInfo.UserName == "")
            {
                DolgozoData.UserName = "Szervíz";
                Felhasználó.Text = DolgozoData.UserName;
            }
            else
            {
                Felhasználó.Text = UserInfo.UserName;  
               
            }
            
        }
     
        public void DisplayUserControl(UserControl uc)
        {
            // Add new user control to content area
            GridPrincipal.Children.Add(uc);
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (50 + (60 * index)), 0, 0);
        }     

        public void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Kezdolap());
                    break;
                case 1:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new DolgozokData());
                    break;
                case 2:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Munkaóra());
                    break;
                case 3:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Calendar());
                    break;
                case 4:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Kapcsolatok());
                    break;
                case 5:
                    GridPrincipal.Children.Clear();
                    GridPrincipal.Children.Add(new Beállítások());
                    break;               
                default:
                    break;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
            
        private void clores_Click(object sender, RoutedEventArgs e)
        {
            if (MenuClosed)
            {
                Storyboard openMenu = (Storyboard)MenuCollapser.FindResource("OpenMenu");
                openMenu.Begin();
                záró.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowBack;
            }
            else
            {
                Storyboard closeMenu = (Storyboard)MenuCollapser.FindResource("CloseMenu");
                closeMenu.Begin();
                záró.Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowForward;
            }

            MenuClosed = !MenuClosed;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DolgozoData.UserName = "";
            DolgozoData.permission = "";
            LoginScreen l = new LoginScreen();
            l.ShowDialog();
            this.Close();
        }

        private void upperSettings_Click(object sender, RoutedEventArgs e)
        {
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new Beállítások());
        }
    }
}
