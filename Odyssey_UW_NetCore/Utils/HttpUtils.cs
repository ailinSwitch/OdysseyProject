using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Odyssey_UW_NetCore.Utils
{
    public class HttpUtils
    {
        private HttpClient http;
        private Task<HttpResponseMessage> httpResponse;
        //private HttpResponseMessage httpResponseMessage;
        private HttpContent responseContent;

        public HttpUtils(string URL)
        {
            http = new HttpClient();
            Uri getUrl = new Uri(URL);
            httpResponse = http.GetAsync(getUrl);
        }
        //private Task<HttpResponseMessage> startHttpInstance(string URL)
        //{
        //    http = new HttpClient();
        //    Uri getUrl = new Uri(URL);
        //    return http.GetAsync(getUrl);
        //}

        private HttpResponseMessage getResponseMessage() => httpResponse.Result;

        public int GetResponseCode() => (int)getResponseMessage().StatusCode;

        public int GetResponseDataId()
        {
            responseContent = getResponseMessage().Content;
            string result = responseContent.ReadAsStringAsync().Result;
            StreamReader sr = new StreamReader(result);
            string json = sr.ReadToEnd();

            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            //dynamic obj = JsonConvert.DeserializeObject<dynamic>(result);
            return jsonObj[0]["userId"];
            //return responseContent.ReadAsStringAsync().Result;
        }

        public string GetResponseData()
        {
            responseContent = getResponseMessage().Content;
            return responseContent.ReadAsStringAsync().Result;
        }

        public void writeFile(Object obj, string fileName)
        {

            string strJson = JsonConvert.SerializeObject(obj);
            string path = $"D:\\VS\\Odyssey_UW_NetCore\\Odyssey_UW_NetCore\\JsonFile\\{fileName}.json";
            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine(strJson.ToString());
                tw.Close();
            }
            //File.WriteAllText(@"D:\VS\Odyssey_UW\Odyssey_UW\Json\AffectedPrograms.json", strJson);
        }

        public static void Main(string[] args)
        {
            HttpUtils http = new HttpUtils("https://jsonplaceholder.typicode.com/posts");
            Example example1 = new Example()
            {
                Id = 1,
                Name = http.GetResponseData(),
                LastName = "Gonzalez",
                Age = 25
            };
            http.writeFile(example1, "nameExample");
            //    string json = JsonConvert.SerializeObject(example1);
            //    string path = @"D:\VS\Odyssey_UW_NetCore\Odyssey_UW_NetCore\JsonFile\example.json";




            //    using (var tw = new StreamWriter(path, true))
            //    {
            //        tw.WriteLine(json.ToString());
            //        tw.Close();
            //    }
            //}


        }

        public class Example
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }

        }

    }
}
