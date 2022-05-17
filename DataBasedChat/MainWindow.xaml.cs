using DataBasedChat.DataBasedChatDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataBasedChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registration a = new Registration();
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = new DataGrid();
            UserTableAdapter adapter = new UserTableAdapter();
            DataBasedChatDataSet.UserDataTable table = new DataBasedChatDataSet.UserDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            bool exists = false;
            User outUser = new User();
            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                User a = new User();
                dataGrid.SelectedIndex = i;
                string login = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[4].ToString();
                string password = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[5].ToString();
                if (log.Text == login && pas.Password == password)
                {
                    exists = true;

                    a.Id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());
                    a.Familiya = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                    a.Ima = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                    a.Othcestvo = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                    a.Login = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[4].ToString();
                    a.Password = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[5].ToString();

                    outUser = a;
                    break;
                }
            }
            if (exists == true)
            {
                Messenger a = new Messenger(outUser);
                a.Top = this.Top;
                a.Left = this.Left;
                this.Hide();
                a.Show();
            }
            else
                MessageBox.Show("Проверьте логин или пароль");


        }
    }
}
