using System.Linq;

namespace Quanta.Domain.Services
{
    public interface IDeviceService
    {
        T Add<T>(T device);

        void Delete(int deviceId);

        bool DeviceExists(int deviceId);

        IQueryable<T> GetAll<T>();

        T GetById<T>(int deviceId);

        T Update<T>(T device);
    }
}