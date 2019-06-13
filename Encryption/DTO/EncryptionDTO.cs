using Encryption.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.DTO
{
    public class EncryptionDTO
    {
        public string AttributeCode { get; set; }
        public string MenuCode { get; set; }
        public string AttributeLabel { get; set; }
        public string ModuleName { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ExecuteTime { get; set; }
        public bool? EncryptionStatus { get; set; }
    }
}
