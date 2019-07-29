using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.Models
{
    public class DependentValue : TblDependentValues
    {
        public new string[] InputValue { get; set; }
    }
}
