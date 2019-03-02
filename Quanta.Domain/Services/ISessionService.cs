using System;

namespace Quanta.Domain.Services
{
    public interface ISessionService
    {
        void Finish(Guid sessionId);
        void StartNew(Guid sessionId, Guid userId, Guid deviceId);
    }
}