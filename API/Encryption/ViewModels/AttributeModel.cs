using Encryption.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.ViewModels
{
    public class AttributeModel
    {        
        public List<AttributeDTO> tblVocattributes { get; set; }
        public string UserName { get; set; }
    }
}
