using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanta.Domain.Services;
using Device = Quanta.WebApi.OData.Models.Device;

namespace Quanta.WebApi.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [ODataRoutePrefix("devices")]
    public class DevicesController : ODataController
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [ODataRoute]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(ODataValue<IEnumerable<Device>>))]
        [EnableQuery(MaxTop = 1000, AllowedQueryOptions = AllowedQueryOptions.All)]
        public IActionResult GetDevices()
        {
            return Ok(_deviceService.GetAll<Device>());
        }

        [ODataRoute("{deviceId}")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(Device))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetDevice(Guid deviceId)
        {
            var device = _deviceService.GetById<Device>(deviceId);

            if (device == null) return NotFound();

            return Ok(device);
        }

        [HttpPost]
        [ODataRoute]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(Device))]
        public IActionResult AddDevice([FromBody] Device device)
        {
            device = _deviceService.Add(device);

            return Created(device);
        }

        [HttpPut]
        [ODataRoute("{deviceId}")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(Device))]
        public IActionResult UpdateDevice([FromODataUri] Guid deviceId, [FromBody] Device device)
        {
            if (!_deviceService.DeviceExists(deviceId))
            {
                return NotFound();
            }

            device = _deviceService.Update(device);

            return Updated(device);
        }

        [HttpDelete]
        [ODataRoute("{deviceId}")]
        [Produces("application/json")]
        [Authorize(Roles = "Admin, Manager")]
        [ProducesResponseType(200, Type = typeof(Device))]
        public IActionResult DeleteDevice([FromODataUri] Guid deviceId)
        {
            if (!_deviceService.DeviceExists(deviceId))
            {
                return NotFound();
            }

            _deviceService.Delete(deviceId);

            return Ok();
        }
    }
}
