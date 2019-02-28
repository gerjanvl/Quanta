using System.Linq;
using AutoMapper;
using Guacamole.Data.Models;
using Guacamole.WebSocket.OData.Models;
using Guacamole.WebSocket.ViewModels;
using Device = Guacamole.WebSocket.OData.Models.Device;
using User = Guacamole.WebSocket.OData.Models.User;

namespace Guacamole.WebSocket.MappingProfiles
{

    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Data.Models.User, User>()
                .ForMember(o => o.Devices, o => o.MapFrom(p => p.UserDevices.Select(ud => ud.Device)));

            CreateMap<User, Data.Models.User>().ForMember(o => o.UserDevices, p => p.Ignore());

            CreateMap<Data.Models.Device, Device>();
            CreateMap<Device, Data.Models.Device>();
        }
    }
}
