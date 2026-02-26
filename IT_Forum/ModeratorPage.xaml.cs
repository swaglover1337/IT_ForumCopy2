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
    /// Логика взаимодействия для ModeratorPage.xaml
    /// </summary>
    public partial class ModeratorPage : Page
    {
        private MainWindow _mainWindow;
        private ITForumDBEntities _db;
        private Comments _selectedComment;

        public ModeratorPage(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _db = ITForumDBEntities.GetInst();

            LoadComments();
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Navigate(new DashboardPage(_mainWindow));
        }

        private void LoadComments()
        {
            var list =
                from c in _db.Comments
                join u in _db.Users on c.UserID equals u.UserID
                join a in _db.Articles on c.ArticleID equals a.ArticleID
                where c.IsDeleted == false
                orderby c.Date descending
                select new
                {
                    Comment = c,
                    UserName = u.Username,
                    ArticleTitle = a.Title,
                    ShortText = c.Content
                };

            CommentsListView.ItemsSource = list.ToList();
        }

        private void CommentsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CommentsListView.SelectedItem == null)
            {
                _selectedComment = null;
                CommentDetailsTextBox.Text = "";
                return;
            }

            dynamic item = CommentsListView.SelectedItem;
            _selectedComment = item.Comment;

            // находит пользователя и статью
            var user = _db.Users.FirstOrDefault(u => u.UserID == _selectedComment.UserID);
            var article = _db.Articles.FirstOrDefault(a => a.ArticleID == _selectedComment.ArticleID);

            // собираем всё в кучу
            CommentDetailsTextBox.Text =
                "Комментарий ID: " + _selectedComment.CommentID + "\n" +
                "Дата: " + _selectedComment.Date + "\n\n" +
                "Автор: " + (user?.Username) + "\n" +
                "Email: " + (user?.Email) + "\n\n" +
                "Текст:\n" + _selectedComment.Content;
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedComment == null)
            {
                MessageBox.Show("Выбери комментарий!");
                return;
            }

            _selectedComment.IsDeleted = true;
            _db.SaveChanges();
            MessageBox.Show("Комментарий удалён!");

            LoadComments();
        }

        private void Block_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Пользователь заблокирован");
        }
    }
}
