using AutoMapper;
using ImageView.DataContracts;
using ImageView.Models.Implementation;

namespace ImageView.ObjectMapper
{
    public class OrganizationProfile: Profile
    {
        protected override void Configure()
        {
            CreateMap<BookmarkContainerBase, BookmarkContainer>();
        }
    }
}
