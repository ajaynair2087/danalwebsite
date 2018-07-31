using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanalJSONApp.Models.OTPResponse
{
    public class RootResultsObject
    {
        public Results results { get; set; }
    }
    public class Results
    {
        public string carrier { get; set; }
        public string correlationId { get; set; }
        public string authenticationKey { get; set; }
    }
}
