using Staff.Class;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Threading;

namespace Staff
{
    public partial class WListEmployees : Window
    {
        Person person;
        ObservableCollection<Person> collection = new ObservableCollection<Person>();   //  Для того чтобы положить в listBox данные для обновления
        static SqlConnection sqlConnection;
        string userAdmin;

        public WListEmployees()
        {
            InitializeComponent();
            person = new Person();  //  Создаем новый обьект Person
            gridPerson.DataContext = person;  //  Делаем привязку grid
            listBox.DataContext = collection;   //  Привязываем коллекцию
        }
        public WListEmployees(string user)
        {
            userAdmin = user;
            InitializeComponent();
            person = new Person();  //  Создаем новый обьект Person
            gridPerson.DataContext = person;  //  Делаем привязку grid
            listBox.DataContext = collection;   //  Привязываем коллекцию
        }
        private void button_Add(object sender, RoutedEventArgs e)   //  Кнопка добать пользователя
        {
            WAddingNewEmployee wAddingNewEmployee = new WAddingNewEmployee(person);
            wAddingNewEmployee.Show();
            person = null;
        }
        private void button_Scan(object sender, RoutedEventArgs e)  //  Просмотр всех сотрудников
        {
            FillData();
        }
        private void button_Delete(object sender, RoutedEventArgs e)    //  Удалить сатрудника
        {
            if (listBox.SelectedIndex < 0)
                return;
            if (userAdmin == "Administrator")
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить сотрудника? Операцию отменить не возможно!", "Внимание!", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Person.DeleteAdministrator(((Person)listBox.SelectedItem).Id_User);
                    FillData();
                }
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить сотрудника?", "Внимание!", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Person.DeleteUser(((Person)listBox.SelectedItem).Id_User);
                    FillData();
                }
            }
        }
        private void button_Edit(object sender, RoutedEventArgs e)  //  Редактировать
        {
            if (listBox.SelectedIndex < 0)
                return;
            Person personUpdate = (Person)listBox.SelectedItem;
            WAddingNewEmployee wAddingNewEmployee = new WAddingNewEmployee(personUpdate);
            wAddingNewEmployee.Title = "Обновление сотрудника";
            wAddingNewEmployee.textBoxNameWindow.Text = "Обновление сотрудника";
            wAddingNewEmployee.buttonAdd.Content = "Обновить";
            wAddingNewEmployee.ShowDialog();
            FillData();
        }
        private void button_Find(object sender, RoutedEventArgs e)  //  Поиск
        {

        }
        public static void newConnection()  //  Подключение к БД
        {
            string connString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;   //  Строка подключения к БД
            sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();  //  Подключаемся к БД
        }
        private void Notification(bool status)  //  Уведомление
        {
            newConnection();
            string commandString = "SELECT COUNT(person.id_user) FROM person WHERE person.is_working = 1 AND end_work <= DATEADD(MONTH,1, GETDATE())";
            SqlCommand getAllComand = new SqlCommand(commandString, sqlConnection);
            var reader = getAllComand.ExecuteReader();
            int result = 0;
            while (reader.Read())
            {
                result = reader.GetInt32(0);
            }
            if (result > 0 && status == true)
            {
                var messageBox = MessageBox.Show($"Найдено {result} сотрудник(а/ов) у которых закончится контракт (договор) через 1 месяц. Показать?", "Уведомление", MessageBoxButton.YesNo);
                if (messageBox == MessageBoxResult.Yes)
                {
                    WEmployeesContractEnds wEmployeesContractEnds = new WEmployeesContractEnds();
                    wEmployeesContractEnds.Show();
                }
            }
            if (result > 0 && status == false)
            {
                var messageBox = MessageBox.Show($"Найдено {result} сотрудник(а/ов) у которых закончится контракт (договор) через 1 месяц. Показать?", "Уведомление", MessageBoxButton.YesNo);
                if (messageBox == MessageBoxResult.Yes)
                {
                    WEmployeesContractEnds wEmployeesContractEnds = new WEmployeesContractEnds();
                    wEmployeesContractEnds.Show();
                }
            }
            else if (result == 0 && status == false)
            {
                MessageBox.Show("Сотрудников у которых истечет контракт через 1 месяц не найдено");
            }
        }
        private void StackPanel_Loaded(object sender, RoutedEventArgs e)    //  Выводим время в labelDateTime
        {
            //  Подсчет времени
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (o, t) => { labelDateTime.Content = DateTime.Now.ToString(); };
            timer.Start();
        }
        private void FillData()
        {
            collection.Clear();
            foreach (Person pers in Person.GetAllPersons())
            {
                collection.Add(pers);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WChart wChart = new WChart();
            wChart.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WListContractBeforeDate wListContractBeforeDate = new WListContractBeforeDate();
            wListContractBeforeDate.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ListEmployeesDepartment listEmployeesDepartment = new ListEmployeesDepartment();
            listEmployeesDepartment.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillData();
            Notification(true);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            WEmployeeInformationForReportingPeriod wEmployeeInformationForReportingPeriod = new WEmployeeInformationForReportingPeriod();
            wEmployeeInformationForReportingPeriod.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Notification(false);
        }
    }
}