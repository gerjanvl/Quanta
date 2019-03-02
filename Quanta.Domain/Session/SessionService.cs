using System;
using System.Linq;
using AutoMapper;

namespace Quanta.Domain.Session
{
    public class SessionService : ISessionService
    {
        private readonly IRepository<Session> _sessionRepository;
        private readonly IMapper _mapper;

        public SessionService(
            IRepository<Session> sessionRepository,
            IMapper mapper
        )
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
        }

        public void StartNew(Guid sessionId, Guid userId, Guid deviceId)
        {
            _sessionRepository.Add(new Session()
            {
                Id = sessionId,
                DeviceId = deviceId,
                UserId = userId
            });

            _sessionRepository.SaveChanges();
        }

        public void Finish(Guid sessionId)
        {
            var session = _sessionRepository.All().First(o => o.Id == sessionId);

            session.FinishedOn = DateTime.Now;

            _sessionRepository.Update(session);
            _sessionRepository.SaveChanges();
        }
    }
}