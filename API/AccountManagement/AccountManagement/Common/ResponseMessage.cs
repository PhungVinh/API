namespace AccountManagement.Common
{
    public class ResponseMessage
    {
        public string Type { set; get; }
        public string Title { set; get; }
        public int Status { set; get; }
        public string Path { set; get; }
        public string Message { set; get; }
        public object fieldError { set; get; }
        // Validate input
    }

    public class ErrorObject
    {
        public string entityName { set; get; }
        public string errorKey { set; get; }
        public string type { set; get; }
        public string title { set; get; }
        public int status { set; get; }
        public string message { set; get; }
        public string paramss { set; get; }
        public object Obj { get; set; }
        // Database
    }

    public class FieldErrors
    {
        public string objectName { set; get; }
        public string field { set; get; }
        public string message { set; get; }

    }
}
