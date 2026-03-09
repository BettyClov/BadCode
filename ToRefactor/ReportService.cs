using System;
using System.Collections.Generic;
using System.Text;

namespace ToRefactor
{
    internal class ReportService
    {
        static readonly HttpClient http = new HttpClient();
        const string API = "https://jsonplaceholder.typicode.com";

      
        static List<string> GetUsers()
        {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Загружаю пользователей...");
            var json = http.GetStringAsync($"{API}/users").GetAwaiter().GetResult();
            Thread.Sleep(500); 
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Пользователи загружены");
            return new List<string> { "Alice", "Bob", "Charlie" };
        }

     
        static List<string> GetPostsForUser(string user)
        {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Загружаю посты для {user}...");
            var json = http.GetStringAsync($"{API}/posts?userId=1").GetAwaiter().GetResult();
            Thread.Sleep(300);
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Посты для {user} загружены");
            return new List<string> { $"Пост1_{user}", $"Пост2_{user}" };
        }

      
        static int GetCommentCount(string post)
        {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Считаю комментарии: {post}...");
            var json = http.GetStringAsync($"{API}/comments?postId=1").GetAwaiter().GetResult();
            Thread.Sleep(200);
            return new Random().Next(1, 20);
        }

      
        static void SaveReport(string content)
        {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Сохраняю отчёт...");
            File.WriteAllText("report.txt", content);
            Thread.Sleep(400); 
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Отчёт сохранён");
        }

       
        static bool SendNotification(string email)
        {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Отправляю письмо на {email}...");
            http.PostAsync($"{API}/posts", null).GetAwaiter().GetResult();
            Thread.Sleep(600);
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Письмо отправлено");
            return true;
        }

        public static void GenerateReport()
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("\n[SYNC] Генерация отчёта...\n");

            
            var users = GetUsers();                          

            var report = "";
            foreach (var user in users)
            {
                var posts = GetPostsForUser(user);              
                foreach (var post in posts)
                {
                    var comments = GetCommentCount(post);      
                    report += $"{user} | {post} | комментариев: {comments}\n";
                }
            }

            SaveReport(report);                                 
            SendNotification("boss@company.com");               

            Console.WriteLine($"\n[SYNC] Готово за {sw.ElapsedMilliseconds}мс");
          
        }
    }
}
