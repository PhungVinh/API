using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.DTO
{
    public class CRUDAttributeDTO
    {
        public List<AttributeDTO> Attributes { get; set; }
        public string OrgCode { get; set; }
    }
}
