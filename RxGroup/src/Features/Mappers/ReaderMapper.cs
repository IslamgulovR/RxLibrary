using AutoMapper;
using RxGroup.Features.Dto.Readers;
using RxGroup.Models;

namespace RxGroup.Features.Mappers;

public class ReaderMapper : Profile
{
    public ReaderMapper()
    {
        CreateMap<Reader, ReaderDto>();
        
        CreateMap<Reader, ReaderReaderHistoryDto>()
            .ForMember(d => d.BooksIssueList, opts
                => opts.MapFrom(reader => reader.BooksIssuance));

        CreateMap<Reader, ReaderIssuedBooksDto>()
            .ForMember(d => d.IssuedBooks, opts
                => opts.Ignore());
    }
}