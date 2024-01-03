using Org.BouncyCastle.Security;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace muslim_helper
{
    internal class DataBase
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["muslimhelperDB"].ConnectionString;
        static SqlConnection sqlConnection = new SqlConnection(connectionString);

        public void openConnection()
        {
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }
        public void closeConnection()
        {
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
        public long[] GetMembersChatIDWithActivatedNotifications()
        {
            openConnection();
            SqlDataReader reader = null;
            List<long> members = new List<long>();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT chatid FROM users_table WHERE namaz_notification = 1", sqlConnection);
                reader = sqlCommand.ExecuteReader();
                if (reader == null)
                    Console.WriteLine("ридер пуст");
                while (reader.Read())
                {
                    members.Add(reader.GetInt32(0));
                    Console.WriteLine(reader.GetInt32(0));
                }
                return members.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                closeConnection();
            }
        }
        private long[] GetMembersChatIDWithActivatedTaskTracking()
        {
            openConnection();
            SqlDataReader reader = null;
            List<long> members = new List<long>();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT chatid FROM users_table WHERE task_tracking = 1", sqlConnection);
                reader = sqlCommand.ExecuteReader();
                if (reader == null)
                    Console.WriteLine("ридер пуст");
                while (reader.Read())
                {
                    members.Add(reader.GetInt32(0));
                    Console.WriteLine(reader.GetInt32(0));
                }
                return members.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                closeConnection();
            }
        }
        private void TaskCreating()
        {
            long[] taskOwners = GetMembersChatIDWithActivatedTaskTracking();
            foreach (long memberID in taskOwners)
            {

            }
        }
    }
}

