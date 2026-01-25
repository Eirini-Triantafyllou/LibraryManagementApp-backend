using LibraryManagementApp.Data;

namespace LibraryManagementApp.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {

        public AdminRepository(LibraryAppDbContext context) : base(context)
        {
        }

        
    }
}
