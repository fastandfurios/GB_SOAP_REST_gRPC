using System;
using System.Collections.Generic;
using System.Linq;
using LibraryService.Models;
using LibraryService.Services.Interfaces;

namespace LibraryService.Services.Implementation
{
    public class LibraryRepository : ILibraryRepositoryService
    {
        private readonly ILibraryDatabaseContextService _dbContext;

        public LibraryRepository(ILibraryDatabaseContextService dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Book book)
        {
            book.Id = Guid.NewGuid()
                .ToString()
                .Replace("-", "")
                .ToLower()
                .Remove(0, 8);

            _dbContext.Books.Add(book);
            return 1;
        }

        public int Delete(Book book)
        {
            _dbContext.Books.Remove(book);
            return 1;
        }

        public IList<Book> GetAll()
        {
            return _dbContext.Books;
        }

        public IList<Book> GetByAuthor(string authorName)
        {
            return _dbContext.Books.Where(book =>
                book.Authors.Where(author =>
                    author.Name.ToLower().Contains(authorName.ToLower())).Count() > 0).ToList();
        }

        public IList<Book> GetByCategory(string category)
        {
            return _dbContext.Books.Where(book =>
                book.Category.ToLower().Contains(category.ToLower())).ToList();
        }

        public Book GetById(string id)
        {
            return _dbContext.Books.FirstOrDefault(b => b.Id.Equals(id));
        }

        public IList<Book> GetByTitle(string title)
        {
            return _dbContext.Books.Where(book =>
                book.Title.ToLower().Contains(title.ToLower())).ToList();
        }

        public int Update(Book book)
        {
            var foundBook = _dbContext.Books.FirstOrDefault(b => b.Id.Equals(book.Id));
            var index = _dbContext.Books.IndexOf(foundBook);
            _dbContext.Books.Insert(index, book);
            return 1;
        }
    }
}
