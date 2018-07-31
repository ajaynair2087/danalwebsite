using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanalJSONApp.Models.ConsumerInfo
{
    public class RootObject
    {
        public string consumerLastName { get; set; }
        public string consumerCountryCode { get; set; }
        public string consumerCity { get; set; }
        public string consumerPostalCode { get; set; }
        public string consumerAddress2 { get; set; }
        public string correlationId { get; set; }
        public string consumerState { get; set; }
        public string consumerAddress1 { get; set; }
        public string referenceId { get; set; }
        public string consumerEmail { get; set; }
        public string consumerFirstName { get; set; }
    }
}