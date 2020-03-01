using Newtonsoft.Json;
using RestSharp;
using System;
using WebapiWinserviceObjects;

namespace WebapiWinserviceClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://127.0.0.1:5006/api/ip2loc");
            var request = new RestRequest("");
            //request.AddJsonBody(JsonConvert.SerializeObject(ctr));
            IRestResponse resp = client.Get(request);
            dynamic respObject = JsonConvert.DeserializeObject(resp.Content);
            Console.WriteLine($"GET Myip: {respObject}");

            request = new RestRequest("");
            request.AddParameter("ip", "91.207.131.254");
            //request.AddJsonBody(JsonConvert.SerializeObject(ctr));
            resp = client.Get(request);
            respObject = JsonConvert.DeserializeObject(resp.Content);
            Console.WriteLine();
            Console.WriteLine($"GET IP: {respObject}");

            request = new RestRequest("");
            request.AddJsonBody(JsonConvert.SerializeObject(new Ip2LocationRequest
            {
                IpAddress = "91.207.131.254"
            }));
            resp = client.Post(request);
            respObject = JsonConvert.DeserializeObject(resp.Content);
            Console.WriteLine();
            Console.WriteLine($"POST Myip: {respObject}");

            request = new RestRequest("");
            request.AddJsonBody(JsonConvert.SerializeObject(new Ip2LocationRequest
            {
                IpAddress = "91.207.131.254"
            }));
            resp = client.Post(request);
            respObject = JsonConvert.DeserializeObject(resp.Content);
            Console.WriteLine();
            Console.WriteLine($"POST IP: {respObject}");
        }
    }
}
