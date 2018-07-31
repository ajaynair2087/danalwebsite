using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanalJSONApp.Models
{
    public class RootResultsObject
    {
        public Results results { get; set; }
    }
    public class Results
    {
        public string encryptedData { get; set; }
        public string cipherSalt { get; set; }
    }


}