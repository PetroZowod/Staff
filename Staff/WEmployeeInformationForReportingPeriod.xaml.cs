using Staff.Class;
using System.Collections.ObjectModel;
using System.Windows;

namespace Staff
{
    public partial class WEmployeeInformationForReportingPeriod : Window
    {
        Person person;
        ObservableCollection<Person> collection = new ObservableCollection<Person>();
        public WEmployeeInformationForReportingPeriod()
        {
            InitializeComponent();
            person = new Person();
            personGrid.DataContext = person;
            listBox.DataContext = collection;
        }

        private void Button_Click_Find(object sender, RoutedEventArgs e)
        {
            if (datePickerPeriodStart.SelectedDate.ToString() != null || datePickerPeriodEnd.SelectedDate != null)
            {
                FillData();
            }
            else
            {
                MessageBox.Show("Введите дату!");
            }
        }

        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void FillData()
        {
            collection.Clear();
            foreach(Person person in Person.Find(textBoxL_Name.Text, textBoxF_Name.Text, textBoxS_Name.Text, datePickerPeriodStart.SelectedDate.Value.ToString("yyyy-MM-dd"), datePickerPeriodEnd.SelectedDate.Value.ToString("yyyy-MM-dd")))
            {
                collection.Add(person);
            }
        }
    }
}
