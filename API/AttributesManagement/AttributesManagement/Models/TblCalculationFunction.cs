using System;
using System.Collections.Generic;

namespace AttributesManagement.Models
{
    public partial class TblCalculationFunction
    {
        public int Id { get; set; }
        public string IsInputValue { get; set; }
        public string InputValue { get; set; }
        public string SelectValue { get; set; }
    }
}
