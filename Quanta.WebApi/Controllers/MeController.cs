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
            var userId = User.GetUserAdId();

            var user = _userService.GetById<User>(userId);

            if (user == null) return Unauthorized();

            return Ok(user);
        }

        [ODataRoute("devices")]
        public IActionResult GetDevices()
        {
            var userId = User.GetUserAdId();

            if (_userService.UserExists(userId)) return Unauthorized();

            var devices = _userService.GetDevices<Device>(userId);

            return Ok(devices);
        }

        [ODataRoute("recentDevices")]
        public IActionResult GetRecentDevices()
        {
            var userId = User.GetUserAdId();

            if (_userService.UserExists(userId)) return Unauthorized();

            var devices = _userService.GetRecentDevices<Device>(userId);

            return Ok(devices);
        }
    }
}
