using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using System.Text;
using DanalJSONApp.Models;
using DanalJSONApp.Models.OTPResponse;
using DanalJSONApp.Util;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace DanalJSONApp.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult SendOTP(string PhoneNumber)
        {
            try
            {
                //generateStaticEVURL(PhoneNumber);
                
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string url = "https://api-sbox.dnlsrv.com/cigateway/id/v1/sendOtp";
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                myHttpWebRequest.Method = "POST";
                WebHeaderCollection myWebHeaderCollection = myHttpWebRequest.Headers;
                if (!WebHeaderCollection.IsRestricted("Accept"))
                {
                    myWebHeaderCollection.Add("Accept", "application/json");
                }
                if (!WebHeaderCollection.IsRestricted("Content-Type"))
                {
                    myWebHeaderCollection.Add("Content-Type", "application/json");
                }
                if (!WebHeaderCollection.IsRestricted("Authorization"))
                {
                    myWebHeaderCollection.Add("Authorization", "M58Kf103FkYNgFbrrQkl2sSAO6lUBdau");
                }
                if (!WebHeaderCollection.IsRestricted("RequestTime"))
                {
                    myWebHeaderCollection.Add("RequestTime", ConfigurationManager.AppSettings["RequestTime"]);
                }

                using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                {
                    string json = "{\r\n\"merchantId\":\"02180007169632\",\r\n\"correlationId\":\""+ RandomString(16) +"\",\r\n\"consumerMdn\":\"" + PhoneNumber + "\"\r\n}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                var streamReader = new StreamReader(httpResponse.GetResponseStream());
                string responseText = streamReader.ReadToEnd();

                DanalJSONApp.Models.OTPResponse.RootResultsObject _res = new JavaScriptSerializer().Deserialize<DanalJSONApp.Models.OTPResponse.RootResultsObject>(responseText);
          
                return Json(_res.results.authenticationKey);
            }
            catch (Exception ex)
            {
                return Json("fail");
            }


            


            //{\r\n\"merchantId\":\"00DF00000015\",\r\n\"correlationId\":\"ABC0881973286793\",\r\n\"consumerMdn\":\"+14085551234\"\r\n}


            /*var accountSid = ConfigurationManager.AppSettings["TwilioAccountSid"];
            var authToken = ConfigurationManager.AppSettings["TwilioAuthToken"];
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber(PhoneNumber);
            var from = new PhoneNumber(ConfigurationManager.AppSettings["MyDanalPhoneNumber"]);

            var message = MessageResource.Create(to: to,
                                                    from: from,
                                                    body:"Your OTP is: 1111");

            return Content(message.Sid);*/

        }


        public ActionResult validateOTP(string otp, string authKey)
        {
            //validateOTP
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string url2 = "https://api-sbox.dnlsrv.com/cigateway/id/v1/validateotp";
                HttpWebRequest myHttpWebRequest2 = (HttpWebRequest)WebRequest.Create(url2);
                myHttpWebRequest2.Method = "POST";
                WebHeaderCollection myWebHeaderCollection2 = myHttpWebRequest2.Headers;
                if (!WebHeaderCollection.IsRestricted("Accept"))
                {
                    myWebHeaderCollection2.Add("Accept", "application/json");
                }
                if (!WebHeaderCollection.IsRestricted("Content-Type"))
                {
                    myWebHeaderCollection2.Add("Content-Type", "application/json");
                }
                if (!WebHeaderCollection.IsRestricted("Authorization"))
                {
                    myWebHeaderCollection2.Add("Authorization", "M58Kf103FkYNgFbrrQkl2sSAO6lUBdau");
                }
                if (!WebHeaderCollection.IsRestricted("RequestTime"))
                {
                    myWebHeaderCollection2.Add("RequestTime", ConfigurationManager.AppSettings["RequestTime"]);
                }

                using (var streamWriter = new StreamWriter(myHttpWebRequest2.GetRequestStream()))
                {
                    string json = "{\r\n\"merchantId\":\"02180007169632\",\r\n\"correlationId\":\"" + RandomString(16) +"\",\r\n\"otp\": \"" + otp +"\",\r\n\"authenticationKey\":\"" + authKey+"\"\r\n}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }



                var httpResponse2 = (HttpWebResponse)myHttpWebRequest2.GetResponse();
                var streamReader = new StreamReader(httpResponse2.GetResponseStream());
                string responseText2 = streamReader.ReadToEnd();
                httpResponse2.Close();


                return Json("Success");

            }
            catch (Exception ex)
            {
                return Json("Fail");

            }
        }

        public static void generateStaticEVURL(string PhoneNumber)
        {
            string baseURL = "http://mi-sbox.dnlsrv.com/msbox/id/OjR4DtTW";
            string correlationID = RandomString(16);
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            string nonce = GenerateRandomNumbers(5);
            string redirectURL = ConfigurationManager.AppSettings["BaseUrl"];
            string matchObject = "\"match\": {\r\"consumermdn\":\"" + PhoneNumber + "\"\r}";

            string data = "correlationid=" + correlationID + "&timestamp=" + timestamp + "&nonce=" + nonce + "&match=" + matchObject;
            string Base64_data = MyCryptoClass.Base64Encode(data);
            string urlEncode_data = MyCryptoClass.EncodeBase64Url(Base64_data);
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateRandomNumbers(int length = 10)
        {
            //Initiate objects & vars  
            byte[] seed = Guid.NewGuid().ToByteArray();
            Random random = new Random(BitConverter.ToInt32(seed, 0));
            int randNumber = 0;
            //Loop ‘length’ times to generate a random number or character
            String randomNumber = "";
            for (int i = 0; i < length; i++)
            {
                randNumber = random.Next(48, 58);
                randomNumber = randomNumber + (char)randNumber;
                //append random char or digit to randomNumber string

            }
            return randomNumber;
        }

    }
}