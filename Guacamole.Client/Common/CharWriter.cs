using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guacamole.Client.Common
{
    public class CharWriter
    {
        private readonly Stream _stream;
        private readonly byte[] _buffer = new byte[512];

        public CharWriter(Stream stream)
        {
            _stream = stream;
        }

        public async Task Write(string text, CancellationToken cancellationToken)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var bytesWritten = 0;

            using (var ms = new MemoryStream(bytes))
            {
                while (true)
                {
                    var buffer = new byte[_buffer.Length];

                    if (bytesWritten + _buffer.Length > bytes.Length)
                    {
                        var bytesToWrite = bytes.Length - bytesWritten;

                        await ms.ReadAsync(buffer, 0, bytesToWrite, cancellationToken);
                        await _stream.WriteAsync(buffer, 0, bytesToWrite, cancellationToken);

                        return;
                    }

                    await ms.ReadAsync(buffer, 0, _buffer.Length, cancellationToken);
                    await _stream.WriteAsync(buffer, 0, _buffer.Length, cancellationToken);

                    bytesWritten += _buffer.Length;
                }
            }
        }
    }
}