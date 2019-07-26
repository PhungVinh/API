using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOC.Models;

namespace VOC.ViewModel
{
    public class ObjectVOCProccess
    {
        //public TblVocprocess vocprocess;
        //public List<UserViewModel> userViewModels;
        //public List<ObjectSteps> objectSteps;
        public string VOCProcessCode;
        public string VOCProcessName;
        public string VOCProcessType;
        public string Description;
        public bool IsActive;
        public int DurationDay;
        public int DurationHour;
        public int DurationMinute;
        public int? CurrentVersion;
        public string CreateBy;
        public DateTime? CreateDate;
        public string UpdateBy;
        public DateTime? UpdateDate;
        public List<UserViewModel> userViewModels;
        public List<ObjectSteps> objectSteps;
    }
}
