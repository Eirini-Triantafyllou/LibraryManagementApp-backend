using LibraryManagementApp.Core.Enums;
using LibraryManagementApp.Data;
using LibraryManagementApp.Models;
using LibraryManagementApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementApp.Repositories
{
    public class LibrarianRepository : BaseRepository<Librarian>, ILibrarianRepository
    {
        public LibrarianRepository(LibraryAppDbContext context) : base(context)
        {
        }

        public async Task<Librarian?> GetLibrarianWithDetailsAsync(int librarianId)
        {
            return await context.Librarians
                .Where(l => l.Id == librarianId)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserLibrarianByUsernameAsync(string username)
        {
            var userLibrarian = await context.Users
                .Where(u => u.Username == username && u.UserRole == UserRole.Librarian)
                .Include(u => u.Librarian)
                .SingleOrDefaultAsync();

            return userLibrarian;
        }

        public async Task<PaginatedResult<User>> GetPaginatedUsersLibrariansAsync(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            var usersWithRoleLibrarian = await context.Users
                .Where(u => u.UserRole == UserRole.Librarian)
                .Include(u => u.Librarian)    // Εager loading της σχετικής οντότητας Librarian 
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            int totalRecords = await context.Users
                .Where(u => u.UserRole == UserRole.Librarian)
                .CountAsync();

            return new PaginatedResult<User>(usersWithRoleLibrarian, totalRecords, pageNumber, pageSize);
        }

        public async Task<PaginatedResult<User>> GetPaginatedLibrariansFilteredAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            IQueryable<User> query = context.Users
                .Where(u => u.UserRole == UserRole.Librarian)
                .Include(u => u.Librarian);

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
