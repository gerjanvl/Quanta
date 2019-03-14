using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Guacamole.Client.Common;
using Guacamole.Client.Protocol;

namespace Guacamole.Client
{
    /// <summary>
    /// A client to connect guacamole server
    /// </summary>
    public class GuacamoleClient : IDisposable
    {
        public Guid ConnectionId { get; set; }

        public bool Connected => _client.Connected;

        private readonly TcpClient _client;
        private readonly IPEndPoint _endPoint;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private NetworkStream _stream;
        private CharReader _charReader;
        private CharWriter _charWriter;

        public GuacamoleClient(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
            _client = new TcpClient();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Create a new connection with guacamole server.
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="arguments"></param>
        /// <returns>Connection Id</returns>
        public async Task<Guid> Connect(string protocol, int width, int height, Dictionary<string, object> arguments)
        {
            if(Connected) throw new Exception("An connection has already been established");

            await _client.ConnectAsync(_endPoint.Address, _endPoint.Port);

            _stream = _client.GetStream();
            _charReader = new CharReader(_stream);
            _charWriter = new CharWriter(_stream);

            await WriteInstruction("select", protocol);

            var argsInstruction = await ReadInstruction();

            var defaultArguments = argsInstruction.Args.ToList();

            var connectionArguments = OverrideDefaultValues(arguments, defaultArguments);

            await WriteInstruction("size", width, height, 96);
            await WriteInstruction("audio", "audio/ogg");
            await WriteInstruction("video");
            await WriteInstruction("image");

            await WriteInstruction("connect", connectionArguments);

            var readyInstruction = await ReadInstruction();

            var connectionId = Guid.Parse(readyInstruction.Args.ToArray()[0].Split('$')[1]);

            ConnectionId = connectionId;

            return connectionId;
        }

        /// <summary>
        /// Create a new connection with guacamole server with an existing session.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public async Task<Guid> ConnectToExistingConnection(Guid connectionId, int width, int height, Dictionary<string, object> arguments)
        {
            if (Connected) throw new Exception("An connection has already been established");

            await _client.ConnectAsync(_endPoint.Address, _endPoint.Port);

            _stream = _client.GetStream();
            _charReader = new CharReader(_stream);
            _charWriter = new CharWriter(_stream);

            await WriteInstruction("select", $"${connectionId.ToString()}");

            var argsInstruction = await ReadInstruction();

            await WriteInstruction("size", width, height, 96);
            await WriteInstruction("audio", "audio/ogg");
            await WriteInstruction("video");
            await WriteInstruction("image");

            var defaultArguments = argsInstruction.Args.ToList();
            var connectionArguments = OverrideDefaultValues(arguments, defaultArguments);

            await WriteInstruction("connect", connectionArguments);

            var readyInstruction = await ReadInstruction();

            ConnectionId = Guid.Parse(readyInstruction.Args.ToArray()[0].Split('$')[1]);

            return connectionId;
        }

        /// <summary>
        /// Read next guacamole server instruction
        /// </summary>
        /// <returns></returns>
        public Task<GuacamoleInstruction> ReadInstruction()
        {
            return ReadInstruction(_cancellationTokenSource.Token);
        }

        /// <summary>
        /// Read next guacamole server instruction
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<GuacamoleInstruction> ReadInstruction(CancellationToken cancellationToken)
        {
            var instruction = new GuacamoleInstruction();

            var buffer = string.Empty;

            while (true)
            {
                var chr = await _charReader.ReadNext(cancellationToken);

                switch (chr)
                {
                    case '.':
                        {
                            var length = int.Parse(buffer);

                            var instructionPartName = await _charReader.ReadNext(length, cancellationToken);

                            if (instruction.OpCode == null)
                                instruction.OpCode = instructionPartName;
                            else
                                instruction.Args.Add(instructionPartName);

                            buffer = string.Empty;
                            break;
                        }
                    case ',':
                        break;
                    case ';':
                        return instruction;
                    default:
                        buffer += chr;
                        break;
                }
            }
        }

        /// <summary>
        /// Read next guacamole server instruction
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task WriteInstruction(string opCode, string[] args)
        {
            return WriteInstruction(opCode, _cancellationTokenSource.Token, (object[])args);
        }

        /// <summary>
        /// Read next guacamole server instruction
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Task WriteInstruction(string opCode, params object[] args)
        {
            return WriteInstruction(opCode, _cancellationTokenSource.Token, args);
        }

        /// <summary>
        /// Write instruction to guacamole server
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task WriteInstruction(string opCode, CancellationToken cancellationToken, object[] args)
        {
            await WriteInstruction(new GuacamoleInstruction
            {
                OpCode = opCode,
                Args = args.Select(o => o?.ToString() ?? string.Empty).ToArray()
            }, cancellationToken);
        }

        /// <summary>
        /// Write instruction to guacamole server
        /// </summary>
        /// <param name="guacamoleInstruction"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task WriteInstruction(GuacamoleInstruction guacamoleInstruction, CancellationToken cancellationToken)
        {
            await _charWriter.Write(guacamoleInstruction.ToString(), cancellationToken);
        }

        private object[] OverrideDefaultValues(Dictionary<string, object> values, List<string> defaultValues)
        {
            var resultValues = new object[defaultValues.Count];

            foreach (var keyValuePair in values)
            {
                var index = defaultValues.IndexOf(keyValuePair.Key);

                if (index == -1) continue;

                if (keyValuePair.Value is bool)
                {
                    resultValues[index] = keyValuePair.Value.ToString().ToLower();
                }
                else
                {
                    resultValues[index] = keyValuePair.Value;
                }
            }

            return resultValues;
        }

        /// <summary>
        /// Disconnect the current connection with guacamole server
        /// </summary>
        /// <returns></returns>
        public void Disconnect()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _stream?.Dispose();
        }
    }
}