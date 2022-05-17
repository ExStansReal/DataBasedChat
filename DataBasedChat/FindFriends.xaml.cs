using DataBasedChat.DataBasedChatDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DataBasedChat
{
    /// <summary>
    /// Логика взаимодействия для FindFriends.xaml
    /// </summary>
    public partial class FindFriends : Window
    {
        private User User;
        DispatcherTimer Timer;
        int oldCount = 0;
        int NewCount = 0;
        public FindFriends(User user)
        {
            InitializeComponent();
            User = user;

            GetFirstPeople();


            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += GetPeople;
            Timer.Start();


        }
        private void GetFirstPeople()
        {
            DataGrid dataGrid = new DataGrid();
            UserTableAdapter adapter = new UserTableAdapter();
            DataBasedChatDataSet.UserDataTable table = new DataBasedChatDataSet.UserDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;

            oldCount = dataGrid.Items.Count;
            listPeople.Items.Clear();
            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;
                string fami = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                string imaa = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                string otches = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                string fio = fami + " " + imaa + " " + otches;
                listPeople.Items.Add(fio);
            }
        }
        
        private void GetPeople(object sender, EventArgs e)
        {
            DataGrid dataGrid = new DataGrid();
            UserTableAdapter adapter = new UserTableAdapter();
            DataBasedChatDataSet.UserDataTable table = new DataBasedChatDataSet.UserDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            NewCount = dataGrid.Items.Count;
            if (NewCount != oldCount)
            {
                oldCount = NewCount;
                listPeople.Items.Clear();
                for (int i = 0; i < dataGrid.Items.Count - 1; i++)
                {
                    dataGrid.SelectedIndex = i;
                    string fami = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                    string imaa = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                    string otches = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                    string fio = fami + " " + imaa  + " "+ otches;
                    listPeople.Items.Add(fio);
                }
            }
            
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
            Back();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (listPeople.SelectedItem != null)
            {
                if (CheckExistsZayavlenie() == false)
                {
                    if (CheckExistsFriend() == false)
                    {
                        new ZayavkiTableAdapter().InsertQuery(User.Id, GetIdUser(), Convert.ToString(DateTime.Today));
                        MessageBox.Show("Заявление отправлено");
                    }
                    else
                        MessageBox.Show("Вы уже в друзьях у этого человека");
                }
                else
                    MessageBox.Show("Вы уже отправили заявление этому человеку");
            }
            else
                MessageBox.Show("Сначала выберет пользователя");
        }
        private int GetIdUser()
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
                string fio = fami + " " + imaa + " " + otches;
                if (fio == listPeople.SelectedItem.ToString())
                {
                    int id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());

                    return id;
                }
            }

            return 0;
        }

        private bool CheckExistsZayavlenie()
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
                string fio = fami + " " + imaa + " " + otches;
                if (fio == listPeople.SelectedItem.ToString())
                {
                    int id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());

                    DataGrid dataGrid1 = new DataGrid();
                    ZayavkiTableAdapter adapter1 = new ZayavkiTableAdapter();
                    DataBasedChatDataSet.ZayavkiDataTable table1 = new DataBasedChatDataSet.ZayavkiDataTable();
                    adapter1.Fill(table1);
                    dataGrid1.ItemsSource = table1;
                    for (int j = 0; i < dataGrid1.Items.Count - 1; i++)
                    {
                        dataGrid1.SelectedIndex = j;

                        int id1 = Convert.ToInt32((dataGrid1.SelectedItem as DataRowView).Row.ItemArray[2].ToString());
                        if (id1 == id)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool CheckExistsFriend()
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
                    string fio = fami + " " + imaa + " " + otches;
                if(fio == listPeople.SelectedItem.ToString())
                {
                    int id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());

                    DataGrid dataGrid1 = new DataGrid();
                    FriendsTableAdapter adapter1 = new FriendsTableAdapter();
                    DataBasedChatDataSet.FriendsDataTable table1 = new DataBasedChatDataSet.FriendsDataTable();
                    adapter1.Fill(table1);
                    dataGrid1.ItemsSource = table1;
                    for (int j = 0; i < dataGrid1.Items.Count - 1; i++)
                    {
                        dataGrid1.SelectedIndex = j;

                        int id1 = Convert.ToInt32((dataGrid1.SelectedItem as DataRowView).Row.ItemArray[2].ToString());
                        if(id1 == id)
                        {
                            return true;
                        }
                    }
                }
             }
            
            return false;
        }
    }
}
