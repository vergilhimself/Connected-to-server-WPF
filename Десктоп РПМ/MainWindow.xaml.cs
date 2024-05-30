using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Десктоп_РПМ
{
    public partial class MainWindow : Window
    {
        UserContext db = new UserContext();
        public MainWindow()
        {
            InitializeComponent();
            ShowAllBooks();
        }
        private void CreateBooks (List<Book> books)
        {
            mainpage.Children.Clear();
            foreach (var book in books)
            {
                Image image = new Image();
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri("http://127.0.0.1/TheSite/BookPics/" + book.ImageUrl);
                logo.EndInit();
                image.Source = logo;
                Button newButton = new Button();
                newButton.Name = "id" + Convert.ToString(book.BookId);
                newButton.Width = 180;
                newButton.Height = 320;
                newButton.Margin = new Thickness(10, 0, 66, 10);
                newButton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFA629"));
                newButton.Click += Click_Book;
                newButton.Cursor = Cursors.Hand;
                newButton.Style = (Style)this.FindResource("DefaultButtonStyle"); // Применение стиля

                // Создание содержимого кнопки
                StackPanel stackPanel = new StackPanel();
                stackPanel.Width = 180;
                stackPanel.Height = 320;

                // Создание изображения
                Image newImage = new Image();
                newImage.Name = "newimage";
                newImage.Width = 180;
                newImage.Height = 284;
                newImage.Cursor = Cursors.Hand;
                newImage.Stretch = Stretch.Fill;
                newImage.Source = image.Source;
                // Создание текстового поля
                TextBox newText = new TextBox();
                newText.Name = "newtext";
                newText.Width = 180;
                newText.Height = 36;
                newText.HorizontalContentAlignment = HorizontalAlignment.Center;
                newText.Background = Brushes.Transparent;
                newText.BorderBrush = Brushes.Transparent;
                newText.Cursor = Cursors.Hand;
                newText.FontFamily = new FontFamily("Comic Sans MS");
                newText.FontWeight = FontWeights.Bold;
                newText.Foreground = Brushes.Green;
                newText.IsReadOnly = true;
                newText.MaxLines = 2;
                newText.SelectionBrush = Brushes.Transparent;
                newText.TextWrapping = TextWrapping.Wrap;
                newText.Text = book.Title + " " + book.Author;
                // Добавление изображения и текстового поля в контейнер StackPanel
                stackPanel.Children.Add(newImage);
                stackPanel.Children.Add(newText);

                // Установка StackPanel в содержимое кнопки
                newButton.Content = stackPanel;
                mainpage.Children.Add(newButton);
                mainpage.Visibility = Visibility.Visible;
            }
        }
        private async Task ShowAllBooks()
        {
            await ApiClass.GetBooks();
            List<Book> books = JsonSerializer.Deserialize<List<Book>>(ApiClass.ApiBooks);
            CreateBooks(books);         
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                navigate.Text = "> Любимые книги";
                mainpage.Visibility = Visibility.Visible;              
                mainpage.Children.Clear();
                using (UserContext db = new UserContext())
                {
                    if (ApiClass.ApiFavoriteBooks == null) await ApiClass.GetFavoriteBooks();
                    List<FavoriteBooks> favbooks = JsonSerializer.Deserialize<List<FavoriteBooks>>(ApiClass.ApiFavoriteBooks);
                    var favoriteBookIds = favbooks
                            .Where(f => f.Email == Account.Email)
                            .Select(f => f.BookId)
                            .ToList();

                    if (ApiClass.ApiBooks == null) await ApiClass.GetBooks();
                    List<Book> books = JsonSerializer.Deserialize<List<Book>>(ApiClass.ApiBooks);
                    var favoriteBooks = books
                            .Where(b => favoriteBookIds
                            .Contains(b.BookId))
                            .ToList();

                    CreateBooks(favoriteBooks);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке избранных книг: {ex.Message}");
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Профиль taskWindow = new Профиль();
            taskWindow.Show();
        }
        private async void MainPage(object sender, RoutedEventArgs e)
        {
            if (ApiClass.ApiBooks == null) await ApiClass.GetBooks();

            // Проверяем, была ли успешно получена JSON-строка
            if (!string.IsNullOrEmpty(ApiClass.ApiBooks))
            {
                mainpage.Children.Clear();
                await ShowAllBooks();
            }
            else
            {
                MessageBox.Show("Failed to get JSON data from API.");
            }
            navigate.Text = "> Главная";
            mainpage.Visibility = Visibility.Visible;
            favoritecontent.Visibility = Visibility.Hidden;
        }
        private void ShowGenreBooks_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string genre = menuItem.Header.ToString();

            LoadGenreBooks(genre);
        }
        private async void LoadGenreBooks(string genre)
        {
            try
            {
                navigate.Text = "> " + genre;
                mainpage.Visibility = Visibility.Hidden;               
                if (ApiClass.ApiBooks == null) await ApiClass.GetBooks();
                List<Book> books = JsonSerializer.Deserialize<List<Book>>(ApiClass.ApiBooks);
                var genreBooks = books.Where(b => b.Genre.ToLower() == genre.ToLower()).ToList();
                CreateBooks(genreBooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке книг: {ex.Message}");
            }
        }
        private async void Click_Book(object sender, RoutedEventArgs e)
        {
            string name = (sender as Button).Name;
            name = name.Remove(0, 2);
            if (ApiClass.ApiBooks == null) await ApiClass.GetBooks();
            List<Book> book = JsonSerializer.Deserialize<List<Book>>(ApiClass.ApiBooks);
            var books = book.Where(b => b.BookId == Convert.ToInt32(name)).ToList();
            foreach (var selectedbook in books)
            {

                Книга taskWindow = new Книга(selectedbook.BookId, selectedbook.Text, selectedbook.Title, selectedbook.Author, selectedbook.Genre, selectedbook.Year);
                taskWindow.Show();
            }
            this.Close();
        }
        private void LoadBooks(string searchQuery = "")
        {
            
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            LoadBooks(searchTextbox.Text.ToLower());
            mainpage.Visibility = Visibility.Hidden;
            favoritecontent.Visibility = Visibility.Visible;
            navigate.Text = "> Поиск";
            try
            {
                if (ApiClass.ApiBooks == null) await ApiClass.GetBooks();
                List<Book> books = JsonSerializer.Deserialize<List<Book>>(ApiClass.ApiBooks);
                var searchbooks = books
                    .Where(b => string.IsNullOrEmpty(searchTextbox.Text.ToLower()) ||
                                b.Title.ToLower().Contains(searchTextbox.Text.ToLower()) ||
                                b.Author.ToLower().Contains(searchTextbox.Text.ToLower()) ||
                                b.Genre.ToLower().Contains(searchTextbox.Text.ToLower()))
                    .ToList();
                mainpage.Children.Clear();
                CreateBooks(searchbooks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке книг: {ex.Message}");
            }
        }
        private void ToTheSource(object sender, RoutedEventArgs e)
        {
            Vk_Tg taskWindow = new Vk_Tg();
            taskWindow.Owner = this;
            taskWindow.Show();
        }
    }
}
