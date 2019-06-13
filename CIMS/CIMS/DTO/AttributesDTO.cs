using CIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIMS.DTO
{
    public class AttributesDTO
    {
        public List<TblVocattributes> tblVocattributes { get; set; }
        public string UserName { get; set; }
    }
}
