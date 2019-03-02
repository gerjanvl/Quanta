using System;
using System.Linq;

namespace Quanta.Domain.Services
{
    public interface IDeviceService
    {
        IQueryable<T> GetAll<T>();

        T GetById<T>(Guid deviceId);

        T Update<T>(T device);

        T Add<T>(T device);

        void Delete(Guid deviceId);

        bool DeviceExists(Guid deviceId);
    }
}