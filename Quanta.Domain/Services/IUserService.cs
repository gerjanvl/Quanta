using System;
using System.Linq;

namespace Quanta.Domain.Services
{
    public interface IUserService
    {
        T Add<T>(T user);

        void AddDevice(int userId, int deviceId);

        T AddNewDevice<T>(int userId, T device);

        void Delete(int userId);

        IQueryable<T> GetAll<T>();

        T GetByAdIdentifier<T>(Guid adIdentifier);

        T GetById<T>(int userId);

        IQueryable<TDevice> GetDevices<TDevice>(int userId);

        string GetUserDeviceConnectionString(int userId, int deviceId);

        void RemoveDevice(int userId, int deviceId);

        T Update<T>(T user);

        bool UserExists(int userId);
    }
}