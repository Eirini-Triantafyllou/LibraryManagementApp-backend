using LibraryManagementApp.Models;
using LibraryManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryManagementApp.Security;
using LibraryManagementApp.DTO;

namespace LibraryManagementApp.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(LibraryAppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return null;

            if (!EncryptionUtil.IsValidPassword(password, user.Password)) return null;

            return user;


        }

        public async Task<User?> GetUserByUsernameAsync(string username) =>
         await context.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<PaginatedResult<User>> GetUsersAsync(int pageNumber, int pageSize, List<Expression<Func<User, bool>>> predicates)
        {
            IQueryable<User> query = context.Users;

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

        public async Task<UserReaderReadOnlyDTO?> GetUserReaderAsync(string username)
        {
            var userReader = await context.Users
                .Where(u => u.Username == username)
                .Include(u => u.Reader)                // Include Reader navigation property
                .Select(u => new UserReaderReadOnlyDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Password = u.Password,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    PhoneNumber = u.Reader!.PhoneNumber,
                    Address = u.Reader!.Address,
                    UserRole = u.UserRole.ToString()!
                })
                .FirstOrDefaultAsync();
            Console.WriteLine("UserReader: " + userReader!.Firstname + userReader!.Lastname);
            return userReader;
        }
    }
}
