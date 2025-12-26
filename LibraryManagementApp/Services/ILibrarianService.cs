using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Services
{
    public interface ILibrarianService
    {
        Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedLibrariansAsync(int pageNumber, int pageSize);
    }
}
