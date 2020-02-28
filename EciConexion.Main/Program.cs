using Common.Utils;
using Newtonsoft.Json.Linq;
using System;

namespace EciConexion.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string url = @"https://www.googleapis.com/customsearch/v1?key=AIzaSyB3Jj0w1YKPnfaqCfEPy5imwechYbtRCyA&cx=001733618061435153373:78pwzinfcxz&q=Fabio+Enrique+Quintero+DiazGranados";

            using (IRestClient<JObject> rc = new RestClient<JObject>(url))
            {
                var returnable = rc.GETRequestAsync().Result;
            }
        }
    }
}
