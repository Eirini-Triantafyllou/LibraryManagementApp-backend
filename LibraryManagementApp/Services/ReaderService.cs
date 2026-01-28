using AutoMapper;
using LibraryManagementApp.Core.Enums;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;
using LibraryManagementApp.Exceptions;
using LibraryManagementApp.Repositories.Interfaces;
using LibraryManagementApp.Security;
using Serilog;

namespace LibraryManagementApp.Services
{
    public class ReaderService : IReaderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<ReaderService> logger = new LoggerFactory().AddSerilog().CreateLogger<ReaderService>();
        public ReaderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


    }
}