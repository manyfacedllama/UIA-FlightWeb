using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace UIA_Web.Controllers
{
    public class IPLocation
    {

        public string IPAddress { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

    }


    public class HomeController : Controller
    {
        public static string GetExternalIP()

        {


            try

            {

                string externalIP;

                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");

                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))

                             .Matches(externalIP)[0].ToString();

                return externalIP;

            }

            catch
            {
                currency = "$ ";
                return "";
            }

        }
        public string GetIPAddress()
        {
            string ipAddress = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                return ipAddress;
            }
            return HttpContext.Request.UserHostAddress;
        }
        public static IPLocation GetIPLocation(string IPAddress)
        {

            IPLocation iplocation;

            string retJson = DownloadDataNoAuth(string.Format("http://www.freegeoip.net/json/{0}", IPAddress));

            var JO = JObject.Parse(retJson);

            iplocation = new IPLocation();

            iplocation.IPAddress = IPAddress;

            iplocation.CountryCode = JO["country_code"].ToString();

            iplocation.CountryName = JO["country_name"].ToString();

            return iplocation;

        }
        public static string DownloadDataNoAuth(string hostURI)
        {
            string retXml = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(hostURI);
                request.Method = "GET";
                String responseLine = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader sr = new StreamReader(dataStream);

                        retXml = sr.ReadToEnd();

                        sr.Close();

                        dataStream.Close();

                    }

                }

            }
            catch (Exception e)
            {

                retXml = null;
                currency = "$ ";

            }

            return retXml;

        }
        public static string currency;
        public IPLocation iplocation;
        public ActionResult Index()
        { if (currency == null){
                string client_ip = GetIPAddress();
                if(currency == null && !String.IsNullOrWhiteSpace(client_ip))
                {
                    iplocation = GetIPLocation(client_ip);
                    if(currency == null)
                    {
                        currency = "$ ";
                    }
                }
                else
                {
                    currency = "$ ";
                }
            }
            var euroCountries = new List<string>();
            euroCountries.AddRange(new String[] { "DE","DK","SE","IT","NL","PL","NO","FI"});
            if (iplocation.CountryCode == "MY") {
              System.Web.HttpContext.Current.Items["currency"] = "RM ";
                currency = "RM ";

            }
            else if(euroCountries.Contains(iplocation.CountryCode))
            {
                System.Web.HttpContext.Current.Items["currency"] = "€ ";
                currency = "€ ";

            }
            else
            {
                System.Web.HttpContext.Current.Items["currency"] = "$ ";
                currency = "$ ";
            }
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexR()
        {
            return View();
        }
    }
}