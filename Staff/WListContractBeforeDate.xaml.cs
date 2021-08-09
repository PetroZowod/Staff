using Staff.Class;
using System.Collections.ObjectModel;
using System.Windows;
namespace Staff
{
    public partial class WListContractBeforeDate : Window
    {
        Person person;
        ObservableCollection<Person> collection = new ObservableCollection<Person>();   //  Для того чтобы положить в listBox данные для обновления
        public WListContractBeforeDate()
        {
            InitializeComponent();
            person = new Person();
            grid.DataContext = person;
            listbox.DataContext = collection;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate != null)
                FillData();
            else
                MessageBox.Show("Введите дату");
        }
        private void FillData()
        {
            collection.Clear();
            foreach (var pers in Person.ListContractBeforeDate(datePicker.SelectedDate.ToString()))
            {
                collection.Add(pers);
            }
        }
    }
}
