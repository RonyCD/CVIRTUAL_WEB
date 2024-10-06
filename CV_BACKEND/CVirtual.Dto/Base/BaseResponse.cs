using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVirtual.Dto.Base
{
    public class BaseResponse <T>
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<MessageResponse> Validations { get; set; }
        public T Data { get; set; }
    }
}
