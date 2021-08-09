using Staff.Class;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Staff
{
    public class Otdel
    {
        static SqlConnection sqlConnection;
        public Otdel()
        {
            Posts_ = new List<Post>();
        }
        public int Id_otdel { get; set; }
        public string Otdel_name { get; set; }
        public List<Post> Posts_ { get; set; }
        public virtual ICollection<Post> Posts { get; set; }

        public static void newConnection()  //  Подключение к БД
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
        }

        public static IEnumerable<Otdel> GetAllOtdels()
        {
            newConnection();
            var commandSQL = "SELECT * FROM otdel; ";
            SqlCommand sqlCommand = new SqlCommand(commandSQL, sqlConnection);
            var reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id_otdel = reader.GetInt32(0);
                    var otdel_name = reader.GetString(1);
                    var otdel = new Otdel { Id_otdel = id_otdel, Otdel_name = otdel_name };
                    yield return otdel;
                }
            }
            sqlConnection.Close();
        }
        public static Otdel Find(string otdel_name) //  Для поиска id отдела
        {
            foreach (var otdel in GetAllOtdels())
                if (otdel.Otdel_name == otdel_name) //  Ищем по name
                    return otdel;  //  Возращаем имя
            return null;    //  Если ничего не нашли возвращаем null
        }
    }
}
