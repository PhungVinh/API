using CIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIMS.DTO
{
    public class EncryptionDTO : TblEncryption
    {
        public new string UpdateDate { get; set; }
    }
}
