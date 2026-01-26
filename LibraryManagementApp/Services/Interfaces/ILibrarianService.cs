using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;

namespace LibraryManagementApp.Services.Interfaces
{
    public interface ILibrarianService
    {
        Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedLibrariansAsync(int pageNumber, int pageSize);
    }
}
