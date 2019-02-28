using System.Linq;
using AutoMapper;
using Quanta.DataAccess.Models;

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
            CreateMap<DataAccess.Models.User, OData.Models.User>()
                .ForMember(o => o.Devices, o => o.MapFrom(p => p.UserDevices.Select(ud => ud.Device)));

            CreateMap<OData.Models.User, DataAccess.Models.User>()
                .ForMember(o => o.UserDevices, p => p.Ignore());
        }

        private void CreateDeviceMappings()
        {
            CreateMap<Device, OData.Models.Device>();

            CreateMap<OData.Models.Device, Device>();
        }
    }
}
