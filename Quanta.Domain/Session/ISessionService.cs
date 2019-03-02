using System;

namespace Quanta.Domain.Session
{
    public interface ISessionService
    {
        void Finish(Guid sessionId);
        void StartNew(Guid sessionId, Guid userId, Guid deviceId);
    }
}