using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace Десктоп_РПМ
{
    public partial class Книга : Window
    {
        public int Id;
        public string Text;
        public string Title;
        public string Author;
        public string Genre;
        public string Year;
        public bool h;
        public Книга()
        {
            InitializeComponent();
        }
        public Книга(int Id, string Text, string Title, string Author, string Genre, string Year)
        {
            this.Id = Id;
            this.Text = Text;
            this.Title = Title;
            this.Author = Author;
            this.Genre = Genre;
            this.Year = Year;
            InitializeComponent();

            AddData();
        }
        private async void AddData()
        {
           
            label.Content = this.Title;
            textbox.Text = this.Text;
            await ApiClass.GetFavoriteBooks();

            
            if (ApiClass.ApiFavoriteBooks != "{\"message\":\"No favorite books found.\"}")
            {
                List<FavoriteBooks> favbooks = JsonSerializer.Deserialize<List<FavoriteBooks>>(ApiClass.ApiFavoriteBooks);
                var count = favbooks.Where(f => f.BookId == this.Id && f.Email == Account.Email).ToList().Count();
                Console.WriteLine(count);
                if (count > 0)
                {
                    heartbutton.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00680A"));
                }
                else
                {
                    heartbutton.Fill = Brushes.Black;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow taskWindow = new MainWindow();
            taskWindow.Show();
            this.Close();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Author + " " + Genre + " " + Convert.ToString(Year));
        }
        private async void heart_Click(object sender, RoutedEventArgs e)
        {

            string checkApiUrl = "http://127.0.0.1/api/check_favorite.php";
            string addApiUrl = "http://127.0.0.1/api/add_to_favorites.php";
            string removeApiUrl = "http://127.0.0.1/api/remove_from_favorites.php";

            using (var client = new WebClient())
            {
                var data = new NameValueCollection();
                data["Email"] = Account.Email.ToString();
                data["BookId"] = Convert.ToString(Id);
                Console.WriteLine(Account.Email);
                try
                {
                    // Проверка наличия книги в избранных
                    var checkResponse = client.UploadValues(checkApiUrl, "POST", data);
                    var checkResponseString = System.Text.Encoding.UTF8.GetString(checkResponse);
                    var exists = checkResponseString.Contains("\"exists\":true");
                    Console.WriteLine(checkResponseString);
                    if (exists)
                    {
                        // Книга уже в избранных, удаляем
                        var removeResponse = client.UploadValues(removeApiUrl, "POST", data);
                        var removeResponseString = System.Text.Encoding.UTF8.GetString(removeResponse);

                        heartbutton.Fill = Brushes.Black;
                        MessageBox.Show(removeResponseString);
                    }
                    else
                    {
                        // Книга не в избранных, добавляем
                        var addResponse = client.UploadValues(addApiUrl, "POST", data);
                        var addResponseString = System.Text.Encoding.UTF8.GetString(addResponse);

                        heartbutton.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00680A"));
                        MessageBox.Show(addResponseString);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

    }
}
