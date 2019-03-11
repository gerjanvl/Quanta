using System.Collections.Generic;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanta.Infrastructure.Services;
using Quanta.WebApi.Configuration;
using Quanta.WebApi.Extensions.OData;
using Quanta.WebApi.OData;
using Quanta.WebApi.OData.Models;

namespace Quanta.WebApi.Controllers
{
    [Authorize]
    [ApiVersion(Constants.Api.V1)]
    [ODataRoutePrefix(Constants.Api.Routes.Me)]
    public class MeController : ODataController
    {
        private readonly IUserService _userService;

        public MeController(IUserService userService)
        {
            _userService = userService;
        }

        [ODataRoute("devices")]
        [Produces(Constants.Api.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200, Type = typeof(ODataValue<IEnumerable<UserDevice>>))]
        public IActionResult GetDevices()
        {
            var userId = User.GetUserAdId();

            if (!_userService.UserExists(userId)) return NotFound();

            var devices = _userService.GetDevices<UserDevice>(userId);

            return Ok(devices);
        }

        [ODataRoute("recentDevices")]
        [Produces(Constants.Api.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200, Type = typeof(ODataValue<IEnumerable<UserDevice>>))]
        public IActionResult GetRecentDevices()
        {
            var userId = User.GetUserAdId();

            if (!_userService.UserExists(userId)) return NotFound();

            var devices = _userService.GetRecentDevices<UserDevice>(userId);

            return Ok(devices);
        }
    }
}
