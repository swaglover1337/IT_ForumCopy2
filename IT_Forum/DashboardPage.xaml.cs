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
    /// Логика взаимодействия для DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : Page
    {
        private MainWindow _mainWindow;
        private ITForumDBEntities _db;

        public DashboardPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _db = ITForumDBEntities.GetInst();

            LoadArticlesActivity();
            LoadArticlesStats();
        }

        private void EditorModeButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new EditorPage(_mainWindow));
        }

        private void ModeratorModeButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new ModeratorPage(_mainWindow));
        }

        private void AdminModeButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new AdminPage(_mainWindow));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new LoginPage(_mainWindow));
        }

        private void LoadArticlesActivity()
        {
            var query =
                from a in _db.Articles
                join u in _db.Users on a.AuthorID equals u.UserID
                join s in _db.ArticleStatuses on a.StatusID equals s.StatusID
                orderby a.Date descending
                select new
                {
                    Title = a.Title,
                    AuthorName = u.Username,
                    Status = s.Status,
                    Date = a.Date
                };

            ArticlesActivityListView.Items.Clear();
            ArticlesActivityListView.ItemsSource = query.Take(20).ToList();
        }

        private void LoadArticlesStats()
        {
            // всего
            int total = _db.Articles.Count();

            // на модерации
            int onModeration = _db.Articles.Count(a => a.StatusID == 2);

            //опубликовано
            int published = _db.Articles.Count(a => a.StatusID == 1);

            TotalArticlesTextBlock.Text = "Всего статей: " + total;
            OnModerationTextBlock.Text = "На модерации: " + onModeration;
            PublishedTextBlock.Text = "Опубликовано: " + published;
        }

        private void FilterClick(object sender, RoutedEventArgs e)
        {
            string search = SearchTextBox.Text;

            var query =
                from a in _db.Articles
                join u in _db.Users on a.AuthorID equals u.UserID
                join s in _db.ArticleStatuses on a.StatusID equals s.StatusID
                orderby a.Date descending
                select new
                {
                    Title = a.Title,
                    AuthorName = u.Username,
                    Status = s.Status,
                    Date = a.Date
                };

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Title.Contains(search));
            }

           
            ArticlesActivityListView.ItemsSource = query.Take(20).ToList();
        }
    }
}

