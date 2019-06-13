using Encryption.Models;
using Encryption.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.DTO
{
    public class AttributeDTO
    {
        public string AttributeCode { get; set; }
        public string AttributeLabel { get; set; }
        public string ParentCode { get; set; }
        public string ModuleName { get; set; }
    }
}
