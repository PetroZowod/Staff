using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Staff
{
    public class Auntification
    {
        static SqlConnection connection;
        public int Id { get; set; }
        public string Users { get; set; }
        public string Password { get; set; }

        public static void newConnection()  //  Подключение к БД
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public static IEnumerable<Auntification> GetAllAuntifications() //  Выводим список всех пользователей
        {
            newConnection();
            var commandSQL = "SELECT * FROM auth_user";
            SqlCommand sqlCommand = new SqlCommand(commandSQL, connection);
            var reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var users = reader.GetString(1);
                    var password = reader.GetString(2);
                    var auntef = new Auntification { Id = id, Users = users, Password = password };
                    yield return auntef;
                }
            }
            connection.Close();
        }
        public static Auntification SelectName(string name, string password)    //  Авторизация
        {
            foreach (var auntif in GetAllAuntifications())
                if (auntif.Users.TrimEnd() == name.TrimEnd() && auntif.Password.TrimEnd() == password.TrimEnd())
                    return auntif;
            return null;
        }

    }
}
