using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DanalJSONApp.Models.MatchResponse
{
    public class RootResultObject
    {
        public class AccountInfo
        {
            public string accountStatus { get; set; }
            public string isPrimaryAccount { get; set; }
            public string accountTenure { get; set; }
            public string mdnTenure { get; set; }
            public string accountType { get; set; }
            public string lastPort { get; set; }
            public string portedFrom { get; set; }
        }

        public class ServiceInfo
        {
            public string serviceProvider { get; set; }
            public string serviceType { get; set; }
            public string countryCode { get; set; }
        }

        public class MatchScores
        {
            public string firstNameScore { get; set; }
            public string lastNameScore { get; set; }
            public string streetAddressScore { get; set; }
            public string cityScore { get; set; }
            public string stateScore { get; set; }
            public string zipCodeScore { get; set; }
            public string emailAddressScore { get; set; }
            public string dataSource { get; set; }
        }

        public class DeviceInfo
        {
            public string vendor { get; set; }
            public string model { get; set; }
        }

        public class TokenInfo
        {
            public string mobileNumberToken { get; set; }
            public string simToken { get; set; }
            public string deviceToken { get; set; }
            public string accountToken { get; set; }
        }

        public class Attributes
        {
            public AccountInfo accountInfo { get; set; }
            public ServiceInfo serviceInfo { get; set; }
            public MatchScores matchScores { get; set; }
            public DeviceInfo deviceInfo { get; set; }
            public TokenInfo tokenInfo { get; set; }
        }

        public class RootObject
        {
            public Attributes attributes { get; set; }
            public string correlationId { get; set; }
            public string referenceId { get; set; }
        }
    }
}