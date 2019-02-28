using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guacamole.Client.Common
{
    public class CharReader
    {
        private readonly Stream _stream;
        private readonly byte[] _buffer = new byte[512];
        private int _currentPosition;
        private int _bufferSize;

        public CharReader(Stream stream)
        {
            _stream = stream;
        }

        public async Task<char> ReadNext(CancellationToken cancellationToken)
        {
            if (_currentPosition == 0)
            {
                _bufferSize = await _stream.ReadAsync(_buffer, 0, _buffer.Length, cancellationToken);
            }
            else if (_currentPosition == _bufferSize)
            {
                _bufferSize = await _stream.ReadAsync(_buffer, 0, _buffer.Length, cancellationToken);
                _currentPosition = 0;
            }

            return (char)_buffer[_currentPosition++];
        }

        public async Task<byte[]> ReadNextBytes(int count, CancellationToken cancellationToken)
        {
            using (var ms = new MemoryStream(count))
            {
                while (true)
                {
                    var bytesToRead = count - (int)ms.Position;
                    var bytesInBufferLeft = _bufferSize - _currentPosition;
                   
                    if (bytesInBufferLeft >= bytesToRead)
                    {
                        await ms.WriteAsync(_buffer, _currentPosition, bytesToRead, cancellationToken);

                        _currentPosition += bytesToRead;

                        return ms.ToArray();
                    }

                    await ms.WriteAsync(_buffer, _currentPosition, bytesInBufferLeft, cancellationToken);

                    _bufferSize = await _stream.ReadAsync(_buffer, 0, _buffer.Length, cancellationToken);

                    _currentPosition = 0;
                }
            }
        }

        public async Task<string> ReadNext(int count, CancellationToken cancellationToken)
        {
            return Encoding.UTF8.GetString(await ReadNextBytes(count, cancellationToken));
        }
    }
}