using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevA4CalculatorEngine.Calculation_Engine.Model
{
    class DeviceMetrics
    {
        public int id { get; set; } //IdRawMetric
        public string metric_value { get; set; }
        public DateTime metric_date { get; set; }
        public int device_type { get; set; }
        public string device_macaddress { get; set; }

      // public DeviceMetrics(string json)
      // {
      //     JObject jObject = JObject.Parse(json);
      //     JToken jMetric = jObject["user"];
      //     metric_value = (string)jMetric["name"];
      //     metric_date = (DateTime)jMetric["teamname"];
      //     device_type = (int)jMetric["email"];
      //     device_macaddress = (string)jMetric["players"];
      // }

    }


}
