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
using DanalJSONApp.Util;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace DanalJSONApp.Controllers
{
    public class HomeController : TwilioController
    {
        public ActionResult Index()
        {
            Object ConsumerInfoObj = null;
            var baseUrl = System.Configuration.ConfigurationManager.AppSettings["BaseUrl"];
            string url = baseUrl + @"/Json/shopItems.json";

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string responseCIText = streamReader.ReadToEnd();
                List<ShopItems> SIList = JsonConvert.DeserializeObject<List<ShopItems>>(responseCIText);
                ViewBag.ShopList = SIList;

            }
            return View();
        }

        [HttpGet]
        public ActionResult RunConsumerInfoApi(string billingPostalCode, string authKey)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //Create a web request for "www.msn.com".
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://api-sbox.dnlsrv.com/cigateway/id/v1/consumerInfo");
                myHttpWebRequest.Method = "POST";

                //Get the headers associated with the request.
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
                    string json = "{ \"merchantId\" : \"02180007169632\", " +
                                    "\"authenticationKey\" : \"" + authKey +"\", " +
                                    "\"intendedUseCase\" : \"AR\"," +
                                    "\"consumerAuth\" : \"" + billingPostalCode +"\"," +
                                    "\"consumerAuthType\" : \"PostalCode\"," +
                                    "\"correlationId\" : \"" + RandomString(16) +"\"" +
                        " }";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string responseText = streamReader.ReadToEnd();

                    Object _res = new JavaScriptSerializer().Deserialize<RootResultsObject>(responseText);
                    string key = "e6WMHBIw94OJBoaii3+S9Q==";
                    string cipherSalt = ((DanalJSONApp.Models.RootResultsObject)_res).results.cipherSalt.Trim();
                    string encryptedData = ((DanalJSONApp.Models.RootResultsObject)_res).results.encryptedData;

                    string decryptedData = MyCryptoClass.AES_decrypt(encryptedData, key, MyCryptoClass.Base64Encode(cipherSalt));
                    Object consumerInfoObj = new JavaScriptSerializer().Deserialize<DanalJSONApp.Models.ConsumerInfo.RootObject>(decryptedData);
                    return Json(consumerInfoObj,JsonRequestBehavior.AllowGet); 

                }
            }
            catch(Exception ex)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MatchAndAttributes(string AuthKey, string firstName, string LastName, string email, string streetAddress1,
                                                string streetAddress2, string City)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //Create a web request for "www.msn.com".
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://api-sbox.dnlsrv.com/cigateway/id/v1/matchandattributes");
                myHttpWebRequest.Method = "POST";

                //Get the headers associated with the request.
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

                string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                string consentID = RandomString(22);

                using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                {
                    string json = "{\r\n   \"intendedUseCase\": \"RM\",\r\n   \"merchantId\": \"02180007169632\",\r\n   \"identity\": {\r\n       \"consumerFirstName\": \""+ firstName +"\",\r\n       \"consumerLastName\": \""+ LastName+"\",\r\n       \"consumerAddress1\": \""+ streetAddress1 + "\",\r\n   \"consumerAddress2\": \"\",\r\n     \"consumerCity\": \"" + City +"\",\r\n       \"consumerState\": \"\",\r\n       \"consumerPostalCode\": \"\",\r\n       \"consumerEmailAddress\": \"" + email +"\"\r\n   },\r\n   \"correlationId\": \"" + RandomString(16) +"\",\r\n   \"authenticationKey\": \""+AuthKey+ "\",\r\n  \"attributeGroups\": \"matchScores,serviceInfo,accountInfo,additionalAccountInfo,deviceInfo,tokenInfo,changeInfo,velocityInfo\",\r\n  \"consentId\" : \"" + consentID +"\",\r\n    \"consentTimeStamp\":\"" +timestamp +"\"\r\n}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string responseText = streamReader.ReadToEnd();

                    Object _res = new JavaScriptSerializer().Deserialize<RootResultsObject>(responseText);
                    string key = "e6WMHBIw94OJBoaii3+S9Q==";
                    string cipherSalt = ((DanalJSONApp.Models.RootResultsObject)_res).results.cipherSalt.Trim();
                    string encryptedData = ((DanalJSONApp.Models.RootResultsObject)_res).results.encryptedData;

                    string decryptedData = MyCryptoClass.AES_decrypt(encryptedData, key, MyCryptoClass.Base64Encode(cipherSalt));
                    Object matchScoreObj = new JavaScriptSerializer().Deserialize<DanalJSONApp.Models.MatchResponse.RootResultObject.RootObject>(decryptedData);

                    int firstNameScore = Convert.ToInt32(((DanalJSONApp.Models.MatchResponse.RootResultObject.RootObject)matchScoreObj).attributes.matchScores.firstNameScore);
                    int lastNameScore = Convert.ToInt32(((DanalJSONApp.Models.MatchResponse.RootResultObject.RootObject)matchScoreObj).attributes.matchScores.lastNameScore);
                    if(firstNameScore < 5 && lastNameScore < 5)
                    {
                        return Json("scorefail", JsonRequestBehavior.AllowGet);
                    }

                    return Json("done", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json("fail", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}