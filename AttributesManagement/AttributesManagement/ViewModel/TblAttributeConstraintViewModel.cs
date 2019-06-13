using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttributesManagement.ViewModel
{
    public class TblAttributeConstraintViewModel
    {
        public Int64 index { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string ContraintsValue { get; set; }
        public string LinkContraints { get; set; }

        public string chkDelete { get; set; }
    }
   
}
