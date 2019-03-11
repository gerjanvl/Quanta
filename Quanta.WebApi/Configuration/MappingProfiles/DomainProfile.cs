using AutoMapper;
using Quanta.Domain.Models;

namespace Quanta.WebApi.Configuration.MappingProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateUserMappings();
            CreateDeviceMappings();
            CreateUserDeviceMappings();
        }

        private void CreateUserMappings()
        {
            CreateMap<User, OData.Models.User>()
                .ForMember(o => o.Devices, o => o.Ignore())
                .ForMember(o => o.RecentDevices, o => o.Ignore());

            CreateMap<OData.Models.User, User>()
                .ForMember(o => o.UserDevices, p => p.Ignore());
        }

        private void CreateDeviceMappings()
        {
            CreateMap<Device, OData.Models.Device>();

            CreateMap<OData.Models.Device, Device>();
        }

        private void CreateUserDeviceMappings()
        {
            CreateMap<Device, OData.Models.UserDevice>();
        }
    }
}
