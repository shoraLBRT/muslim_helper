using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace muslim_helper
{
    internal class DataBase
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["usersDB"].ConnectionString;
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

        public void AddUserIntoDB(string username, string firstname, long chatid)
        {
            openConnection();
            SqlCommand checkUniqueness = new SqlCommand($@"SELECT COUNT(chatid) FROM users_db WHERE chatid = '{chatid}'", sqlConnection);

            if ((Int32)checkUniqueness.ExecuteScalar() == 1)
                Console.WriteLine("такой пользователь уже имеется");
            else
            {
                SqlCommand insertUserInfo = new SqlCommand(@$"INSERT INTO [users_db] (username, firstname, chatid) VALUES (N'{username}', N'{firstname}', N'{chatid}')", sqlConnection);
                Console.WriteLine(insertUserInfo.ExecuteNonQuery().ToString());
                Console.WriteLine("данные о пользователе загружены в базу данных");
            }
            closeConnection();
        }

        public void SetNamazNotificationForUser(long chatid, bool state)
        {
            openConnection();
            SqlCommand setNamazNotification = new($@"UPDATE users_db SET namaz_notification = '{state}' WHERE chatid = '{chatid}'", sqlConnection);
            setNamazNotification.ExecuteNonQuery();
            closeConnection();
        }

        public long[] GetMembersChatIDWithActivatedNotifications()
        {
            openConnection();
            SqlDataReader reader = null;
            List<long> members = new List<long>();
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT chatid FROM users_db WHERE namaz_notification = 1", sqlConnection);
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
    }
}

