using System.Windows;
using System.Windows.Input;

namespace Staff
{
    public partial class WAuthorization : Window
    {
        Auntification auntification;
        public WAuthorization()
        {
            InitializeComponent();
            auntification = new Auntification();
            comboBoxLogin.Items.Add(auntification.Users);
            comboBoxLogin.Items.Clear();
            foreach(var aut in Auntification.GetAllAuntifications())
            {
                comboBoxLogin.Items.Add(aut.Users);
            }
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)    //  При нажатии отмена закрываем окно
        {
            this.Close();
        }

        private void Button_Click_Login(object sender, RoutedEventArgs e)   //  Авторизация пользователя
        {
            Auntification auntification = Auntification.SelectName(comboBoxLogin.Text,passwordBox.Password);      //  Передаем имя и пароль для авторизации
            if (auntification == null)  //  Если авторизация не удалась выводим сообщение
            {
                MessageBox.Show("Неверный пароль");
            }
            else
            {
                if (comboBoxLogin.Text == "Administrator")
                {
                    WListEmployees wAdminListEmployees = new WListEmployees(comboBoxLogin.Text);
                    wAdminListEmployees.buttonDelete.Content = "Удалить сотрудника";
                    wAdminListEmployees.Show();
                    this.Close();
                    return;
                }
                WListEmployees wListEmployees = new WListEmployees();
                wListEmployees.Show();
                this.Close();
            }
        }
    }
}
