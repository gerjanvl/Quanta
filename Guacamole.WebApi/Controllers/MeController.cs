using Guacamole.Domain.Services;
using Guacamole.WebApi.OData;
using Guacamole.WebApi.OData.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Guacamole.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ODataRoutePrefix("me")]
    public class MeController : ODataController
    {
        private readonly UserService _userService;

        public MeController(UserService userService)
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
