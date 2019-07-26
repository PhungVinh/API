using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.Common
{
    public class ErrorObject
    {
        public string EntityName { set; get; }
        public string ErrorKey { set; get; }
        public string Type { set; get; }
        public string Title { set; get; }
        public int Status { set; get; }
        public string Message { set; get; }
        public string Paramss { set; get; }
        public object Obj { get; set; }
    }
}
