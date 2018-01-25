using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using NUnit.Framework;
using RaterPrice.Persistence.Domain;

namespace RaterPrice.Tests
{

    public class RequestTests
    {
        private static string Get(string url)
        {
            var req = System.Net.WebRequest.Create(url);
            var resp = req.GetResponse();
            var stream = resp.GetResponseStream();
            if (stream == null) return null;
            var sr = new System.IO.StreamReader(stream);
            var Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

        private static string Post(string url, string data)
        {
            var req = System.Net.WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 100000;
            req.ContentType = "application/x-www-form-urlencoded";
            var sentData = Encoding.GetEncoding(1251).GetBytes(data);
            req.ContentLength = sentData.Length;
            var sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            var res = req.GetResponse();
            var receiveStream = res.GetResponseStream();
            if (receiveStream == null) return null;
            var sr = new System.IO.StreamReader(receiveStream, Encoding.UTF8);
            var read = new char[256];
            var count = sr.Read(read, 0, 256);
            var Out = string.Empty;
            while (count > 0)
            {
                var str = new string(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }
            return Out;
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["raterPriceConnectionString"].ConnectionString;
        }

        private static string GetBaseUrl()
        {
            // return "http://91.195.100.148/";
            return "http://localhost/";
        }

        [Test]
        public void GetShopsTest()
        {
            var connectionString = GetConnectionString();
            const int cityId = 8;
            const string querySearch = "О";
            var url = GetBaseUrl() + $"api/shops/search?cityId={cityId}&querySearch={querySearch}";
            var r = Get(url);
            var list = new JavaScriptSerializer().Deserialize<List<Shop>>(r);
            using (RaterPriceContext context = RaterPriceContext.Create())
            {
                var shops = context.Shops.Where(s => s.CityId == cityId && s.Name.IndexOf(querySearch) > -1).Select(s=>s);
                Assert.AreEqual(list.Count(), shops.Count());
                foreach (var s in shops)
                {
                    var shopId = s.Id;
                    var shopCityId = s.CityId;
                    var shopName = s.Name;
                    Assert.AreEqual(list.Any(x => x.Name == shopName && x.Id == shopId && x.CityId == shopCityId), true);
                }
            }
        }

        [Test]
        public void GetCitiesTest()
        {
            var connectionString = GetConnectionString();
            var url = GetBaseUrl() + "/api/cities";
            var r = Get(url);
            var list = new JavaScriptSerializer().Deserialize<List<City>>(r);
            using (var context = new RaterPriceContext())
            {
                var cities = context.Cities.Select(c => c);
                Assert.AreEqual(list.Count(), cities.Count());
                foreach (var c in cities)
                {
                    var id = c.Id;
                    var name = c.Name;
                    Assert.AreEqual(list.Any(x => x.Name == name && x.Id == id), true);
                }
            }
        }
    }
}
