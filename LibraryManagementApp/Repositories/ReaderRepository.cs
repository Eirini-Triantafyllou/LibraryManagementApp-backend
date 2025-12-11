using LibraryManagementApp.Models;
using LibraryManagementApp.Core.Enums;
using LibraryManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementApp.Repositories
{
    public class ReaderRepository : BaseRepository<Reader>, IReaderRepository
    {
        public ReaderRepository(LibraryAppDbContext context) : base(context)
        {
        }

        public async Task<List<Book>> GetBorrowedBooksAsync(int readerId)
        {
            List<Book> books;

            books = await context.Readers
                .Where(r => r.Id == readerId)
                .SelectMany(r => r.Books)
                .ToListAsync();

            return books;
        }
        public async Task<Reader?> GetReaderByFullNameAsync(string fullName)
        {
            return await context.Readers
                .Where(r => r.FullName == fullName)
                .SingleOrDefaultAsync();
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersReadersAsync(int pageNumber, int pageSize, 
            List<Expression<Func<User, bool>>> predicates)
        {
            IQueryable<User> query = context.Users
                .Where(u => u.UserRole == UserRole.Reader)
                .Include(u => u.Reader);   // Εager loading της σχετικής οντότητας Reader


            if (predicates != null && predicates.Count > 0)
            {
                foreach (var predicate in predicates)
                {
                    query = query.Where(predicate);
                }
            }

            int totalRecords = await query.CountAsync();
            int skip = (pageNumber - 1) * pageSize;

            var data = await query
                .OrderBy(u => u.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<User>(data, totalRecords, pageNumber, pageSize);


        }

        
    }
}
