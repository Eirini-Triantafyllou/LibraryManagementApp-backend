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
            CreateMap<User, UpdateUserReaderDTO>().ReverseMap();

            CreateMap<Book, BookByAuthorDTO>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src =>
                        src.Author != null
                            ? $"{src.Author.Firstname} {src.Author.Lastname}"
                            : "Άγνωστος Συγγραφέας"));
        }
    }
}
