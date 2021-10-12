using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Error
{
    public class APIException
    {
        public APIException(int statusCode,string message=null,string details=null )
        {
            StatusCode=statusCode;
            Message=message;
            Details=details;
        }
        public int StatusCode{get;set;}
        public string Message{get;set;}

        public string Details{get;set;}
    }
}