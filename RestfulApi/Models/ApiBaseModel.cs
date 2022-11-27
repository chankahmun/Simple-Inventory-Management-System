using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    public class ApiBaseModel
    {
        public const string PARAMETERS_MISSING = "Missing parameter";

        public const string INVALID_SUPPLIER = "Invalid suppliers Code";

        public const string INVALID_SKU = "Invalid SDK. Value must be more than 0";

        public const string SERVER_ERR = "Server Code Error is occured. Please try again later";

        public const string INVALID_EMAIL = "Invalid email format. eg: example@gmail.com";

        public const string INVALID_PHONE_NO = "Invalid phone no format. eg: 011-111-1111";

        public class ResponseModel
        {
            public bool Status { set; get; } = false;
            public string Err { set; get; } = "";
            public object Data { set; get; }

        }

        public class RequestModel
        {
            public Dictionary<string, string> Params { get; set; }

        }

    
    }
}