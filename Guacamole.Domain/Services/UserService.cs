using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Guacamole.DataAccess;
using Guacamole.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Guacamole.Domain.Services
{
    public class UserService
    {
        private readonly IRepository<UserDevice> _userDeviceRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Device> _deviceRepository;
        private readonly IMapper _mapper;

        public UserService(
            IRepository<UserDevice> userDeviceRepository, 
            IRepository<User> userRepository,
            IRepository<Device> deviceRepository,
            IMapper mapper
        )
        {
            _userDeviceRepository = userDeviceRepository;
            _userRepository = userRepository;
            _deviceRepository = deviceRepository;
            _mapper = mapper;
        }

        public T GetById<T>(int userId)
        {
            return _userRepository.All().Where(o => o.Id == userId).ProjectTo<T>().FirstOrDefault();
        }

        public T GetByAdIdentifier<T>(Guid adIdentifier)
        {
            return _userRepository.All().Where(o => o.UserIdentity == adIdentifier).ProjectTo<T>().FirstOrDefault();
        }

        public bool UserExists(int userId)
        {
            return _userRepository.All().Any(o => o.Id == userId);
        }

        public IQueryable<T> GetAll<T>()
        {
            return _userRepository.All().ProjectTo<T>();
        }

        public IQueryable<TDevice> GetDevices<TDevice>(int userId)
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

        public void Delete(int userId)
        {
            var user = _userRepository.All().FirstOrDefault(o => o.Id == userId);

            if (user == null)
            {
                return;
            }

            _userRepository.Delete(user);
            _userRepository.SaveChanges();
        }

        public T AddNewDevice<T>(int userId, T device)
        {
            var deviceDto = _mapper.Map<Device>(device);

            _deviceRepository.Add(deviceDto);
            
            _userDeviceRepository.Add(new UserDevice() { UserId = userId, DeviceId = deviceDto.Id });
            _userDeviceRepository.SaveChanges();

            return _mapper.Map<T>(deviceDto);
        }

        public void RemoveDevice(int userId, int deviceId)
        {
            var userDevice = _userDeviceRepository
                .All()
                .FirstOrDefault(o => o.UserId == userId && o.DeviceId == deviceId);

            if (userDevice == null) return;

            _userDeviceRepository.Delete(userDevice);
            _userDeviceRepository.SaveChanges();
        }

        public string GetUserDeviceConnectionString(int userId, int deviceId)
        {
           return _userDeviceRepository.All().Where(o => o.DeviceId == deviceId & o.UserId == userId).Select(o => o.Device.ConnectionString).FirstOrDefault();
        }

        public void AddDevice(int userId, int deviceId)
        {
            _userDeviceRepository.Add(new UserDevice() { UserId = userId, DeviceId = deviceId });
        }
    }
}
