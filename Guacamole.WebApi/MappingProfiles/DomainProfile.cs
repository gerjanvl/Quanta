using System.Linq;
using AutoMapper;
using Guacamole.DataAccess.Models;

namespace Guacamole.WebApi.MappingProfiles
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<DataAccess.Models.User, OData.Models.User>()
                .ForMember(o => o.Devices, o => o.MapFrom(p => p.UserDevices.Select(ud => ud.Device)));

            CreateMap<OData.Models.User, DataAccess.Models.User>()
                .ForMember(o => o.UserDevices, p => p.Ignore());

            CreateMap<Device, OData.Models.Device>();

            CreateMap<OData.Models.Device, Device>();
        }
    }
}
