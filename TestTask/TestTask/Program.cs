using System;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using System.Linq;
using TestTask.Services.Implementations;
namespace TestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("your_connection_string_here") // ”бедитесь, что заменили на ваш connection string
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var taskService = new TaskService(context);

                // 1.  ниги с названием "Red" и опубликованные после выхода альбома "Carolus Rex" группы Sabaton
                var redBooks = taskService.GetBooksContainingRedPublishedAfter2012().Result;
                Console.WriteLine("Books containing 'Red' and published after 2012:");
                redBooks.ForEach(b => Console.WriteLine(b.Title));

                // 2.  нига с наибольшей стоимостью опубликованного тиража
                var mostExpensiveBook = taskService.GetBookWithHighestPublishedCost().Result;
                Console.WriteLine($"\nBook with the highest published cost: {mostExpensiveBook?.Title}");

                // 3. јвторы, написавшие четное количество книг, изданных после 2015 года
                var authorsWithEvenBooks = taskService.GetAuthorsWithEvenNumberOfBooksAfter2015().Result;
                Console.WriteLine("\nAuthors with an even number of books published after 2015:");
                authorsWithEvenBooks.ForEach(a => Console.WriteLine($"{a.Name} {a.Surname}"));

                // 4. јвтор книги с самым длинным названием
                var authorOfLongestTitleBook = taskService.GetAuthorOfLongestTitledBook().Result;
                Console.WriteLine($"\nAuthor of the book with the longest title: {authorOfLongestTitleBook?.Name} {authorOfLongestTitleBook?.Surname}");
            }


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
