using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProjetDevA4CalculatorEngine.DataBase_Access
{
    class Database_Access_Layer
    {
        MySqlConnection connection;
        private string BDDChain = Properties.Settings.Default.BDDChain;
        //  private string q;
        public void CheckNouvelleSemaine() //Utilisé pour connaitre l'ID en avance du prochain devis
        {
            try
            {
                connection = new MySqlConnection(BDDChain);

                connection.Open();
                string q = "SELECT * FROM calculatedmetrics WHERE ID = 1";
                MySqlCommand cmd = new MySqlCommand(q, connection);
                ModelDB.Metric = (string)cmd.ExecuteScalar().ToString();
                connection.Close();
            }
            catch (Exception ex)
            {
            }

        }


        public void Ajout_Type(string InputType) //Publie les metrics calculée dans la BDD
        {
            try
            {
                connection = new MySqlConnection(BDDChain);

                connection.Open();
                string q = "INSERT INTO device_type(Device_Type_Name) VALUES (\"" + InputType + "\")";
                MySqlCommand cmd = new MySqlCommand(q, connection);
                cmd.ExecuteNonQuery();
                //ModelDB.Metric = (string)cmd.ExecuteScalar().ToString();
                connection.Close();
            }
            catch (Exception ex)
            {
            }

        }


        public void PostCalculatedMetrics(string DeviceID, string MetricValue, DateTime DateStart, DateTime DateEnd, int MetricTypeID) //Ajouter un type de device
        {
            try
            {
                connection = new MySqlConnection(BDDChain);
                connection.Open();
                string q = "INSERT INTO calculated_metric(" +
                    "Device_ID,Calculated_Metric_Value,DateStart,DateEnd,Metric_Calculation_Type_ID) " +
                    "VALUES (" +
                    //"\"" + MetricID + "\"," +
                    "\"" + DeviceID + "\"," +
                    "\"" + MetricValue + "\"," +
                    "\"" + DateStart + "\"," +
                    "\"" + DateEnd + "\"," +
                    "\"" + MetricTypeID + "\"" +
                    ")";
                Console.WriteLine(q);
                MySqlCommand cmd = new MySqlCommand(q, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void GenerateDevices(int DeviceTypeID, string MacAdress, string DeviceName) //Ajouter un type de device
        {
            try
            {
                connection = new MySqlConnection(BDDChain);
                connection.Open();
                string q = "INSERT INTO device(" +
                       "Device_Type_ID,MAC_Address,Device_Name) " +
                       "VALUES (" +
                       //"\"" + MetricID + "\"," +
                       "\"" + DeviceTypeID + "\"," +
                       "\"" + MacAdress + "\"," +
                       "\"" + DeviceName + "\"" +
                       ")";
                // string q = "INSERT INTO device(Device_Type_ID,MAC_Address,Device_Name) VALUES (\"1\",\"6F:14:5B:F7:80:CF\",\"Xazhecysy\")";

                Console.WriteLine(q);
                MySqlCommand cmd = new MySqlCommand(q, connection);
                cmd.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void GetLastDate()
        {
            try
            {
                connection = new MySqlConnection(BDDChain);

                connection.Open();
                string q = "SELECT Last_Date FROM last_rawmetrics_update";
                MySqlCommand cmd = new MySqlCommand(q, connection);
                ModelDB.LastUpdateDate = (DateTime)cmd.ExecuteScalar();
                connection.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void GetLastIndex()
        {
            try
            {
                connection = new MySqlConnection(BDDChain);

                connection.Open();
                string q = "SELECT Last_Index FROM last_rawmetrics_update";
                MySqlCommand cmd = new MySqlCommand(q, connection);
                ModelDB.LastUpdateIndex = (int)cmd.ExecuteScalar();
                connection.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void UpdateLastIndex(int index)
        {
            try
            {
                connection = new MySqlConnection(BDDChain);

                connection.Open();
                string q = "UPDATE `last_rawmetrics_update` SET `Last_Index`=  " + index + "";
                MySqlCommand cmd = new MySqlCommand(q, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void UpdateLastDate(string date)
        {
            try
            {
                connection = new MySqlConnection(BDDChain);

                connection.Open();
                string q = "UPDATE `last_rawmetrics_update` SET `Last_Date`=  \"" + date + "\"";
                MySqlCommand cmd = new MySqlCommand(q, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public void AddMetricsToDB(StringBuilder sCommand)
        {
            try
            {
                using (MySqlConnection mConnection = new MySqlConnection(BDDChain))
                {

                    mConnection.Open();
                    using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                    {
                        myCmd.CommandType = CommandType.Text;
                        myCmd.ExecuteNonQuery();
                        connection.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        public void DDBCO()
        {
            try
            {
                connection = new MySqlConnection(BDDChain);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();

                    ModelDB.Connection = true;
                }
                else
                    ModelDB.Connection = false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        } //Test connexion basique à la BDD

        public void DDBDECO()
        {
            try
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    ModelDB.Connection = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        } //Deconnexion basique à la BDD


    }

}
