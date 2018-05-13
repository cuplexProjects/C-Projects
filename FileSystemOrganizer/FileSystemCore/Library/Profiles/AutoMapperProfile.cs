using Hangfire.Annotations;

namespace FileSystemCore.Library.Profiles
{
    [UsedImplicitly]
    public class AutoMapperProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.
        /// </summary>
        public AutoMapperProfile()
        {
            //SearchProfileModel.CreateMappings(this);
            //ApplicationSettingsModel.CreateMappings(this);
            //ComputedFileHashModel.CreateMappings(this);
            //FileHashCollection.CreateMappings(this);
        }
    }
}