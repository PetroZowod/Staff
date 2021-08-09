using Staff.Class;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace Staff
{
    public partial class WEmployeesContractEnds : Window
    {
        Person person;
        ObservableCollection<Person> collection = new ObservableCollection<Person>();   //  Для того чтобы положить в listBox данные для обновления
        static SqlConnection sqlConnection;

        public WEmployeesContractEnds()
        {
            InitializeComponent();
            person = new Person();  //  Создаем новый обьект Person
            gridPerson.DataContext = person;  //  Делаем привязку grid
            listBox.DataContext = collection;   //  Привязываем коллекцию
            FillData();
        }
        public static void newConnection()  //  Подключение к БД
        {
            string connString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;   //  Строка подключения к БД
            sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();  //  Подключаемся к БД
        }

        private void FillData()
        {
            collection.Clear();
            foreach (var pers in Person.NotificationGettAllPerson())
            {
                collection.Add(pers);
            }
        }
        private void button_Edit(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex < 0)
                return;
            Person personUpdate = (Person)listBox.SelectedItem;
            WAddingNewEmployee wAddingNewEmployee = new WAddingNewEmployee(personUpdate);
            wAddingNewEmployee.Title = "Обновление сотрудника";
            wAddingNewEmployee.textBoxNameWindow.Text = "Обновление сотрудника";
            wAddingNewEmployee.buttonAdd.Content = "Обновить";
            wAddingNewEmployee.ShowDialog();
        }

        private void button_Delete(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedIndex < 0)
                return;
            Person.DeleteUser(((Person)listBox.SelectedItem).Id_User);
            FillData();
        }

    }

}