using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.DataVisualization.Charting;

namespace Staff
{
    public partial class WChart : Window
    {
        static SqlConnection sqlConnection;
        public WChart()
        {
            InitializeComponent();
        }
        private void datePicker_CalendarOpened(object sender, RoutedEventArgs e)
        {
            var datepicker = sender as DatePicker;
            if (datepicker != null)
            {
                var popup = datepicker.Template.FindName(
                    "PART_Popup", datepicker) as Popup;
                if (popup != null && popup.Child is Calendar)
                {
                    ((Calendar)popup.Child).DisplayMode = CalendarMode.Decade;
                }
            }
        }
        public void newConnection()  //  Подключение к БД
        {
            string connString = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;   //  Строка подключения к БД
            sqlConnection = new SqlConnection(connString);
            sqlConnection.Open();  //  Подключаемся к БД
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker.SelectedDate != null)
            {
                var sqlcommandString = $"SELECT MONTH(fired_date), COUNT(id_user) FROM person WHERE YEAR(fired_date) = {datePicker.SelectedDate.Value.Year} GROUP BY MONTH(fired_date) ORDER BY MONTH(fired_date) DESC";
                newConnection();
                SqlCommand selectCommand = new SqlCommand(sqlcommandString, sqlConnection);
                var reader = selectCommand.ExecuteReader();
                int j = 0;
                string[] axisXData = new string[12] { "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь", "октябрь", "ноябрь", "декабрь" };
                int[] axisYData = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object mount = reader.GetInt32(0);
                        object count = reader.GetInt32(1);
                        // axisXData[j] = (int)mount;
                        axisYData[(int)mount - 1] = (int)count;
                        j++;
                    }
                }
                chart.Series.Clear();
                chart.ChartAreas.Clear();
                // Все графики находятся в пределах области построения ChartArea, создадим ее
                chart.ChartAreas.Add(new ChartArea("ChartPerson"));
                // Добавим линию, и назначим ее в ранее созданную область "Default"
                chart.Series.Add(new Series("Series"));
                chart.Series["Series"].ChartArea = "ChartPerson";
                chart.Series["Series"].IsValueShownAsLabel = true; //  Отображаем значения
                chart.Series["Series"].ChartType = SeriesChartType.Column; //  Отображаем колонки
                chart.ChartAreas["ChartPerson"].AxisX.ScaleView.Zoom(1, 9);  //  Масштаб графика
                chart.Series["Series"].LabelFormat = "# чел.";   // формат отображения
                chart.Series["Series"].Points.DataBindXY(axisXData, axisYData); //  Добовляем данные
            }
            else
                MessageBox.Show("Введите дату");
        }
    }
}