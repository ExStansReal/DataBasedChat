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
using System.Windows.Shapes;

namespace DataBasedChat
{
    /// <summary>
    /// Логика взаимодействия для Change_User.xaml
    /// </summary>
    public partial class Change_User : Window
    {
        private User User;
        public Change_User(User user)
        {
            InitializeComponent();
            User = user;
            log.Text = User.Login;
            pas.Text = User.Password;
            if(String.IsNullOrEmpty(User.Othcestvo) || User.Othcestvo == null)
               FIO.Text = $"ФИО: {User.Familiya} {User.Ima}";
            else
                FIO.Text = $"ФИО: {User.Familiya} {User.Ima} {User.Othcestvo}";
        }
        private void Back()
        {
            Messenger a = new Messenger(User);
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(log.Text) && !String.IsNullOrWhiteSpace(pas.Text))
            {
                if (pas.Text.Length >= 6)
                {
                    if (CheckSameLogin() == false)
                    {
                        
                            new UserTableAdapter().UpdateQuery(User.Familiya, User.Ima, User.Othcestvo, log.Text, pas.Text,User.Id);
                        User.Login = log.Text;
                        User.Password = pas.Text;
                            MessageBox.Show("Данные изменены!");
                        Back();


                    }
                    else
                        MessageBox.Show("Пользователь с таким логином уже есть");
                }
                else
                    MessageBox.Show("Пароль должен быть длиннее или равен 6 символам");

            }
            else
                MessageBox.Show("Вы заполнели не все поля");

        }
        private bool CheckSameLogin()
        {
            DataGrid dataGrid = new DataGrid();
            UserTableAdapter adapter = new UserTableAdapter();
            DataBasedChatDataSet.UserDataTable table = new DataBasedChatDataSet.UserDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;
                string login = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[4].ToString();
                int anotherId = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());
                if (login == log.Text && anotherId != User.Id)
                    return true;
            }

            return false;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Back();
        }
    }
}
