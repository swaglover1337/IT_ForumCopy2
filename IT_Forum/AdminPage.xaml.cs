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
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private MainWindow _mainWindow;
        private ITForumDBEntities _db;

        public AdminPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _db = ITForumDBEntities.GetInst();

            LoadUsers();
            LoadAllUsers();
            LoadPartners();
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new DashboardPage(_mainWindow));
        }

        private void EditorModeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new EditorPage(_mainWindow));
        }

        private void ModeratorModeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new ModeratorPage(_mainWindow));
        }

        private void LoadUsers()
        {
            var list =
                from u in _db.Users
                where u.UserID <= 5 
                       && u.UserID > 0
                orderby u.Username
                select new
                {
                    UserName = u.Username,
                    RoleName = "Сотрудник" 
                };

            UsersListView.ItemsSource = list.ToList();
        }

        private void LoadAllUsers()
        {
            var list =
                from u in _db.Users
                orderby u.Username
                select new
                {
                    Username = u.Username,
                    Email = u.Email,
                    CreationDate = u.CreationDate
                };

            StaffActivityListView.ItemsSource = list.ToList();
        }

        private void LoadPartners()
        {
            var list =
                from p in _db.Partners
                orderby p.PartnerName
                select new
                {
                    PartnerName = p.PartnerName,
                    ContactInfo = p.ContactInfo ?? "Нет контактов" 
                };

            PartnersListView.ItemsSource = list.ToList();
        }
    }
}
