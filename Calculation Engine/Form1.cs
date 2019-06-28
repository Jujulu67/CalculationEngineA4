using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetDevA4CalculatorEngine
{
    public partial class Form1 : Form
    {
        Calculation_Engine.Controller.Calculation_Controller CalcCtrl = new Calculation_Engine.Controller.Calculation_Controller();
        DataBase_Access.Database_Access_Layer DAL = new DataBase_Access.Database_Access_Layer();


        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CalcCtrl.getRdm();
            textBox1.Text = Calculation_Engine.Model.Calculation_Model.Cacc;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            label1.Text = DataBase_Access.ModelDB.Connection.ToString();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DAL.DDBCO();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            DAL.DDBDECO();

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            DAL.CheckNouvelleSemaine();
            textBox1.Text = DataBase_Access.ModelDB.Metric;
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            DAL.Ajout_Type(textBox2.Text);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Settings.Default.BDDChain);
        }

        private void BtnAddCalculatedMetrics_Click(object sender, EventArgs e)
        {
            string DeviceID, MetricValue;
            //int MetricID;
            DateTime DateStart, DateEnd;

            DeviceID = MetricValue = default;
            // MetricID = default;
            DateStart = DateEnd = default;


            if (String.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Gngn vraies valeurs svp");
                return; //Arrete tout si jamais c'est le cas
            }
            else
                MetricValue = textBox3.Text; //On est dans le cas ou c'est good


            //if (MetricID == default)
            //{
            //    MetricID = "";
            //}
            if (DeviceID == default)
            {
                //DeviceID = GetRandomMacAddress();
                var random = new Random();
                DeviceID = (1 + random.Next(100)).ToString();
            }
            else if (MetricValue == default)
            {
                var random = new Random();
                MetricValue = (1 + random.Next()).ToString();
            }
            else if (DateStart == default)
            {
                DateStart = DateTime.Now;
                string formatForMySql = DateStart.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (DateEnd == default)
            {
                DateEnd = DateTime.Now;
                string formatForMySql = DateEnd.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (DeviceID == default && MetricValue == default)
            {
            }
            DAL.PostCalculatedMetrics(DeviceID, MetricValue, DateStart, DateEnd, 0);
        }


        private void Button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DateTime.Now.ToString());
        }

        private void BtnGenerateDevice_Click(object sender, EventArgs e)
        {
            int DeviceTypeID;
            string MacAdress, DeviceName;
            var rnd = new Random();
            Stopwatch timer = new Stopwatch();
            timer.Start();

            //for (int i = 0; i < Convert.ToInt32(TxNumberDevices.Text); i++)
            //{
            //    DeviceTypeID = (1 + rnd.Next(10));
            //    MacAdress = GetRandomMacAddress();
            //    DeviceName = GenerateName(8);
            //    DAL.GenerateDevices(DeviceTypeID, MacAdress, DeviceName);
            //}

            string ConnectionString = Properties.Settings.Default.BDDChain;
            StringBuilder sCommand = new StringBuilder("INSERT INTO device (Device_Type_ID, MAC_Address, Device_Name) VALUES ");
            using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
            {
                List<string> Rows = new List<string>();
                for (int i = 0; i < Convert.ToInt32(TxNumberDevices.Text); i++)
                {
                    DeviceTypeID = (1 + rnd.Next(10));
                    MacAdress = GetRandomMacAddress(rnd);
                    DeviceName = GenerateName(8, rnd);
                    Rows.Add(string.Format("('{0}','{1}','{2}')", DeviceTypeID, MacAdress, DeviceName));

                }
                sCommand.Append(string.Join(",", Rows));
                sCommand.Append(";");
                Console.WriteLine(sCommand);

                mConnection.Open();
                using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                {
                    myCmd.CommandType = CommandType.Text;
                    myCmd.ExecuteNonQuery();
                }
            }

            timer.Stop();
            MessageBox.Show("time spent: " + timer.Elapsed);
        }

        public static string GetRandomMacAddress(Random r)
        {
            var buffer = new byte[6];
            r.NextBytes(buffer);
            var result = String.Concat(buffer.Select(x => string.Format("{0}:", x.ToString("X2"))).ToArray());
            return result.TrimEnd(':');
        }

        public static string GenerateName(int len, Random r)
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;


        }

        private void TxNumberDevices_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }


        private void BtnStart_onClick(object sender, EventArgs e)
        {
            if (BtnStartCalculation.Text == "Start")
            {
                BtnStartCalculation.Text = "Stop";
                TimerCalculation.Interval = (Convert.ToInt32(TxtCalculationTime.Text) * 1000 * 60); //On définit l'interval du timer par rapport à la txtbox
                TimerCalculation.Start(); //On démarre le timer

                //DOSOMETHING();

            }
            else
                BtnStartCalculation.Text = "Start";

            TimerCalculation.Stop(); //On démarre le timer

        }

        private void TimerCalculation_Tick(object sender, EventArgs e)
        {
            //DOSOMETHING();
            try
            {

            }
            catch (Exception ex)
            {

            }


        }

        private void Button9_Click(object sender, EventArgs e)
        {
            //CalcCtrl.GetMoyenneFromList();
            // CalcCtrl.CalculateHumidity(0);
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            MessageBox.Show(CalcCtrl.GetMoyenneFromList().ToString());
        }

        private void Button11_Click(object sender, EventArgs e)
        {

            string json = @"[{""id"":2,""metric_value"":""50"",""metric_date"":""2019-06-26T16:20:11.522403"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":3,""metric_value"":""12"",""metric_date"":""2019-06-26T16:20:46.188723"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":5,""metric_value"":""14"",""metric_date"":""2019-06-26T16:20:46.188723"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":4,""metric_value"":""20"",""metric_date"":""2019-06-26T16:20:47.544581"",""device_type"":1,""device_macaddress"":""00-14-22-01-23-46""}]";

            var metricList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Calculation_Engine.Model.DeviceMetrics>>(json);
            var deviceMetricList = metricList.GroupBy(d => d.device_macaddress).Select(group => group.ToList())
                                             .ToList(); ;

            StringBuilder sCommand = new StringBuilder("INSERT INTO calculated_metric (Device_macaddress, Calculated_Metric_Value, DateStart, DateEnd, Metric_Calculation_Type_ID) VALUES ");
            List<string> Rows = new List<string>();


            //get date
            //get last id

            foreach (var deviceMetric in deviceMetricList)
            {

                switch (deviceMetric.First().device_type)
                {
                    case 1:
                        Rows.AddRange(HandleTemperatureStats(deviceMetric));
                        break;
                    case 2:
                        Rows.AddRange(HandleTemperatureStats(deviceMetric));
                        break;
                    default:
                        break;
                }

            }
            sCommand.Append(string.Join(",", Rows));
            sCommand.Append(";");

            Console.WriteLine(sCommand);

        }

        private string HandleAverageValue(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            return string.Format("('{0}','{1}','{2}','{3}','{4}')", metrics.First().device_macaddress, metrics.Average(c => Double.Parse(c.metric_value)), DateTime.Now, DateTime.Now, 0);
        }

        private string HandleMedianValue(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            return string.Format("('{0}','{1}','{2}','{3}','{4}')", metrics.First().device_macaddress, GetMedian(metrics.Select(c => int.Parse(c.metric_value)).ToList()), DateTime.Now, DateTime.Now, 1);
        }

        private List<string> HandleTemperatureStats(List<Calculation_Engine.Model.DeviceMetrics> metrics)
        {
            List<string> stats = new List<string>();
            stats.Add(HandleAverageValue(metrics));
            stats.Add(HandleMedianValue(metrics));

            return stats;
            //todo
        }

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
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            CalcCtrl.InitializeUpdateMetrics();

        }

        private void Button13_Click(object sender, EventArgs e)
        {
            DAL.UpdateLastDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            Console.WriteLine(@"[{""id"":2,""metric_value"":""50"",""metric_date"":""2019-06-26T16:20:11.522403"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":3,""metric_value"":""12"",""metric_date"":""2019-06-26T16:20:46.188723"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":5,""metric_value"":""14"",""metric_date"":""2019-06-26T16:20:46.188723"",""device_type"":2,""device_macaddress"":""00-14-22-01-23-45""}
,{""id"":4,""metric_value"":""20"",""metric_date"":""2019-06-26T16:20:47.544581"",""device_type"":1,""device_macaddress"":""00-14-22-01-23-46""}]");
        }
    }
}
