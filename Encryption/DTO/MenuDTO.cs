using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Encryption.DTO
{
    public class MenuDTO
    {
        public List<EncryptionDTO> AttributesEncryption { get; set; }
        public DateTime SchedulerTime { get; set; }
    }
}
