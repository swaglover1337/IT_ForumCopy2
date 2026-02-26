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

namespace IT_Forum
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private MainWindow _mainWindow;

        public LoginPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (login == "admin" && password == "admin")
            {
                _mainWindow.MainFrame.Navigate(new AdminPage(_mainWindow));
            }
            else if (login == "redactor" && password == "redactor")
            {
                _mainWindow.MainFrame.Navigate(new EditorPage(_mainWindow));
            }
            else if (login == "moderator" && password == "moderator")
            {
                _mainWindow.MainFrame.Navigate(new ModeratorPage(_mainWindow));
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка входа",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

 }
