ADD YOUR IMPLEMENTATIONS HERE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;

namespace TestTask.Services.Implementations
{
    public class TaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1.  ниги с названием "Red" и опубликованные после выхода альбома "Carolus Rex" группы Sabaton
        public async Task<List<Book>> GetBooksContainingRedPublishedAfter2012()
        {
            return await _context.Books
                .Where(b => b.Title.Contains("Red") && b.PublishDate > new DateTime(2012, 12, 31))
                .ToListAsync();
        }

        // 2.  нига с наибольшей стоимостью опубликованного тиража
        public async Task<Book> GetBookWithHighestPublishedCost()
        {
            return await _context.Books
                .OrderByDescending(b => b.Price * b.QuantityPublished)
                .FirstOrDefaultAsync();
        }

        // 3. јвторы, написавшие четное количество книг, изданных после 2015 года
        public async Task<List<Author>> GetAuthorsWithEvenNumberOfBooksAfter2015()
        {
            var authorBookCounts = await _context.Books
                .Where(b => b.PublishDate > new DateTime(2015, 12, 31))
                .GroupBy(b => b.AuthorId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            return await _context.Authors
                .Where(a => authorBookCounts.ContainsKey(a.Id) && authorBookCounts[a.Id] % 2 == 0)
                .ToListAsync();
        }

        // 4. јвтор книги с самым длинным названием
        public async Task<Author> GetAuthorOfLongestTitledBook()
        {
            var longestTitledBook = await _context.Books
                .OrderByDescending(b => b.Title.Length)
                .ThenBy(b => b.AuthorId)
                .FirstOrDefaultAsync();
            return await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == longestTitledBook.AuthorId);
        }
    }
}
