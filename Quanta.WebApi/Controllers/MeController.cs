using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanta.Domain.Services;
using Quanta.WebApi.Extensions.OData;
using Quanta.WebApi.OData;
using Quanta.WebApi.OData.Models;

namespace Quanta.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ODataRoutePrefix("me")]
    public class MeController : ODataController
    {
        private readonly IUserService _userService;

        public MeController(IUserService userService)
        {
            _userService = userService;
        }

        [ODataRoute()]
        [Authorize()]
        public IActionResult Get()
        {
            var user = _userService.GetByAdIdentifier<User>(User.GetUserAdId());

            if (user == null) return Unauthorized();

            return Ok(user);
        }

        [ODataRoute("devices")]
        public IActionResult GetDevices()
        {
            var user = _userService.GetByAdIdentifier<User>(User.GetUserAdId());

            if (user == null) return Unauthorized();

            var devices = _userService.GetDevices<Device>(user.Id);

            return Ok(devices);
        }
    }
}
