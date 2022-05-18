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
using System.Windows.Threading;

namespace DataBasedChat
{
    /// <summary>
    /// Логика взаимодействия для Messenger.xaml
    /// </summary>
    public partial class Messenger : Window
    {
        private User User;

        DispatcherTimer TimerF;
        int oldCountF = 0;
        int NewCountF = 0;
        public Messenger(User inToUser)
        {
            InitializeComponent();
            User = inToUser;

            TimerF = new DispatcherTimer();
            TimerF.Interval = TimeSpan.FromSeconds(1);
            TimerF.Tick += GetFriends;
            TimerF.Start();
        }

        List<User> Friends = new List<User>();

        private void GetFriends(object sender, EventArgs e)
        {
            DataGrid dataGrid = new DataGrid();
            FriendsTableAdapter adapter = new FriendsTableAdapter();
            DataBasedChatDataSet.FriendsDataTable table = new DataBasedChatDataSet.FriendsDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            NewCountF = dataGrid.Items.Count;
            if (NewCountF != oldCountF)
            {
                oldCountF = NewCountF;

                Friends.Clear();
                Chats.Items.Clear();

                for(int i = 0; i < dataGrid.Items.Count -1; i++)
                {
                    dataGrid.SelectedIndex = i;
                    int idUser = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString());
                    if(idUser == User.Id)
                    { 
                        Chats.Items.Add(GetFioUser(Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString())));
                    }
                }

            }
        }

        private string GetFioUser(int idUser)
        {
            DataGrid dataGrid = new DataGrid();
            UserTableAdapter adapter = new UserTableAdapter();
            DataBasedChatDataSet.UserDataTable table = new DataBasedChatDataSet.UserDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;

                int id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());

                if (id == idUser)
                {
                    string fami = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                    string imaa = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                    string otches = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                    User friend = new User();
                    friend.Familiya = fami;
                    friend.Ima = imaa;
                    friend.Othcestvo = otches;
                    friend.Id = id;
                    Friends.Add(friend);
                    if (otches == "" || String.IsNullOrWhiteSpace(otches))
                        return fami + " " + imaa[0] + ".";
                    else
                        return fami + " " + imaa[0] + ". " + otches[0] + ".";
                }
                
            }
            return "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TimerF.Stop();
            FindFriends a = new FindFriends(User);
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TimerF.Stop();
            MainWindow a = new MainWindow();
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TimerF.Stop();
            Zayavki a = new Zayavki(User);
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Chats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WhoIsTalking.Text = $"Чат с: {Chats.SelectedItem.ToString()}";
        }
    }
}
