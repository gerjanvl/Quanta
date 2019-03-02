using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Quanta.Domain;
using Quanta.Domain.Models;

namespace Quanta.Infrastructure.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IRepository<Device> _deviceRepository;
        private readonly IMapper _mapper;

        public DeviceService(
            IRepository<Device> deviceRepository,
            IMapper mapper
        )
        {
            _deviceRepository = deviceRepository;
            _mapper = mapper;
        }

        public T GetById<T>(Guid deviceId)
        {
            return _deviceRepository.All().Where(o => o.Id == deviceId).ProjectTo<T>().FirstOrDefault();
        }

        public bool DeviceExists(Guid deviceId)
        {
            return _deviceRepository.All().Any(o => o.Id == deviceId);
        }

        public IQueryable<T> GetAll<T>()
        {
            return _deviceRepository.All().ProjectTo<T>();
        }

        public T Add<T>(T device)
        {
            var deviceDto = _mapper.Map<Device>(device);

            _deviceRepository.Add(deviceDto);
            _deviceRepository.SaveChanges();

            return _mapper.Map<T>(deviceDto);
        }

        public T Update<T>(T device)
        {
            var deviceDto = _mapper.Map<Device>(device);

            deviceDto.LastModified = DateTime.Now;

            _deviceRepository.Update(deviceDto);
            _deviceRepository.SaveChanges();

            return _mapper.Map<T>(deviceDto);
        }

        public void Delete(Guid deviceId)
        {
            var device = _deviceRepository.All().FirstOrDefault(o => o.Id == deviceId);

            if (device == null)
            {
                return;
            }

            _deviceRepository.Delete(device);
            _deviceRepository.SaveChanges();
        }
    }
}