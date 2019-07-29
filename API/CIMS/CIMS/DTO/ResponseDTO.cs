using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIMS.DTO
{
    public class ResponseDTO
    {
        public int StatusCode { get; set; }
        public dynamic Response { get; set; }
    }
}
