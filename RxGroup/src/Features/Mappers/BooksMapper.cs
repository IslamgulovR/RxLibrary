using AutoMapper;
using RxGroup.Features.Dto.Books;
using RxGroup.Models;

namespace RxGroup.Features.Mappers;

public class BooksMapper : Profile
{
    public BooksMapper()
    {
        CreateMap<Book, BookDto>().ReverseMap();
        
        CreateMap<Book, AvailableBookDto>()
            .ForMember(d => d.AvailableQuantity, opts
                => opts.Ignore());
        
        CreateMap<Book, IssuedBookDto>()
            .ForMember(d => d.Readers, opts
                => opts.Ignore());

        CreateMap<Book, BookBookHistoryDto>()
            .ForMember(d => d.ReadersList, opts 
                => opts.MapFrom(book => book.BooksIssuance));
    }
}