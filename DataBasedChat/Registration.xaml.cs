using DataBasedChat.DataBasedChatDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static DataBasedChat.DataBasedChatDataSet;

namespace DataBasedChat
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }
        private void Back()
        {
            MainWindow a = new MainWindow();
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (!String.IsNullOrWhiteSpace(fam.Text) && !String.IsNullOrWhiteSpace(ima.Text) && !String.IsNullOrWhiteSpace(log.Text) && !String.IsNullOrWhiteSpace(pas.Text))
            {
                if (pas.Text.Length >= 6)
                {
                    if (CheckSameLogin() == false)
                    {
                        if (CheckSameFIO() == false)
                        {
                            new UserTableAdapter().InsertQuery(fam.Text, ima.Text, otch.Text, log.Text, pas.Text);
                            MessageBox.Show("Вы зарегистрированы!");
                            Back();
                        }
                        else
                            MessageBox.Show("Пользователь с таким ФИО уже есть");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Back();
        }
        private bool CheckSameFIO()
        {
            DataGrid dataGrid = new DataGrid();
            UserTableAdapter adapter = new UserTableAdapter();
            DataBasedChatDataSet.UserDataTable table = new DataBasedChatDataSet.UserDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;
                string fami = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                string imaa = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                string otches = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                string fio = fami + imaa + otches;
                if (fio == (fam.Text + ima.Text + otch.Text))
                    return true;
            }

            return false;
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
                if (login == log.Text)
                    return true;
            }

            return false;
        }
    }
}
