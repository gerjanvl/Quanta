using System.Linq;
using AutoMapper;
using Quanta.DataAccess.Entities;

namespace Quanta.WebApi.MappingProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateUserMappings();
            CreateDeviceMappings();
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
    }
}
