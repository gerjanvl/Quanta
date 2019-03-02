using System;
using System.Linq;

namespace Quanta.Infrastructure.Services
{
    public interface IUserService
    {
        IQueryable<T> GetAll<T>();

        T GetById<T>(Guid userId);

        T Add<T>(T user);

        T Update<T>(T user);

        void Delete(Guid userId);

        bool UserExists(Guid userId);

        void AddDevice(Guid userId, Guid deviceId);

        void RemoveDevice(Guid userId, Guid deviceId);

        IQueryable<TDevice> GetDevices<TDevice>(Guid userId);

        IQueryable<TDevice> GetRecentDevices<TDevice>(Guid userId);

        string GetUserDeviceConnectionString(Guid userId, Guid deviceId);
    }
}