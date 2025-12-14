using AutoMapper;
using LibraryManagementApp.Data;
using LibraryManagementApp.DTO;

namespace LibraryManagementApp.Configuration
{
    public class MapperConfig : Profile
    {

        public MapperConfig()
        {
            CreateMap<User, UserReadOnlyDTO>().ReverseMap();
        }
    }
}
