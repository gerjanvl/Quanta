using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Guacamole.Client.Common;

namespace Guacamole.Client
{
    public class GuacamoleClient : IDisposable
    {
        private readonly TcpClient _client;
        private readonly IPEndPoint _endPoint;
        private NetworkStream _stream;
        private CharReader _charReader;
        private CharWriter _charWriter;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public bool Connected => _client.Connected;

        public GuacamoleClient(IPEndPoint endPoint)
        {
            _endPoint = endPoint;
            _client = new TcpClient();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async Task<string> Connect(string protocol, int width, int height, Dictionary<string, object> arguments)
        {
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

            return readyInstruction.Args.ToArray()[0];
        }

        public Task<GuacamoleInstruction> ReadInstruction()
        {
            return ReadInstruction(_cancellationTokenSource.Token);
        }

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

        public Task WriteInstruction(string opCode, string[] args)
        {
            return WriteInstruction(opCode, _cancellationTokenSource.Token, (object[])args);
        }

        public Task WriteInstruction(string opCode, params object[] args)
        {
            return WriteInstruction(opCode, _cancellationTokenSource.Token, args);
        }

        public async Task WriteInstruction(string opCode, CancellationToken cancellationToken, object[] args)
        {
            await WriteInstruction(new GuacamoleInstruction
            {
                OpCode = opCode,
                Args = args.Select(o => o == null ? "" : o.ToString()).ToArray()
            }, cancellationToken);
        }

        public async Task WriteInstruction(GuacamoleInstruction guacamoleInstruction, CancellationToken cancellationToken)
        {
            await _charWriter.Write(guacamoleInstruction.ToString(), cancellationToken);
        }

        public void Disconnect()
        {
            _cancellationTokenSource.Cancel();
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

        public void Dispose()
        {
            _client?.Dispose();
            _stream?.Dispose();
        }
    }
}