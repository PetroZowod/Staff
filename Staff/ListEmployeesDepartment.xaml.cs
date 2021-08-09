using Staff.Class;
using System.Collections.ObjectModel;
using System.Windows;

namespace Staff
{
    public partial class ListEmployeesDepartment : Window
    {
        Otdel otdel;
        Person person;
        ObservableCollection<Person> collection = new ObservableCollection<Person>();
        public ListEmployeesDepartment()
        {
            InitializeComponent();
            otdel = new Otdel();
            comboBoxOtdel.Items.Add(otdel.Otdel_name);
            foreach (var otd in Otdel.GetAllOtdels())
            {
                comboBoxOtdel.Items.Add(otd.Otdel_name);
            }
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
            foreach (var pers in Person.ListLemployeesByDepartment(comboBoxOtdel.Text.ToString(), datePicker.SelectedDate.ToString()))
            {
                collection.Add(pers);
            }
        }
    }
}
