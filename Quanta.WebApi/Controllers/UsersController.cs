using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanta.Domain.Services;
using Quanta.WebApi.Configuration;
using Quanta.WebApi.Extensions.OData;
using Quanta.WebApi.OData;
using Quanta.WebApi.OData.Configuration;
using Device = Quanta.WebApi.OData.Models.Device;
using User = Quanta.WebApi.OData.Models.User;

namespace Quanta.WebApi.Controllers
{
    [Authorize]
    [ApiVersion(Constants.Api.V1)]
    [ODataRoutePrefix(Constants.Api.Routes.Users)]
    public class UsersController : ODataController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [ODataRoute("{userId}/devices")]
        [Produces(Constants.Api.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200, Type = typeof(ODataValue<IEnumerable<Device>>))]
        public IActionResult GetDevices(Guid userId)
        {
            if (!_userService.UserExists(userId))
            {
                return NotFound();
            }

            // User can access his/her own devices
            if (User.IsInRole(Constants.Api.Roles.User))
            {
                var currentUserAdId = User.GetUserAdId();
                var user = _userService.GetById<User>(userId);
               
                if (user.UserIdentity != currentUserAdId)
                    return Unauthorized();
            }

            var userDevices = _userService.GetDevices<Device>(userId);

            return Ok(userDevices);
        }

        [ODataRoute("{userId}/devices/$ref")]
        [Authorize(Roles = Constants.Api.ApplicationJson)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateRef([FromODataUri] Guid userId)
        {
            var segment = await Request.GetODataRefIdAsync<Guid>();

            if (segment.Key == Constants.Api.Routes.Devices)
            {
                _userService.AddDevice(userId, segment.Value);
            }
            else
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Roles = Constants.Api.ApplicationJson)]
        [ODataRoute("{userId}/devices/{deviceId}/$ref")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteRef([FromODataUri] Guid userId, Guid deviceId)
        {
            _userService.RemoveDevice(userId, deviceId);

            return NoContent();
        }
    }
}