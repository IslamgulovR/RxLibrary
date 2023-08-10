using AutoMapper;
using RxGroup.Features.Dto.Books;
using RxGroup.Features.Dto.Issues;
using RxGroup.Features.Dto.Readers;
using RxGroup.Models;

namespace RxGroup.Features.Mappers;

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