using System;
using System.Collections.Specialized;
using System.Net;
using System.Windows;
namespace Десктоп_РПМ
{
    /// <summary>
    /// Логика взаимодействия для AccWindow.xaml
    /// </summary>
    public partial class AccWindow : Window
    {
        public AccWindow()
        {
            InitializeComponent();

        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string apiUrl = "http://127.0.0.1/api/auth_api.php";
            using (UserContext context = new UserContext())
            {
                var data = new NameValueCollection();
                data["email"] = txtEmail.Text; ;
                data["username"] = txtUsername.Text;
                data["password"] = txtPassword.Password;

                // Отправка HTTP POST-запроса
                try
                {
                    using (var client = new WebClient())
                    {
                        // Отправка данных и получение ответа
                        var response = client.UploadValues(apiUrl, "POST", data);

                        // Преобразование ответа в строку
                        var responseString = System.Text.Encoding.Default.GetString(response);



                        if (responseString.Contains("success"))
                        {
                            // Пользователь аутентифицирован успешно
                            Account.Email = data["email"];
                            Account.Name = data["username"];
                            Account.Password = data["password"];
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            // Пользователь не аутентифицирован
                            Console.WriteLine("Authentication failed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string apiUrl = "http://127.0.0.1/api/reg_api.php"; // URL для API регистрации
            using (UserContext context = new UserContext())
            {
                var data = new NameValueCollection();
                data["email"] = txtEmail.Text;
                data["username"] = txtUsername.Text;
                data["password"] = txtPassword.Password;

                // Отправка HTTP POST-запроса
                try
                {
                    using (var client = new WebClient())
                    {
                        // Установка кодировки клиента в UTF-8
                        client.Encoding = System.Text.Encoding.UTF8;

                        // Отправка данных и получение ответа
                        var response = client.UploadValues(apiUrl, "POST", data);

                        // Преобразование ответа в строку с использованием UTF-8
                        var responseString = System.Text.Encoding.UTF8.GetString(response);

                        if (responseString.Contains("Registration successful."))
                        {
                            // Пользователь зарегистрирован успешно
                            MessageBox.Show("Registration successful!");
                          
                        }
                        else if (responseString.Contains("Username already exists."))
                        {
                            // Логин уже используется
                            MessageBox.Show("Username already exists.");
                        }
                        else
                        {
                            // Ошибка при регистрации
                            MessageBox.Show("Registration error: " + responseString);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
