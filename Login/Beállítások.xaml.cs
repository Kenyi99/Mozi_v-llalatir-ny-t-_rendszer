using System;
using System.Collections.Generic;
using System.IO;
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

namespace Login
{
    /// <summary>
    /// Interaction logic for Beállítások.xaml
    /// </summary>
    public partial class Beállítások : UserControl
    {
        public Beállítások()
        {
            InitializeComponent();
            changer.Text = Calendar.DailyChange.ToString();
            user.Text = $"Felhasználóneve:\t {UserInfo.UserName}";
            var users = DataManager.users();
            users.Insert(0, "");
            cbUsers.ItemsSource = users;
            if (DolgozoData.permission == "0")
            {
                permission.Text = $"Jogosultsága:\t Normál";
            }
            else
            {
                permission.Text = $"Jogosultsága:\t Admin";

            }
        }
        private void dailyChanger_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter("DailyChange.txt"))
            {
                sw.Flush();
                sw.WriteLine(changer.Text);
                Calendar.DailyChange = double.Parse(changer.Text);
                
            }
            //Calendar.DailyChange = double.Parse(changer.Text);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void passModify_Click(object sender, RoutedEventArgs e)
        {
                      
            if (passModify.Content.ToString() == "Jelszó módosít")
            {
                passModify.Content = "Ok";
                cbUsers.Visibility = Visibility.Collapsed;
                userDelete.Visibility = Visibility.Collapsed;
                texts.Visibility = Visibility.Visible;
                Email.Visibility = Visibility.Collapsed;
                newUser.Content = "Mégsem";
                MaterialDesignThemes.Wpf.HintAssist.SetHint(passNow, "Régi jelszó:");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(newPass, "Jelszó:");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(newPassAgain, "Jelszó újra:");

            }
            else if (passModify.Content.ToString() == "Mégsem")
            {
                newUser.Content = "Új felhasználó";
                passModify.Content = "Jelszó módosít";
                cbUsers.Visibility = Visibility.Visible;
                userDelete.Visibility = Visibility.Visible;
                texts.Visibility = Visibility.Collapsed;
                Email.Visibility = Visibility.Collapsed;
                passNow.Clear();
                newPass.Clear();
                newPassAgain.Clear();
                Email.Clear();
            }
            else
            {
                string error = AzEllenőr();
                if (error.Equals(""))
                {
                    if (DataManager.passCheck(DataManager.CreateMD5(passNow.Text).ToLower(), UserInfo.UserName))
                    {                       
                        passModify.Content = "Jelszó módosít";
                        newUser.Content = "Új felhasználó";
                        DataManager.newPass(DataManager.CreateMD5(newPass.Password).ToLower(), UserInfo.UserName);
                        user.Text = $"Felhasználóneve:\t {UserInfo.UserName}";
                        var users = DataManager.users();
                        users.Insert(0, "");
                        texts.Visibility = Visibility.Collapsed;
                        newUser.Visibility = Visibility.Visible;
                        cbUsers.Visibility = Visibility.Visible;
                        userDelete.Visibility = Visibility.Visible;
                        passNow.Clear();
                        newPass.Clear();
                        newPassAgain.Clear();
                        Email.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Nem jó az előző jelszó!");
                    }
                }
                else
                {
                    MessageBox.Show($"{error}", "HIBA", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }                     
        }

        private string AzEllenőr()
        {
            if (string.IsNullOrWhiteSpace(passNow.Text) || string.IsNullOrWhiteSpace(newPass.Password) || string.IsNullOrWhiteSpace(newPassAgain.Password))
            {
                return "Kötelező megadni adatokat!";
            }
            if (newPass.Password != newPassAgain.Password)
            {
                return "Nem egyeznek meg a jelszavak!";
            }
            if (!newPass.Password.Any(char.IsDigit))
            {
                return "Jelszó gyenge, tegyen bele számokat!";
            }
            if (newPass.Password.Length < 4)
            {
                return "Nem elég hosszú a jelszó";
            }          
                            
            return "";
        }

        private void newUser_Click(object sender, RoutedEventArgs e)
        {
            
            if (newUser.Content.ToString() == "Új felhasználó")
            {
                newUser.Content = "Ok";
                passModify.Content = "Mégsem";
                cbUsers.Visibility = Visibility.Collapsed;
                userDelete.Visibility = Visibility.Collapsed;
                texts.Visibility = Visibility.Visible;
                Email.Visibility = Visibility.Visible;
                MaterialDesignThemes.Wpf.HintAssist.SetHint(passNow, "Felhasználó név:");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(newPass, "Jelszó:");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(newPassAgain, "Jelszó újra:");
                MaterialDesignThemes.Wpf.HintAssist.SetHint(Email, "Email:");

            }
            else if (newUser.Content.ToString() == "Mégsem")
            {
                newUser.Content = "Új felhasználó";
                passModify.Content = "Jelszó módosít";
                texts.Visibility = Visibility.Collapsed;
                cbUsers.Visibility = Visibility.Visible;
                userDelete.Visibility = Visibility.Visible;
                passNow.Clear();
                newPass.Clear();
                newPassAgain.Clear();
                Email.Clear();
            }
            else
            {

                if (!cbUsers.Items.Contains(passNow.Text))
                {
                    string error = AzEllenőr();
                    if (error.Equals(""))
                    {
                        newUser.Content = "Új felhasználó";
                        passModify.Content = "Jelszó módosít";
                        DataManager.userInsert(passNow.Text, DataManager.CreateMD5(newPass.Password).ToLower(), 1, Email.Text);
                        user.Text = $"Felhasználóneve:\t {UserInfo.UserName}";
                        var users = DataManager.users();
                        users.Insert(0, "");
                        texts.Visibility = Visibility.Collapsed;
                        Email.Visibility = Visibility.Collapsed;
                        cbUsers.Visibility = Visibility.Visible;
                        userDelete.Visibility = Visibility.Visible;
                        passModify.Visibility = Visibility.Visible;
                        passNow.Clear();
                        newPass.Clear();
                        newPassAgain.Clear();
                        Email.Clear();
                    }
                    else
                    {
                        MessageBox.Show($"{error}", "HIBA", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Van már ilyen felhasználó", "HIBA", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void userDelete_Click(object sender, RoutedEventArgs e)
        {
            if (cbUsers.SelectedItem.ToString() != "" && !user.Text.Contains(cbUsers.SelectedItem.ToString()))
            {
                if (MessageBox.Show("Biztos törölni szeretné?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    DataManager.userDelet(cbUsers.Text);
                    var users = DataManager.users();
                    users.Insert(0, "");
                    cbUsers.ItemsSource = users;
                }

            }
            else
            {
                MessageBox.Show("Ezt nem lehet törölni!", "Warning");
            }
        }
    }
}
