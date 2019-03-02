using System.Collections.Concurrent;
using Guacamole.Client;

namespace Quanta.Infrastructure.Guacamole
{
    public class GuacamoleClientManager
    {
        public ConcurrentDictionary<string, GuacamoleClient> GuacamoleClients { get; set; }

        public GuacamoleClientManager()
        {
            GuacamoleClients = new ConcurrentDictionary<string, GuacamoleClient>();
        }

        public void Add(string connectionId, GuacamoleClient client)
        {
            GuacamoleClients.TryAdd(connectionId, client);
        }

        public GuacamoleClient Get(string connectionId)
        {
            return GuacamoleClients.TryGetValue(connectionId, out var guacamoleClient) ? guacamoleClient : null;
        }

        public void Remove(string connectionId)
        {
            if (!GuacamoleClients.TryRemove(connectionId, out var client)) return;

            client.Disconnect();
            client.Dispose();
        }
    }
}