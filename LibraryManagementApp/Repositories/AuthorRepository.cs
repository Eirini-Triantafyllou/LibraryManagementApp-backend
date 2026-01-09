using Microsoft.EntityFrameworkCore;
using LibraryManagementApp.Data;
using LibraryManagementApp.Models;
using System.Linq.Expressions;

namespace LibraryManagementApp.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {

        public AuthorRepository(LibraryAppDbContext context) : base(context)
        {
        }
        public async Task<List<Book>> GetAuthorBooksAsync(int authorId)
        {
            List<Book> books;

            books = await context.Authors
                .Where(a => a.Id == authorId)
                .SelectMany(a => a.Books)
                .ToListAsync();

            return books;
        }

        public async Task<Author?> GetAuthorWithDetailsAsync(int authorId)
        {
            return await context.Authors
                .Where(a => a.Id == authorId)
                .FirstOrDefaultAsync(a => a.Id == authorId);
        }

      

        public async Task<PaginatedResult<Author>> GetPaginatedAuthorsFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<Author, bool>>> predicates)
        {
            IQueryable<Author> query = context.Authors; 

            if (predicates != null && predicates.Count > 0)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            // Get total records count before pagination
            int totalRecords = await query.CountAsync();

            // Apply pagination
            int skip = (pageNumber - 1) * pageSize;

            var data = await query
                .OrderBy(a => a.Lastname) // Example ordering
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Author>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

        }

        public async Task<PaginatedResult<Book>> GetPaginatedBooksByAuthorNameAsync(string authorName, int pageNumber, int pageSize)
        {
            var query = context.Books
                .Include(b => b.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(authorName))
            {
                var searchTerm = authorName.Trim().ToLower();

                query = query.Where(b =>
                b.Author!.Firstname.ToLower().Contains(searchTerm) ||
                b.Author.Lastname.ToLower().Contains(searchTerm) ||
                (b.Author.Firstname + " " + b.Author.Lastname).ToLower().Contains(searchTerm));
            }

            int totalRecords = await query.CountAsync();

            int skip = (pageNumber - 1) * pageSize;

            var data = await query
                .OrderBy(b => b.Author!.Lastname)
                .ThenBy(b => b.Author!.Firstname)
                .ThenBy(b => b.Title)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Book>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
