using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace Десктоп_РПМ
{
    internal static class ApiClass
    {
        public static string ApiBooks { get; set; }
        public static string ApiUsers { get; set; }
        public static string ApiFavoriteBooks { get; set; }
        public static string ApiReadingHistories { get; set; }
        public static async Task GetBooks()
        {
            // URL вашего API-эндпоинта
            string apiUrl = "http://127.0.0.1/api/books_api.php";

            // Создание HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Отправка GET-запроса к API-эндпоинту
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Проверка успешности запроса
                    if (response.IsSuccessStatusCode)
                    {
                        // Чтение содержимого ответа
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Вывод полученных данных
                        Console.WriteLine("Response JSON:");
                        Console.WriteLine(responseData);
                        ApiBooks = responseData;
                    }
                    else
                    {
                        // В случае ошибки выводим статус-код
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // В случае исключения выводим сообщение об ошибке
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
        }
        public static async Task GetUsers()
        {
            // URL вашего API-эндпоинта
            string apiUrl = "http://127.0.0.1/api/users_api.php";

            // Создание HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Отправка GET-запроса к API-эндпоинту
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Проверка успешности запроса
                    if (response.IsSuccessStatusCode)
                    {
                        // Чтение содержимого ответа
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Вывод полученных данных
                        Console.WriteLine("Response JSON:");
                        Console.WriteLine(responseData);
                        ApiUsers = responseData;
                    }
                    else
                    {
                        // В случае ошибки выводим статус-код
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // В случае исключения выводим сообщение об ошибке
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
        }
        public static async Task GetFavoriteBooks()
        {
            // URL вашего API-эндпоинта
            string apiUrl = "http://127.0.0.1/api/favorite_books_api.php";

            // Создание HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Отправка GET-запроса к API-эндпоинту
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Проверка успешности запроса
                    if (response.IsSuccessStatusCode)
                    {
                        // Чтение содержимого ответа
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Вывод полученных данных
                        Console.WriteLine("Response JSON:");
                        Console.WriteLine(responseData);
                        ApiFavoriteBooks = responseData;
                    }
                    else
                    {
                        // В случае ошибки выводим статус-код
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // В случае исключения выводим сообщение об ошибке
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
        }
        public static async Task GetReadingHistories()
        {
            // URL вашего API-эндпоинта
            string apiUrl = "http://127.0.0.1/api/reading_histories_api.php";

            // Создание HttpClient
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Отправка GET-запроса к API-эндпоинту
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Проверка успешности запроса
                    if (response.IsSuccessStatusCode)
                    {
                        // Чтение содержимого ответа
                        string responseData = await response.Content.ReadAsStringAsync();

                        // Вывод полученных данных
                        Console.WriteLine("Response JSON:");
                        Console.WriteLine(responseData);
                        ApiReadingHistories = responseData;
                    }
                    else
                    {
                        // В случае ошибки выводим статус-код
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // В случае исключения выводим сообщение об ошибке
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
        }

        public static async Task ClearAndAddDBFromJson(string ApiBooks, string ApiUsers, string ApiFavoriteBooks, string ApiReadingHistories)
        {
            Console.WriteLine("Started ClearAndAddDBFromJson");
            using (UserContext _context = new UserContext())
            {
                _context.Database.CreateIfNotExists();

                /*_context.Database.ExecuteSqlCommand("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
                _context.Database.ExecuteSqlCommand("TRUNCATE TABLE FavoriteBooks;");
                _context.Database.ExecuteSqlCommand("TRUNCATE TABLE ReadingHistories;");
                _context.Database.ExecuteSqlCommand("TRUNCATE TABLE ReadingHistories;");
                _context.Database.ExecuteSqlCommand($"TRUNCATE TABLE Books;");
                _context.Database.ExecuteSqlCommand($"TRUNCATE TABLE Users;");
                _context.Database.ExecuteSqlCommand("EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");*/

                // Чтение данных из JSON-файла
                // Добавление каждой таблицы из JSON в базу данных
                try
                {
                    List<Book> books = JsonSerializer.Deserialize<List<Book>>(ApiBooks);
                    foreach (var book in books)
                    {
                        _context.Books.Add(book);
                    }
                }
                catch (Exception ex) { }
                try
                {
                    List<User> users = JsonSerializer.Deserialize<List<User>>(ApiUsers);
                    foreach (var user in users)
                    {
                        _context.Users.Add(user);
                    }
                }
                catch (Exception ex) { }
                try
                {
                    List<FavoriteBooks> favoriteBooks = JsonSerializer.Deserialize<List<FavoriteBooks>>(ApiFavoriteBooks);
                    foreach (var fav in favoriteBooks)
                    {
                        _context.FavoriteBooks.Add(fav);
                    }
                }
                catch (Exception ex) { }
                try
                {
                    List<ReadingHistory> readingHistories = JsonSerializer.Deserialize<List<ReadingHistory>>(ApiReadingHistories);
                    foreach (var history in readingHistories)
                    {
                        _context.ReadingHistory.Add(history);
                    }
                }
                catch (Exception ex) { }





                // Сохранение изменений в базе данных
                await _context.SaveChangesAsync();

            }
            Console.WriteLine("Check DB, Db exists and updated");
        }

    }
}
