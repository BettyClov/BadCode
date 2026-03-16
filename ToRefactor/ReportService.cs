using System;
using System.Collections.Generic;
using System.Text;

namespace ToRefactor
{
    internal class ReportService
    {
        readonly HttpClient http = new HttpClient();
        const string API = "https://jsonplaceholder.typicode.com";

      
        async Task<List<string>> GetUsersAsync() {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Загружаю пользователей...");
            var json = await http.GetStringAsync($"{API}/users"); 
            await Task.Delay(500); 
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Пользователи загружены");
            return new List<string> { "Alice", "Bob", "Charlie" };
        }

     
        async Task<List<string>> GetPostsForUserAsync(string user) {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Загружаю посты для {user}...");
            var json = await http.GetStringAsync($"{API}/posts?userId=1");
            await Task.Delay(300);
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Посты для {user} загружены");
            return new List<string> { $"Пост1_{user}", $"Пост2_{user}" };
        }

      
        async Task<int> GetCommentCountAsync(string post) {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Считаю комментарии: {post}...");
            var json = await http.GetStringAsync($"{API}/comments?postId=1");
            await Task.Delay(200);
            return new Random().Next(1, 20);
        }

      
        async Task SaveReportAsync(string content) {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Сохраняю отчёт...");
            await File.WriteAllTextAsync("report.txt", content); 
            await Task.Delay(400);
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Отчёт сохранён");
        }

       
        async Task<bool> SendNotificationAsync(string email) {
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Отправляю письмо на {email}...");
            await http.PostAsync($"{API}/posts", null); 
            await Task.Delay(600);
            Console.WriteLine($"  [#{Thread.CurrentThread.ManagedThreadId}] Письмо отправлено");
            return true;
        }

        public async static Task GenerateReportAsync() {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("\n[ASYNC] Генерация отчёта...\n");
            var service = new ReportService(); 

            var users = await service.GetUsersAsync(); 

            var report = "";
            foreach (var user in users) {
                var posts = await service.GetPostsForUserAsync(user); 
                foreach (var post in posts) {
                    var comments = await service.GetCommentCountAsync(post); 
                    report += $"{user} | {post} | комментариев: {comments}\n";
                }
            }

            await service.SaveReportAsync(report); 
            await service.SendNotificationAsync("boss@company.com"); 

        Console.WriteLine($"[ASYNC] Готово за {sw.ElapsedMilliseconds}мс");
        }
    }
}
