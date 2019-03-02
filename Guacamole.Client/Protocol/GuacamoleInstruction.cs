using System.Collections.Generic;
using System.Linq;

namespace Guacamole.Client.Protocol
{
    public class GuacamoleInstruction
    {
        public GuacamoleInstruction()
        {
            Args = new List<string>();
        }

        public string OpCode { get; set; }

        public ICollection<string> Args { get; set; }

        public override string ToString()
        {
            IEnumerable<string> messageParts = new List<string>()
            {
                OpCode
            };

            messageParts = messageParts
                .Concat(Args)
                .Select(o => $"{o.Length}.{o}");

            return $"{string.Join(",", messageParts)};";
        }
    }
}
