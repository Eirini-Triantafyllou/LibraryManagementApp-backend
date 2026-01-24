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
                .ForMember(dest => dest.AuthorFullName,
                    opt => opt.MapFrom(src =>
                        src.Author != null
                            ? $"{src.Author.AuthorFullName}"
                            : "Άγνωστος Συγγραφέας"));

            CreateMap<Book, UpdateBookDTO>().ReverseMap();
            CreateMap<Book, CreateBookDTO>().ReverseMap();
        }
    }
}
