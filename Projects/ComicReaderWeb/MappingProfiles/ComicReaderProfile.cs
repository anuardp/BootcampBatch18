using AutoMapper;
using ComicReader.Models;
using ComicReader.DTOs;

namespace ComicReader.MappingProfiles;

public class ComicReaderProfile : Profile
{
    public ComicReaderProfile()
    {
        // Customer mappings
        CreateMap<Customer, UserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.IsSubscribed, opt => opt.MapFrom(src => src.IsSubscribe))
            .ForMember(dest => dest.SubscribeEndDate, opt => opt.MapFrom(src => src.SubscribeEndDate));

        CreateMap<RegisterDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User")) 
            .ForMember(dest => dest.IsSubscribe, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.SubscribeEndDate, opt => opt.MapFrom(src => (DateTime?)null))
            .ForMember(dest => dest.SubscribeHistories, opt => opt.Ignore());

        // Comic mappings
        CreateMap<AddNewComicDto, Comic>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.TotalChapter, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.Chapters, opt => opt.Ignore());

        CreateMap<Comic, ReadComicDto>()
            .ForMember(dest => dest.ComicId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.IsPremium, opt => opt.MapFrom(src => src.IsPremium))
            .ForMember(dest => dest.SelectedChapterId, opt => opt.Ignore())
            .ForMember(dest => dest.Chapters, opt => opt.MapFrom(src => src.Chapters.OrderBy(c => c.ChapterNumber)))
            .ForMember(dest => dest.Pages, opt => opt.Ignore()); 

        // Chapter mappings
        CreateMap<AddNewChapterDto, Chapter>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateUploaded, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.TotalPage, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.Pages, opt => opt.Ignore())
            .ForMember(dest => dest.Comic, opt => opt.Ignore());

        // Mapping untuk ChapterInfo (nested di ReadComicDto)
        CreateMap<Chapter, ChapterInfo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ChapterNumber, opt => opt.MapFrom(src => src.ChapterNumber))
            .ForMember(dest => dest.TotalPage, opt => opt.MapFrom(src => src.TotalPage))
            .ForMember(dest => dest.DateUploaded, opt => opt.MapFrom(src => src.DateUploaded));

        // Page mappings
        CreateMap<AddPageDto, Page>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ChapterId, opt => opt.MapFrom(src => src.ChapterId))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageUrl, opt => opt.MapFrom(src => src.PageUrl))
            .ForMember(dest => dest.Chapter, opt => opt.Ignore());

        CreateMap<UpdatePageDto, Page>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id tetap dari sumber
            .ForMember(dest => dest.ChapterId, opt => opt.Ignore()) // tidak boleh update chapterId
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.PageUrl, opt => opt.MapFrom(src => src.PageUrl))
            .ForMember(dest => dest.Chapter, opt => opt.Ignore());

        // Mapping untuk PageInfo (nested di ReadComicDto)
        CreateMap<Page, PageInfo>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.PageUrl));

        // SubscribeHistory optional (jika diperlukan untuk response)
        CreateMap<SubscribeHistory, SubscribeHistoryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod));
    }
}