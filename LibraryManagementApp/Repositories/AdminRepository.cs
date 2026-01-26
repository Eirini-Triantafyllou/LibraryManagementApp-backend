using LibraryManagementApp.Data;
using LibraryManagementApp.Repositories.Interfaces;

namespace LibraryManagementApp.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {

        public AdminRepository(LibraryAppDbContext context) : base(context)
        {
        }

        
    }
}
