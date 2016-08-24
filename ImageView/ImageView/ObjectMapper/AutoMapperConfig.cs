using AutoMapper;
using ImageView.DataContracts;
using ImageView.Models.Implementation;

namespace ImageView.ObjectMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Configuration { get; private set; }
        public static void RegisterMappings()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<BookmarkContainerImplementation, BookmarkContainer>();
                cfg.CreateMap<BookmarkFolderImplementation, BookmarkFolder>();
                cfg.AddProfile<OrganizationProfile>();
            });
            Configuration = config.CreateMapper();
           
        }
    }
}
