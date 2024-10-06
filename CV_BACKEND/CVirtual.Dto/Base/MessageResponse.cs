using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.Base
{
    public class MessageResponse
    {
        public string Code { get; }
        public string Message { get; }

        public MessageResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
