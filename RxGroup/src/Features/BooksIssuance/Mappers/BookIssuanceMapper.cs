using AutoMapper;
using RxGroup.Features.Books.Dto;
using RxGroup.Features.BooksIssuance.Dto;
using RxGroup.Features.Readers.Dto;
using RxGroup.Models;

namespace RxGroup.Features.BooksIssuance.Mappers;

public class BookIssuanceMapper : Profile
{
    public BookIssuanceMapper()
    {
        CreateMap<IssueBookDto, BookIssuance>()
            .ForMember(d => d.Id, opts
                => opts.Ignore())
            .ForMember(d => d.ReturnDate, opts
                => opts.Ignore())
            .ForMember(d => d.IssueDate, opts
                => opts.MapFrom(x => DateTimeOffset.UtcNow));

        CreateMap<BookIssuance, BookReaderHistoryDto>();

        CreateMap<BookIssuance, ReaderBookHistoryDto>();
    }
}