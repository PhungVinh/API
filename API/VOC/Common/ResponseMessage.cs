using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VOC.Common
{
    public class ResponseMessage
    {
        public string Type { set; get; }
        public string Title { set; get; }
        public int Status { set; get; }
        public string Path { set; get; }
        public string Message { set; get; }
        public object FieldError { set; get; }
    }
}
