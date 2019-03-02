using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Quanta.Domain.User
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserDevice.UserDevice> _userDeviceRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Device.Device> _deviceRepository;
        private readonly IMapper _mapper;

        public UserService(
            IRepository<UserDevice.UserDevice> userDeviceRepository, 
            IRepository<User> userRepository,
            IRepository<Device.Device> deviceRepository,
            IMapper mapper
        )
        {
            _userDeviceRepository = userDeviceRepository;
            _userRepository = userRepository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
        }

        public T GetById<T>(Guid userId)
        {
            return _userRepository.All().Where(o => o.Id == userId).ProjectTo<T>().FirstOrDefault();
        }

        public bool UserExists(Guid userId)
        {
            return _userRepository.All().Any(o => o.Id == userId);
        }

        public IQueryable<T> GetAll<T>()
        {
            return _userRepository.All().ProjectTo<T>();
        }

        public IQueryable<TDevice> GetDevices<TDevice>(Guid userId)
        {
            return _userRepository.All()
                .Include(o => o.UserDevices)
                .Where(o => o.Id == userId)
                .SelectMany(o => o.UserDevices.Select(d => d.Device))
                .ProjectTo<TDevice>();
        }

        public T Add<T>(T user)
        {
            var userDto = _mapper.Map<User>(user);

            _userRepository.Add(userDto);
            _userRepository.SaveChanges();

            return _mapper.Map<T>(userDto);
        }

        public T Update<T>(T user)
        {
            var userDto = _mapper.Map<User>(user);

            _userRepository.Update(userDto);
            _userRepository.SaveChanges();

            return _mapper.Map<T>(userDto);
        }

        public void Delete(Guid userId)
        {
            var user = _userRepository.All().FirstOrDefault(o => o.Id == userId);

            if (user == null)
            {
                return;
            }

            _userRepository.Delete(user);
            _userRepository.SaveChanges();
        }

        public T AddNewDevice<T>(Guid userId, T device)
        {
            var deviceDto = _mapper.Map<Device.Device>(device);

            _deviceRepository.Add(deviceDto);
            
            _userDeviceRepository.Add(new UserDevice.UserDevice() { UserId = userId, DeviceId = deviceDto.Id });
            _userDeviceRepository.SaveChanges();

            return _mapper.Map<T>(deviceDto);
        }

        public void RemoveDevice(Guid userId, Guid deviceId)
        {
            var userDevice = _userDeviceRepository
                .All()
                .FirstOrDefault(o => o.UserId == userId && o.DeviceId == deviceId);

            if (userDevice == null) return;

            _userDeviceRepository.Delete(userDevice);
            _userDeviceRepository.SaveChanges();
        }

        public string GetUserDeviceConnectionString(Guid userId, Guid deviceId)
        {
           return _userDeviceRepository.All()
               .Where(o => o.DeviceId == deviceId & o.UserId == userId)
               .Select(o => o.Device.ConnectionString)
               .FirstOrDefault();
        }

        public IQueryable<TDevice> GetRecentDevices<TDevice>(Guid userId)
        {
            var sampleDate = DateTime.Now.AddDays(-7).Date;

            return _userRepository.All()
                .Include(o => o.Sessions)
                .Where(o => o.Id == userId)
                .SelectMany(o => o.UserDevices.Select(d => d.Device))
                .SelectMany(o => o.Sessions)
                .Where(o => o.UserId == userId &&  o.CreatedOn >= sampleDate)
                .OrderByDescending(o => o.CreatedOn)
                .GroupBy(o => o.Device)
                .Select(o => o.Key)
                .ProjectTo<TDevice>();
        }

        public void AddDevice(Guid userId, Guid deviceId)
        {
            _userDeviceRepository.Add(new UserDevice.UserDevice() { UserId = userId, DeviceId = deviceId });
        }
    }
}
