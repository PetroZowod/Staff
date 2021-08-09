
using Staff.Class;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace Staff
{
    public partial class WAddingNewEmployee : Window
    {
        Otdel otdel;
        Person person;
        ObservableCollection<Person> collection = new ObservableCollection<Person>();
        public WAddingNewEmployee(Person person)
        {
            InitializeComponent();
            otdel = new Otdel();
            comboBox_Otdel.Items.Add(otdel.Otdel_name);
            foreach (var otd in Otdel.GetAllOtdels())
            {
                comboBox_Otdel.Items.Add(otd.Otdel_name);
            }
            if (person == null)
                person = new Person();
            else
            {
                this.person = person;
                personGrid.DataContext = person;
            }
            // Чистим comboBox с договором и котнрактом и заполняем договор
            comboBoxContractDogovor.SelectedIndex = -1;
            comboBoxContractDogovor.ItemsSource = new string[] { "1 год", "2 года", "3 года", "4 года", "5 лет" };
        }
        //  Button //
        private void Button_Click_Add(object sender, RoutedEventArgs e) //  Кнопка Добавить
        {
            if (textBoxNameWindow.Text == "Добавление нового сотрудника")
            {
                if (textBoxL_Name.Text.Length == 0 || textBoxF_Name.Text.Length == 0 || comboBox_Otdel.SelectedItem == null || comboBox_Post.SelectedItem == null || datePickerStartWork.SelectedDate == null || comboBoxContractDogovor.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите добавить сотрудника?", "Внимание!", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    person?.Insert(comboBox_Post.Text, comboBoxContractDogovor.Text.ToString());
                    this.Close();
                }
            }
            else
            // Кнопка обновить
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите изменить информацию?", "Внимание!", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Person.InsertOldPerson1((person).Id_User);
                    Person persons = new Person();
                    persons.UpdatePerson(person);
                    persons.Update(comboBox_Post.Text, comboBoxContractDogovor.Text.ToString());
                    this.Close();
                }
            }
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)  //  Кнопка отмена
        {
            this.Close();
        }

        private void comboBox_Otdel_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)    //  При выборе отдела заполняем combobox должности
        {
            if (comboBox_Otdel.SelectedItem != null)
            {
                Otdel otdelSelect = Otdel.Find(comboBox_Otdel.SelectedItem.ToString()); //  Ищем по имени выбранный отдел
                comboBox_Post.Items.Clear();    //  Чистим combobox поста
                foreach (var post in Post.GetAllPost()) //  Проходимся по таблице post и ищем совпадения
                {
                    if (otdelSelect.Id_otdel == post.Id_otdel)   //  Если совпадение найдено то выводим список
                        comboBox_Post.Items.Add(post.Post_name);
                }
            }
        }
        private void FillData()
        {
            collection.Clear();
            foreach (var p in Person.GetAllPersons())
            {
                collection.Add(p);
            }
        }
        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton peressed = (RadioButton)sender;
            if (peressed.Content.ToString() == "Контракт" && peressed.IsChecked == true)
            {
                comboBoxContractDogovor.SelectedIndex = -1;
                comboBoxContractDogovor.ItemsSource = new string[] { "1 год", "2 года", "3 года", "4 года", "5 лет" };
            }
            else if (peressed.Content.ToString() == "Договор" && peressed.IsChecked == true)
            {
                comboBoxContractDogovor.SelectedIndex = -1;
                comboBoxContractDogovor.ItemsSource = new string[] { "1 месяц", "2 месяца", "3 месяца", "4 месяца", "5 месяцев",
                "6 месяцев", "7 месяцев", "8 месяцев", "9 месяцев","10 месяцев", "11 месяцев", "12 месяцев" };
            }
        }
    }
}