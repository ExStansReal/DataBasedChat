﻿using DataBasedChat.DataBasedChatDataSetTableAdapters;
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
using System.Windows.Threading;

namespace DataBasedChat
{
    /// <summary>
    /// Логика взаимодействия для Zayavki.xaml
    /// </summary>
    public partial class Zayavki : Window
    {
        private User User;

        DispatcherTimer Timer;
        int oldCount = 0;
        int NewCount = 0;
        public Zayavki(User user)
        {
            InitializeComponent();
            User = user;

            GetZayavki();


            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += GetZayavkiAlways;
            Timer.Start();
        }

        private void GetZayavkiAlways(object sender, EventArgs e)
        {
            
            DataGrid dataGrid = new DataGrid();
            ZayavkiTableAdapter adapter = new ZayavkiTableAdapter();
            DataBasedChatDataSet.ZayavkiDataTable table = new DataBasedChatDataSet.ZayavkiDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            NewCount = dataGrid.Items.Count;
            if (NewCount != oldCount)
            {
                oldCount = NewCount;

                zayavki.Items.Clear();

                for(int i = 0; i < dataGrid.Items.Count - 1; i++)
                {
                    dataGrid.SelectedIndex = i;
                    int idUser = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString());
                    int otheridUser = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString());
                    if (User.Id == idUser)
                    {
                        DataGrid dataGrid1 = new DataGrid();
                        UserTableAdapter adapter1 = new UserTableAdapter();
                        DataBasedChatDataSet.UserDataTable table1 = new DataBasedChatDataSet.UserDataTable();
                        adapter1.Fill(table1);
                        dataGrid1.ItemsSource = table1;
                        for(int j = 0; j < dataGrid1.Items.Count - 1; j++)
                        {
                            dataGrid1.SelectedIndex = j;

                            int againOtherIdUser = Convert.ToInt32((dataGrid1.SelectedItem as DataRowView).Row.ItemArray[0].ToString());

                            if (otheridUser == againOtherIdUser)
                            {
                                string fami = (dataGrid1.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                                string imaa = (dataGrid1.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                                string otches = (dataGrid1.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                                string fio = fami + " " + imaa + " " + otches;

                                zayavki.Items.Add(fio);
                            }
                        }

                    }
                }
                Added = false;
            }
        }

        private void GetZayavki()
        {
            DataGrid dataGrid = new DataGrid();
            ZayavkiTableAdapter adapter = new ZayavkiTableAdapter();
            DataBasedChatDataSet.ZayavkiDataTable table = new DataBasedChatDataSet.ZayavkiDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            NewCount = dataGrid.Items.Count;

                zayavki.Items.Clear();

                for (int i = 0; i < dataGrid.Items.Count - 1; i++)
                {
                    dataGrid.SelectedIndex = i;
                    int idUser = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString());
                    int otheridUser = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString());
                    if (User.Id == idUser)
                    {
                        DataGrid dataGrid1 = new DataGrid();
                        UserTableAdapter adapter1 = new UserTableAdapter();
                        DataBasedChatDataSet.UserDataTable table1 = new DataBasedChatDataSet.UserDataTable();
                        adapter1.Fill(table1);
                        dataGrid1.ItemsSource = table1;
                        for (int j = 0; j < dataGrid1.Items.Count - 1; j++)
                        {
                            dataGrid1.SelectedIndex = j;

                            int againOtherIdUser = Convert.ToInt32((dataGrid1.SelectedItem as DataRowView).Row.ItemArray[0].ToString());

                            if (otheridUser == againOtherIdUser)
                            {
                                string fami = (dataGrid1.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                                string imaa = (dataGrid1.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                                string otches = (dataGrid1.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                                string fio = fami + " " + imaa + " " + otches;

                                zayavki.Items.Add(fio);
                            }
                        }

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
            Timer.Stop();
            Back();
        }
        bool Added = false;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (zayavki.SelectedItem != null && Added == false)
            {
                Added = true;
                int AnotherUser = GetIdUser();
                new FriendsTableAdapter().InsertQuery(User.Id, AnotherUser);
                if (CheckForExistsChat(AnotherUser) == false)
                {
                    new ChatTableAdapter().InsertQuery(User.Id, AnotherUser);
                }
                new ZayavkiTableAdapter().DeleteQuery(getIdZayavleniya());
                MessageBox.Show("Друг добавлен");
                oldCount++;
            }
            else
                MessageBox.Show("Сначала выберете заявление");
        }
        private bool CheckForExistsChat(int Another_ID)
        {
            DataGrid dataGrid = new DataGrid();
            ChatTableAdapter adapter = new ChatTableAdapter();
            DataBasedChatDataSet.ChatDataTable table = new DataBasedChatDataSet.ChatDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;

                int First_Id = (Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString()));
                int Second_Id = (Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString()));

                if(First_Id == User.Id && Second_Id == Another_ID)
                {
                    return true;
                }

                if (First_Id == Another_ID && Second_Id == User.Id)
                {
                    return true;
                }


            }
            return false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (zayavki.SelectedItem != null)
            {
                new ZayavkiTableAdapter().DeleteQuery(getIdZayavleniya());
            }
            else
                MessageBox.Show("Сначала выберете заявление");
        }

        private int getIdZayavleniya()
        {
            int idUser = GetIdUser();
            DataGrid dataGrid = new DataGrid();
            ZayavkiTableAdapter adapter = new ZayavkiTableAdapter();
            DataBasedChatDataSet.ZayavkiDataTable table = new DataBasedChatDataSet.ZayavkiDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;

                int firstUser = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString());
                int MyIdUser = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString());
                if (firstUser == idUser && MyIdUser == User.Id)
                {
                    return Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());
                }
            }

                return 0;
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
                if (fio == zayavki.SelectedItem.ToString())
                {
                    int id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());

                    return id;
                }
            }

            return 0;
        }
    }
}
