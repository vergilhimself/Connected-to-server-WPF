using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Десктоп_РПМ
{
    /// <summary>
    /// Логика взаимодействия для Профиль.xaml
    /// </summary>
    public partial class Профиль : Window
    {
        public Профиль()
        {
            InitializeComponent();
            AddData();
        }
        private async void AddData()
        {
            login.Text = Account.Name;
            email.Text = Account.Email;
            password.Password = Account.Password;

            listbox.Items.Clear();
            await ApiClass.GetFavoriteBooks();
            if (ApiClass.ApiFavoriteBooks == "{\"message\":\"No favorite books found.\"}")
            {
                Label label = new Label();
                label.Content = "Нет избранных книг";
                listbox.Items.Add(label);
            }
            else
            {
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

                foreach (var book in favoriteBooks)
                {
                    BitmapImage image = new BitmapImage(new Uri("http://127.0.0.1/TheSite/BookPics/" + book.ImageUrl)); // Замените "example_image.jpg" на путь к вашему изображению
                    image.DecodePixelWidth = 32;
                    image.DecodePixelHeight = 32;


                    // Создание элемента StackPanel для содержания изображения и текста
                    StackPanel stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;

                    // Добавление изображения в StackPanel
                    Image img = new Image();
                    img.Width = 32;
                    img.Height = 32;
                    img.Source = image;
                    img.Stretch = System.Windows.Media.Stretch.Uniform; // Масштабирование изображения до размера контейнера
                    stackPanel.Children.Add(img);

                    string uniqueCode = Convert.ToString(book.BookId);
                    stackPanel.Tag = uniqueCode;

                    // Добавление текста в StackPanel
                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Text = book.Title;
                    stackPanel.Children.Add(textBlock1);

                    TextBlock textBlock2 = new TextBlock();
                    textBlock2.Text = " " + book.Author;
                    stackPanel.Children.Add(textBlock2);

                    // Добавление StackPanel в ListBox
                    listbox.Items.Add(stackPanel);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow taskWindow = new MainWindow();
            taskWindow.Show();
            this.Close();
        }

        private async void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверка, был ли выбран элемент
            if (listbox.SelectedItem != null)
            {
                // Получение названия элемента
                string uniqueCode = ((StackPanel)listbox.SelectedItem).Tag.ToString(); // Предполагается, что вторым элементом в StackPanel является TextBlock с названием

                if (ApiClass.ApiBooks == null) await ApiClass.GetBooks();
                List<Book> book = JsonSerializer.Deserialize<List<Book>>(ApiClass.ApiBooks);
                var books = book.Where(b => b.BookId == Convert.ToInt32(uniqueCode)).ToList();
                foreach (var selectedbook in books)
                {

                    Книга taskWindow = new Книга(selectedbook.BookId, selectedbook.Text, selectedbook.Title, selectedbook.Author, selectedbook.Genre, selectedbook.Year);
                    taskWindow.Show();
                }
                

                this.Close();
            }
        }
    }
}
