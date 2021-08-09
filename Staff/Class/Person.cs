using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Staff.Class
{
    public class Person : Post
    {
        static SqlConnection sqlConnection;

        public int Id_User { get; set; }            //  ID
        public string L_Name { get; set; }          //  Фамилия
        public string F_Name { get; set; }          //  Имя
        public string S_Name { get; set; }          //  Отчество
        public bool Is_Working { get; set; }        //  Работает?
        public int Post_Id { get; set; }            //  ID Должности (post)
        public DateTime? Start_Work { get; set; }   //  Начало работы
        public DateTime? End_Work { get; set; }     //  Конец контракта
        public DateTime? Fired_Date { get; set; }   //  Дата увольнения
        public Post Posts { get; set; }

        public static void newConnection()  //  Подключение к БД
        {
            string connString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;   //  Строка подключения к БД
            sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();  //  Подключаемся к БД
        }
        public static IEnumerable<Person> GetAllPersons()   //  Показать ВСЁ
        {
            newConnection();
            var commandString = "SELECT person.id_user, person.l_name, person.f_name, person.s_name, post.post_name, person.start_work, person.end_work FROM  person JOIN post ON person.post_id = post.id_post AND  is_working = 1";
            SqlCommand getAllComand = new SqlCommand(commandString, sqlConnection);
            SqlDataReader reader = getAllComand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id_User = reader.GetInt32(0);
                    string l_name = reader.GetString(1);
                    string f_name = reader.GetString(2);
                    string s_name = reader.GetString(3);
                    string postName = reader.GetString(4);
                    DateTime start_Work = reader.GetDateTime(5);
                    DateTime end_Work = new DateTime();
                    if (!reader.IsDBNull(6))    //  Если поле null то установливаем формат даты  31.12.9999 7:00:00
                        end_Work = reader.GetDateTime(6);
                    else
                        end_Work = new DateTime(9999, 12, 31, 7, 00, 00);
                    Person person = new Person { Id_User = id_User, L_Name = l_name, F_Name = f_name, S_Name = s_name, Post_name = postName, Start_Work = start_Work, End_Work = end_Work };
                    yield return person;
                }
            }
            sqlConnection.Close();
        }
        public static IEnumerable<Person> NotificationGettAllPerson()   // Для отображения списка уведомления
        {
            newConnection();
            string commandString = "SELECT person.id_user, person.l_name, person.f_name, person.s_name, post.post_name, person.start_work, person.end_work FROM person JOIN post ON post.id_post = person.post_id AND person.is_working = 1 AND end_work <= DATEADD(MONTH,1, GETDATE())";
            SqlCommand getAllComand = new SqlCommand(commandString, sqlConnection);
            SqlDataReader reader = getAllComand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id_User = reader.GetInt32(0);
                    string l_name = reader.GetString(1);
                    string f_name = reader.GetString(2);
                    string s_name = reader.GetString(3);
                    string postName = reader.GetString(4);
                    DateTime start_Work = reader.GetDateTime(5);
                    DateTime end_Work = new DateTime();
                    if (!reader.IsDBNull(6))    //  Если поле null то установливаем формат даты  31.12.9999 7:00:00
                        end_Work = reader.GetDateTime(6);
                    else
                        end_Work = new DateTime(9999, 12, 31, 7, 00, 00);
                    Person person = new Person { Id_User = id_User, L_Name = l_name, F_Name = f_name, S_Name = s_name, Post_name = postName, Start_Work = start_Work, End_Work = end_Work };
                    yield return person;
                }
            }
            sqlConnection.Close();
        }
        public void Insert(string Post_name, string dogovor) //  Вставка
        {
            string date = ChoosingDeadline(dogovor);
            newConnection();
            string commandString = $"INSERT INTO person (l_name, f_name, s_name, post_id, start_work, end_work) VALUES (@l_name, @f_name, @s_name, " +
                $"(SELECT id_post FROM post WHERE post_name = N'{Post_name}'), @start_work, DATEADD({date} @start_work))";

            SqlCommand insertCommand = new SqlCommand(commandString, sqlConnection);
            insertCommand.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("l_name", L_Name),
                new SqlParameter("f_name", F_Name),
                new SqlParameter("s_name", S_Name),
                new SqlParameter("start_work",Start_Work)
        });
            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public void InsertOldPerson() //  Вставка в другую таблицу при изменении текущего пользователя (Для отчетного периода)
        {
            newConnection();
            string commandString = $"INSERT INTO old_person (tab_name, l_name, f_name, s_name, post_id, start_work, end_work) VALUES (@id_User, @l_name, @f_name, @s_name, @post_id, @start_work, @end_work)";
            SqlCommand insertCommand = new SqlCommand(commandString, sqlConnection);
            insertCommand.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("id_User", Id_User),
                new SqlParameter("l_name", L_Name),
                new SqlParameter("f_name", F_Name),
                new SqlParameter("s_name", S_Name),
                new SqlParameter("post_id", Post_Id),
                new SqlParameter("start_work",Start_Work),
                new SqlParameter("end_work",End_Work)
        });
            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public static void InsertOldPerson1(int id) //  Вставка
        {
            newConnection();
            string commandString = $"INSERT INTO old_person (tab_name, l_name, f_name, s_name, is_working, post_id, start_work, end_work, fired_date) SELECT * FROM person WHERE id_User = @id_User";
            SqlCommand insertCommand = new SqlCommand(commandString, sqlConnection);
            insertCommand.Parameters.AddWithValue("id_User", id);
            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public static void DeleteAdministrator(int id)   //  Удалить пользователя (Для администратора)
        {
            newConnection();
            string commandString = "DELETE FROM person WHERE id_user = @id";
            SqlCommand deleteCommand = new SqlCommand(commandString, sqlConnection);
            deleteCommand.Parameters.AddWithValue("id", id);
            deleteCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public static void DeleteUser(int id)   //  Удалить(уволить) сотрудника (Для пользовтеля)   Дата увольнения текущая
        {
            newConnection();
            var commandString = $"UPDATE person SET is_working = 0, Fired_Date = GETDATE() WHERE id_user = @id";
            SqlCommand DeleteUpdateCommand = new SqlCommand(commandString, sqlConnection);
            DeleteUpdateCommand.Parameters.AddWithValue("id", id);
            DeleteUpdateCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public void Update(string Post_name, string dogovor)    //  Обновление записи
        {
            string date = ChoosingDeadline(dogovor);
            newConnection();
            string commandSring = $"UPDATE person SET l_name = @l_name, f_name = @f_name, s_name = @s_name, post_id = (SELECT id_post FROM post WHERE post_name = N'{Post_name}'), " +
                $"start_work = @start_work, end_work = DATEADD({date} @start_work) WHERE Id_User = @Id_User";
            SqlCommand updateCommand = new SqlCommand(commandSring, sqlConnection);
            updateCommand.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("id_User",Id_User),
                new SqlParameter("l_name", L_Name),
                new SqlParameter("f_name", F_Name),
                new SqlParameter("s_name", S_Name),
                new SqlParameter("start_work",Start_Work)
            });
            updateCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public void UpdatePerson(Person personNew)  //  Обновление обьекта всеми не пустыми полями в personNew
        {
            if (personNew.Id_User > 0) Id_User = personNew.Id_User;
            if (personNew.L_Name?.Length > 0) L_Name = personNew.L_Name;
            if (personNew.F_Name?.Length > 0) F_Name = personNew.F_Name;
            if (personNew.S_Name?.Length > 0) S_Name = personNew.S_Name;
            if (personNew.Post_name?.Length > 0) Post_name = personNew.Post_name;
            if (personNew.Start_Work.Value.Date > DateTime.MinValue) Start_Work = personNew.Start_Work.Value.Date;

        }
        public static IEnumerable<Person> ListContractBeforeDate(string date)    //  Поиск сотрудников по окончанию договора
        {
            newConnection();
            string commandString = $"SELECT person.id_user, person.l_name, person.f_name, person.s_name, post.post_name, person.start_work, person.end_work FROM  person JOIN post ON person.post_id = post.id_post AND  is_working = 1 AND person.end_work <= '{date}'";
            SqlCommand getAllComand = new SqlCommand(commandString, sqlConnection);
            SqlDataReader reader = getAllComand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id_User = reader.GetInt32(0);
                    string l_name = reader.GetString(1);
                    string f_name = reader.GetString(2);
                    string s_name = reader.GetString(3);
                    string postName = reader.GetString(4);
                    DateTime start_Work = reader.GetDateTime(5);
                    DateTime end_Work = new DateTime();
                    if (!reader.IsDBNull(6))    //  Если поле null то установливаем формат даты  31.12.9999 7:00:00
                        end_Work = reader.GetDateTime(6);
                    else
                        end_Work = new DateTime(9999, 12, 31, 7, 00, 00);
                    Person person = new Person { Id_User = id_User, L_Name = l_name, F_Name = f_name, S_Name = s_name, Post_name = postName, Start_Work = start_Work, End_Work = end_Work };
                    yield return person;
                }
            }
            sqlConnection.Close();
        }
        public static IEnumerable<Person> ListLemployeesByDepartment(string otdel, string date)    //  Поиск сотрудников по окончанию договора
        {
            newConnection();
            var commandString = $"SELECT person.id_user, person.l_name, person.f_name, person.s_name, post.post_name, person.start_work, person.end_work FROM otdel JOIN post ON post.id_otdel = otdel.id_otdel AND otdel.otdel_name LIKE N'{otdel}%' JOIN person ON person.post_id = post.id_post AND person.start_work >= '{date}'";
            SqlCommand getAllComand = new SqlCommand(commandString, sqlConnection);
            var reader = getAllComand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id_User = reader.GetInt32(0);
                    string l_name = reader.GetString(1);
                    string f_name = reader.GetString(2);
                    string s_name = reader.GetString(3);
                    string postName = reader.GetString(4);
                    DateTime start_Work = reader.GetDateTime(5);
                    DateTime end_Work = new DateTime();
                    if (!reader.IsDBNull(6))    //  Если поле null то установливаем формат даты  31.12.9999 7:00:00
                        end_Work = reader.GetDateTime(6);
                    else
                        end_Work = new DateTime(9999, 12, 31, 7, 00, 00);
                    Person person = new Person { Id_User = id_User, L_Name = l_name, F_Name = f_name, S_Name = s_name, Post_name = postName, Start_Work = start_Work, End_Work = end_Work };
                    yield return person;
                }
            }
            sqlConnection.Close();
        }

        public static IEnumerable<Person> Find(string lname, string fname, string sname, string dateStart, string dateEnd)
        {
            string commandString = "SELECT old_person.tab_name, old_person.l_name, old_person.f_name, old_person.s_name, post.post_name, old_person.start_work, old_person.end_work FROM old_person " +
                    $"JOIN post ON old_person.post_id = post.id_post AND (old_person.start_work BETWEEN '{dateStart}' AND '{dateEnd}') AND (old_person.end_work BETWEEN '{dateStart}' AND '{dateEnd}')";
            if (lname != "" && fname == "" && sname == "" && dateStart != "" && dateEnd != "")
            {
                commandString = $"SELECT old_person.tab_name, old_person.l_name, old_person.f_name, old_person.s_name, post.post_name, old_person.start_work, old_person.end_work FROM old_person " +
                    $"JOIN post ON old_person.post_id = post.id_post AND old_person.l_name LIKE N'%{lname}%' AND (old_person.start_work BETWEEN '{dateStart}' AND '{dateEnd}') AND (old_person.end_work BETWEEN '{dateStart}' AND '{dateEnd}')";
            }
            else if (lname != "" && fname != "" && sname == "" && dateStart != "" && dateEnd != "")
            {
                commandString = $"SELECT old_person.tab_name, old_person.l_name, old_person.f_name, old_person.s_name, post.post_name, old_person.start_work, old_person.end_work FROM old_person " +
                    $"JOIN post ON old_person.post_id = post.id_post AND old_person.l_name LIKE N'%{lname}%' AND old_person.f_name LIKE N'%{fname}%' AND (old_person.start_work BETWEEN '{dateStart}' AND '{dateEnd}') AND (old_person.end_work BETWEEN '{dateStart}' AND '{dateEnd}')";
            }
            else if (lname != "" && fname != "" && sname != "" && dateStart != "" && dateEnd != "")
            {
                commandString = $"SELECT old_person.tab_name, old_person.l_name, old_person.f_name, old_person.s_name, post.post_name, old_person.start_work, old_person.end_work FROM old_person " +
                    $"JOIN post ON old_person.post_id = post.id_post AND old_person.l_name LIKE N'%{lname}%' AND old_person.f_name LIKE N'%{fname}%' AND old_person.s_name LIKE N'%{sname}%' AND (old_person.start_work BETWEEN '{dateStart}' AND '{dateEnd}') AND (old_person.end_work BETWEEN '{dateStart}' AND '{dateEnd}')";
            }
            newConnection();
            SqlCommand getAllComand = new SqlCommand(commandString, sqlConnection);
            var reader = getAllComand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int id_User = reader.GetInt32(0);
                    string l_name = reader.GetString(1);
                    string f_name = reader.GetString(2);
                    string s_name = reader.GetString(3);
                    string postName = reader.GetString(4);
                    DateTime start_Work = reader.GetDateTime(5);
                    DateTime end_Work = new DateTime();
                    if (!reader.IsDBNull(6))    //  Если поле null то установливаем формат даты  31.12.9999 7:00:00
                        end_Work = reader.GetDateTime(6);
                    else
                        end_Work = new DateTime(9999, 12, 31, 7, 00, 00);
                    Person person = new Person { Id_User = id_User, L_Name = l_name, F_Name = f_name, S_Name = s_name, Post_name = postName, Start_Work = start_Work, End_Work = end_Work };
                    yield return person;
                }
            }
            sqlConnection.Close();
        }

        private string ChoosingDeadline(string dateDogovorContract) //  Для добавления окончания договора
        {
            string date = null;
            switch (dateDogovorContract)
            {
                case "1 год":
                    date = "YEAR, 1,";
                    break;
                case "2 года":
                    date = "YEAR, 2,";
                    break;
                case "3 года":
                    date = "YEAR, 3,";
                    break;
                case "4 года":
                    date = "YEAR, 4,";
                    break;
                case "5 лет":
                    date = "YEAR, 5,";
                    break;
                case "1 месяц":
                    date = "MONTH, 1,";
                    break;
                case "2 месяца":
                    date = "MONTH, 2,";
                    break;
                case "3 месяца":
                    date = "MONTH, 3,";
                    break;
                case "4 месяца":
                    date = "MONTH, 4,";
                    break;
                case "5 месяцев":
                    date = "MONTH, 5,";
                    break;
                case "6 месяцев":
                    date = "MONTH, 6,";
                    break;
                case "7 месяцев":
                    date = "MONTH, 7,";
                    break;
                case "8 месяцев":
                    date = "MONTH, 8,";
                    break;
                case "9 месяцев":
                    date = "MONTH, 9,";
                    break;
                case "10 месяцев":
                    date = "MONTH, 10,";
                    break;
                case "11 месяцев":
                    date = "MONTH, 11,";
                    break;
                case "12 месяцев":
                    date = "MONTH, 12,";
                    break;
                default:
                    date = "DAY, 0,";
                    break;
            }
            return date;
        }
        public override string ToString()
        {
            return $"{Id_User} {L_Name} {F_Name} {S_Name} {Post_name} {Start_Work.Value.ToString("d")} {End_Work.Value.ToString("d")}";
        }
    }
}