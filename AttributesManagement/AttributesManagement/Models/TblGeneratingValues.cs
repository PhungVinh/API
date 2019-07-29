using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblGeneratingValues
    {
        public int Id { get; set; }
        public string AttributeCode { get; set; }
        public string MenuCode { get; set; }
        public string IsReuse { get; set; }
        public string InputFormat { get; set; }
        public int? MinimumLenght { get; set; }
        public int? MaxLenght { get; set; }
        public string ExclusionCharacters { get; set; }
        public string RequiredCharacters { get; set; }
        public bool? IsCapitalizeLetter { get; set; }
        public bool? IsLowercaseLetter { get; set; }
        public bool? IsSpecialCharacters { get; set; }
    }
}
