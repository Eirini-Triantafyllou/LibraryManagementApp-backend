using AutoMapper;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Models;
using LibraryManagementApp.Repositories;
using Serilog;

namespace LibraryManagementApp.Services
{
    public class LibrarianService : ILibrarianService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<LibrarianService> logger =
            new LoggerFactory().AddSerilog().CreateLogger<LibrarianService>();

        public LibrarianService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PaginatedResult<UserReadOnlyDTO>> GetPaginatedLibrariansAsync(int pageNumber, int pageSize)
        {
            var result = await unitOfWork.LibrarianRepository.GetPaginatedUsersLibrariansAsync(pageNumber, pageSize);

            var dtoResult = new PaginatedResult<UserReadOnlyDTO>
            {
                Data = result.Data.Select(u => new UserReadOnlyDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname,
                    UserRole = u.UserRole.ToString()!
                }).ToList(),
                TotalRecords = result.TotalRecords,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };
            logger.LogInformation("Retrieved {Count} librarians for page {PageNumber} with page size {PageSize}.",
                dtoResult.Data.Count, pageNumber, pageSize);
            return dtoResult;
        }
    }
}