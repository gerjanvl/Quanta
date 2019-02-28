using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Guacamole.Data;
using Guacamole.Data.Models;
using Guacamole.Domain;
using Guacamole.Domain.Services;
using Guacamole.WebSocket.Extensions;
using Guacamole.WebSocket.OData;
using Guacamole.WebSocket.OData.Configuration;
using Guacamole.WebSocket.OData.Models;
using Guacamole.WebSocket.ViewModels;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.UriParser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Device = Guacamole.WebSocket.OData.Models.Device;
using User = Guacamole.WebSocket.OData.Models.User;

namespace Guacamole.WebSocket.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ODataRoutePrefix("users")]
    public class UsersController : ODataController
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [ODataRoute]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(ODataValue<IEnumerable<User>>))]
        [EnableQuery(MaxTop = 1000, AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAll<User>());
        }

        [ODataRoute("{userId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200, Type = typeof(User))]
        public ActionResult<IQueryable<User>> GetUser(int userId)
        {
            var currentUserAdId = User.GetUserAdId();

            var user = _userService.GetById<User>(userId);

            if (user == null) return NotFound();

            // User can only access his own data
            if (User.IsInRole("User") && user.UserIdentity != currentUserAdId) return Unauthorized();

            return Ok(user);
        }

        [ODataRoute("{userId}/devices")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(200, Type = typeof(ODataValue<IEnumerable<Device>>))]
        public IActionResult GetDevices(int userId)
        {

            if (!_userService.UserExists(userId))
            {
                return NotFound();
            }

            // User can access his own devices
            if (User.IsInRole("User"))
            {
                var currentUserAdId = User.GetUserAdId();
                var user = _userService.GetById<User>(userId);
               
                if (user.UserIdentity != currentUserAdId)
                    return Unauthorized();
            }

            var userDevices = _userService.GetDevices<Device>(userId);

            return Ok(userDevices);
        }

        [HttpPost]
        [Produces("application/json")]
        [ODataRoute("{userId}/devices")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(Device))]
        public IActionResult AddDevice([FromODataUri] int userId, [FromBody] Device device)
        {
            if (!_userService.UserExists(userId))
            {
                return NotFound();
            }

            _userService.AddNewDevice(userId, device);

            return Created(device);
        }

        [HttpPost]
        [ODataRoute]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(Device))]
        public IActionResult AddUser([FromBody] User user)
        {
            user = _userService.Add(user);

            return Created(user);
        }

        [HttpPut]
        [ODataRoute("{userId}")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(Device))]
        public IActionResult UpdateUser([FromODataUri] int userId, [FromBody] User user)
        {
            if (!_userService.UserExists(userId))
            {
                return NotFound();
            }

            user = _userService.Update(user);

            return Updated(user);
        }

        [HttpDelete]
        [ODataRoute("{userId}")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(Device))]
        public IActionResult DeleteUser([FromODataUri] int userId)
        {
            if (!_userService.UserExists(userId))
            {
                return NotFound();
            }

            _userService.Delete(userId);

            return Ok();
        }

        [Authorize(Roles = "Admin, Manager")]
        [ODataRoute("{userId}/devices/$ref")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> CreateRef([FromODataUri] int userId)
        {
            var segment = await Request.GetODataRefIdAsync<int>();

            if (segment.Key == DeviceModelConfiguration.Devices)
            {
                _userService.AddDevice(userId, segment.Value);
            }
            else
            {
                return BadRequest();
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin, Manager")]
        [ODataRoute("{userId}/devices/{deviceId}/$ref")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteRef([FromODataUri] int userId, int deviceId)
        {
            _userService.RemoveDevice(userId, deviceId);

            return NoContent();
        }
    }
}