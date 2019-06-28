using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevA4CalculatorEngine.Calculation_Engine.Controller
{
    class Calculation_Controller
    {
        Calculation_Engine.Model.Calculation_Model CalcModel;
        DataBase_Access.Database_Access_Layer DAL = new DataBase_Access.Database_Access_Layer();

        public static int length;
        Random rnd = new Random();

        public void InitializeUpdateMetrics()
        {
            try
            {
                GetListsOfNewMetrics();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void getRdm()
        {
            Random rnd = new Random();
            int result = rnd.Next(52);
            Calculation_Engine.Model.Calculation_Model.Cacc = result.ToString();
        }

        public List<string> HandleHumidity(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            //  double value = default;
            //
            //  if (CalculationType == 0) //Si 0 on calcule la moyenne
            //  {
            //      value = GetMoyenneFromList();
            //  }
            //
            //  DAL.PostCalculatedMetrics(
            //      //  (1 + rnd.Next(1000)).ToString(),
            //      "13",
            //      value.ToString(),
            //      DateTime.Now,
            //      DateTime.Now,
            //      CalculationType
            //      );
            //

            List<string> stats = new List<string>();
            stats.Add(CalculateAverageValue(metrics));
            stats.Add(CalculateMedianValue(metrics));

            return stats;
        }

        public List<string> HandleTemperature(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            List<string> stats = new List<string>();
            stats.Add(CalculateAverageValue(metrics));
            stats.Add(CalculateMedianValue(metrics));

            return stats;
        }

        public List<string> HandleLight(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            List<string> stats = new List<string>();
            stats.Add(CalculateAverageValue(metrics));
            stats.Add(CalculateMedianValue(metrics));

            return stats;
        }

        public List<string> HandlePressure(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            List<string> stats = new List<string>();
            stats.Add(CalculateAverageValue(metrics));
            stats.Add(CalculateMedianValue(metrics));

            return stats;
        }

        public List<string> HandleDecibel(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            List<string> stats = new List<string>();
            stats.Add(CalculateAverageValue(metrics));
            stats.Add(CalculateMedianValue(metrics));

            return stats;
        }

        public List<string> HandleCO2Level(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            List<string> stats = new List<string>();
            stats.Add(CalculateAverageValue(metrics));
            stats.Add(CalculateMedianValue(metrics));

            return stats;
        }


        public double GetMoyenneFromList()
        {

            List<double> list = new List<double>();
            list.Add(1.332);
            list.Add(2);
            list.Add(3.134);

            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("La moyenne de la liste:  {0}", list.Average());

            return Math.Round(list.Average(), 2);

        }

        public void GetListsOfNewMetrics()
        {
            DAL.GetLastIndex(); //get the last index to request the JSON
            DAL.GetLastDate(); //get last date for the

            // using (WebClient webClient = new System.Net.WebClient())
            // {
            //     WebClient n = new WebClient();
            //     json = n.DownloadString("https://jsonplaceholder.typicode.com/posts/1");
            //     string valueOriginal = Convert.ToString(json);
            //     Console.WriteLine(json);
            //
            // }
            //Calculation_Engine.Model.DeviceMetrics device_metrics = new Calculation_Engine.Model.DeviceMetrics(json);
            string json = @"[{""id"":2,""metric_value"":""50"",""metric_date"":""2019-06-26T16:20:11.522403"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":3,""metric_value"":""12"",""metric_date"":""2019-06-26T16:20:46.188723"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":5,""metric_value"":""14"",""metric_date"":""2019-06-26T16:20:46.188723"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":4,""metric_value"":""20"",""metric_date"":""2019-06-26T16:20:47.544581"",""device_type"":1,""device_macaddress"":""00-14-22-01-23-46""}]";

            var metricList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Calculation_Engine.Model.DeviceMetrics>>(json);
            var deviceMetricList = metricList.GroupBy(d => d.device_macaddress).Select(group => group.ToList())
                                             .ToList(); ;

            StringBuilder sCommand = new StringBuilder("INSERT INTO calculated_metric (Device_macaddress, Calculated_Metric_Value, DateStart, DateEnd, Metric_Calculation_Type_ID) VALUES ");
            List<string> Rows = new List<string>();


            foreach (var deviceMetric in deviceMetricList)
            {

                switch (deviceMetric.First().device_type)
                {
                    case 1: //Humidity
                        Rows.AddRange(HandleHumidity(deviceMetric));
                        break;
                    case 2://Temperature
                        Rows.AddRange(HandleTemperature(deviceMetric));
                        break;
                    case 3://Luminosity
                        Rows.AddRange(HandleLight(deviceMetric));
                        break;
                    case 4://Pressure
                        Rows.AddRange(HandlePressure(deviceMetric));
                        break;
                    case 5://sound
                        Rows.AddRange(HandleDecibel(deviceMetric));
                        break;
                    case 6://AirSensor
                        Rows.AddRange(HandleCO2Level(deviceMetric));
                        break;
                    default:
                        break;
                }

            }
            sCommand.Append(string.Join(",", Rows));
            sCommand.Append(";");

            Console.WriteLine(sCommand);
            Console.WriteLine(DataBase_Access.ModelDB.LastUpdateDate);
            Console.WriteLine(DataBase_Access.ModelDB.LastUpdateIndex);
            Console.WriteLine(metricList.Select(c => c.id).Max());

             DAL.AddMetricsToDB(sCommand);
             DAL.UpdateLastIndex(metricList.Select(c => c.id).Max()); //get the last index to request the JSON
             DAL.UpdateLastDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); //get last date for the
        }

        private string CalculateAverageValue(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            return string.Format("('{0}','{1}','{2}','{3}','{4}')", metrics.First().device_macaddress, metrics.Average(c => Double.Parse(c.metric_value)), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DataBase_Access.ModelDB.LastUpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), 0);
        }

        private string CalculateMedianValue(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            return string.Format("('{0}','{1}','{2}','{3}','{4}')", metrics.First().device_macaddress, GetMedian(metrics.Select(c => int.Parse(c.metric_value)).ToList()), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DataBase_Access.ModelDB.LastUpdateDate.ToString("yyyy-MM-dd HH:mm:ss"), 1);
        }

        //  public string HandleTemperatureStats(List<Model.DeviceMetrics> metrics)
        //  {
        //      List<Model.DeviceMetrics> MetricsMoyennesList = metrics
        //       .GroupBy(l => l.device_macaddress)
        //       .Select(cl => new Model.DeviceMetrics
        //       {
        //           device_macaddress = cl.First().device_macaddress,
        //           metric_value = cl.Average(c => c.metric_value),
        //           device_type = cl.First().device_type
        //       }).ToList();
        //      return string.Format("('{0}','{1}','{2}')", DeviceTypeID, MacAdress, DeviceName;
        //      //todo
        //  }

        public static decimal GetMedian(List<int> source)
        {
            // Create a copy of the input, and sort the copy
            int[] temp = source.ToArray();
            Array.Sort(temp);
            int count = temp.Length;
            if (count == 0)
            {
                throw new InvalidOperationException("Empty collection");
            }
            else if (count % 2 == 0)
            {
                // count is even, average two middle elements
                int a = temp[count / 2 - 1];
                int b = temp[count / 2];
                return (a + b) / 2m;
            }
            else
            {
                // count is odd, return the middle element
                return temp[count / 2];
            }
        } //get the median value of the list
    }
}

