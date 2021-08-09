using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Staff.Class
{
    public class Post :Otdel
    {

        static SqlConnection sqlConnection;
        public Post()
        {
            Persons = new List<Person>();
        }
        public int Id_post { get; set; }
        public string Post_name { get; set; }
        public int Id_otdel { get; set; }
        public Otdel otdel { get; set; }
        public List<Person> Persons { get; set; }
        public static void newCollection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;   //  Строка подключения к БД из app.config
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();  //  Подключаемся к БД
        }
        public static IEnumerable<Post> GetAllPost()
        {
            newCollection();
            var commandSQL = "SELECT * FROM post";
            SqlCommand sqlCommand = new SqlCommand(commandSQL, sqlConnection);
            var reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var id_post = reader.GetInt32(0);
                    var post_name = reader.GetString(1);
                    var id_otdel = reader.GetInt32(2);
                    var post = new Post { Id_post = id_post, Post_name = post_name, Id_otdel = id_otdel };
                    yield return post;
                }
            }
            sqlConnection.Close();
        }
        public override string ToString()
        {
            return $"{Id_post} {Id_otdel} {Post_name}";
        }
    }
}
