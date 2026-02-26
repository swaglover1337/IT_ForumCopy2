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
    /// Логика взаимодействия для EditorPage.xaml
    /// </summary>
    public partial class EditorPage : Page
    {        
            private MainWindow _mainWindow;
            private ITForumDBEntities _db;

            private Articles _selectedArticle; //текущая выбранная

        public EditorPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;

            _db = ITForumDBEntities.GetInst();

            LoadPendingArticles(); //ПЕРЕЗАГРУЗКА СТАТЕЙ
            LoadReviewArticles();
        }


        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new DashboardPage(_mainWindow));
        }

        private void GoodClick(object sender, RoutedEventArgs e)
        {
            if (_selectedArticle == null)
            {
                MessageBox.Show("Выберите статью");
                return;
            }

            _selectedArticle.StatusID = 1;
            _db.SaveChanges();
            MessageBox.Show("Статья успешно одобрена");

            LoadPendingArticles(); 
            LoadReviewArticles();

        }

        private void BadClick(object sender, RoutedEventArgs e)
        {
            if (_selectedArticle == null)
            {
                MessageBox.Show("Выберите статью");
                return;
            }

            _selectedArticle.StatusID = 3;
            _db.SaveChanges();
            MessageBox.Show("Статья успешно отклонена");

            LoadPendingArticles();
            LoadReviewArticles();
        }

        private void NormClick(object sender, RoutedEventArgs e)
        {
            if (_selectedArticle == null)
            {
                MessageBox.Show("Выберите статью");
                return;
            }

            _selectedArticle.StatusID = 2;
            _db.SaveChanges();
            MessageBox.Show("Статья возвращена на проверку");
            LoadPendingArticles();
            LoadReviewArticles();
        }

        private void ClearArticleDetails()
        {
            _selectedArticle = null;
            ArticleTitleTextBox.Text = string.Empty;
            ArticleAuthorTextBox.Text = string.Empty;
            ArticleContentTextBox.Text = string.Empty;
        }

        private void LoadReviewArticles()
        {
            var query =
                from a in _db.Articles
                join u in _db.Users on a.AuthorID equals u.UserID
                join s in _db.ArticleStatuses on a.StatusID equals s.StatusID
                where a.StatusID == 1 || a.StatusID == 3  // одобрено ИЛИ отклонено
                orderby a.Date descending
                select new
                {
                    Article = a,
                    Title = a.Title,
                    AuthorName = u.Username,
                    Status = s.Status
                };

            ReviewArticlesListView.ItemsSource = query.ToList();
        }

        private void ReviewArticlesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReviewArticlesListView.SelectedItem == null)
            {
                return;
            }

            PendingArticlesListView.SelectedItem = null;

            dynamic item = ReviewArticlesListView.SelectedItem;
            _selectedArticle = item.Article as Articles;

            if (_selectedArticle != null)
            {
                ArticleTitleTextBox.Text = _selectedArticle.Title;

                var author = _db.Users.FirstOrDefault(u => u.UserID == _selectedArticle.AuthorID);
                ArticleAuthorTextBox.Text = author != null ? author.Username : "-";

                ArticleContentTextBox.Text = _selectedArticle.Content;
            }
        }

        private void LoadPendingArticles()
        {
            int moderationStatusId = 2;

            var query =
            from a in _db.Articles
            join u in _db.Users on a.AuthorID equals u.UserID
            where a.StatusID == moderationStatusId
            orderby a.Date descending
            select new
            {
                Article = a,
                Title = a.Title,
                AuthorName = u.Username
            };

            PendingArticlesListView.ItemsSource = query.ToList();
            ClearArticleDetails();
        }

        private void PendingArticlesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PendingArticlesListView.SelectedItem == null)
            {
                ClearArticleDetails();
                return;
            }
            dynamic item = PendingArticlesListView.SelectedItem;
            _selectedArticle = item.Article as Articles;

            if (_selectedArticle != null)
            {
                ArticleTitleTextBox.Text = _selectedArticle.Title;

                var author = _db.Users.FirstOrDefault(u => u.UserID == _selectedArticle.AuthorID);
                ArticleAuthorTextBox.Text = author != null ? author.Username : "-";

                ArticleContentTextBox.Text = _selectedArticle.Content;
            }
        }

    }

}

