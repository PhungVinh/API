using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VinhDemo.Common
{


    public class ResponseMessage
    {
        public string Title { set; get; }
        public int Status { set; get; }
        public string Message { set; get; }
        public object fieldError { set; get; }
    }
    public class ErrorObject
    {
        public string entityName { set; get; }
        public string errorKey { set; get; }
        public string title { set; get; }
        public int status { set; get; }
        public string message { set; get; }
        // Database
    }
    public class FieldErrors
    {
        public string objectName { set; get; }
        public string field { set; get; }
        public string message { set; get; }

    }
}

