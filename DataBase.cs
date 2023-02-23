using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace muslim_helper
{
    internal class DataBase
    {
        public static MySqlConnection connMaster = new MySqlConnection();


        static string server = "localhost";
        static string port = "3306";
        static string Uid = "root";
        static string password = "root";
        static string dataBase = "userhistory";

        public static MySqlConnection dataSource()
        {
            connMaster = new MySqlConnection($"server={server} port = {port} Uid = {Uid} password = {password} database = {dataBase}");
            return connMaster;

        }

        public void openConnection()
        {
            if(connMaster.State == System.Data.ConnectionState.Closed)
                connMaster.Open();
        }
        public void closeConnection()
        {
            if (connMaster.State == ConnectionState.Open)
                connMaster.Close();
        }

        public MySqlConnection GetConnection()
        {
            return connMaster;
        }
    }
}
