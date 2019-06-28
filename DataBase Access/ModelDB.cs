using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetDevA4CalculatorEngine.DataBase_Access
{
    class ModelDB
    {
        public static int LastUpdateIndex;
        public static DateTime LastUpdateDate;
        public static string Metric;
        public static bool Connection;

        #region Calculated Metrics
        public static int MetricID;
        public static string DeviceID; //Will be the macAdress
        public static string MetricType;
        public static string MetricValue;
        public static DateTime DateStart;
        public static DateTime DateEnd;
        #endregion


        public void ResetMetrics() //Permet de reset la valeur de chaque metric avant le push BDD pour éviter des erreurs
        {
           // MetricID = default;
            DeviceID = default;
            MetricValue = default;
            DateStart = default;
            DateEnd = default;
        }
    }
}
