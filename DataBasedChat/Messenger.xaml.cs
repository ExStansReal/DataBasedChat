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
        DispatcherTimer TimerC;
        int oldCountC = 0;
        int NewCountC = 0;
        public Messenger(User inToUser)
        {
            InitializeComponent();
            User = inToUser;

            TimerF = new DispatcherTimer();
            TimerF.Interval = TimeSpan.FromSeconds(1);
            TimerF.Tick += GetFriends;
            TimerF.Start();

            TimerC = new DispatcherTimer();
            TimerC.Interval = TimeSpan.FromSeconds(1);
            TimerC.Tick += LoadChatAlways;
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
                    if (Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString()) == User.Id)
                    {
                        Chats.Items.Add(GetFioUser(Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString())));
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
            TimerC.Stop();
            TimerF.Stop();
            FindFriends a = new FindFriends(User);
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TimerC.Stop();
            TimerF.Stop();
            MainWindow a = new MainWindow();
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TimerC.Stop();
            TimerF.Stop();
            Zayavki a = new Zayavki(User);
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Chats_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            TimerC.Stop();
            try
            {
                if(Chats.SelectedItem != null)
                WhoIsTalking.Text = $"Чат с: {Chats.SelectedItem.ToString()}";
            }
            catch
            {

            }

            if (Chats.SelectedItem != null)
            {
                sendMes.IsEnabled = true;
                LoadChat();
                TimerC.Start();
            }
        }

        private void LoadChatAlways(object sender, EventArgs e)
        {
           
            ID_Chat = FindIDChat();
            DataGrid dataGrid = new DataGrid();
            MessedgesTableAdapter adapter = new MessedgesTableAdapter();
            DataBasedChatDataSet.MessedgesDataTable table = new DataBasedChatDataSet.MessedgesDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;
            NewCountC = dataGrid.Items.Count;
            if (NewCountC != oldCountC)
            {
                Chat.Items.Clear();
                oldCountC = NewCountC;
                for (int i = 0; i < dataGrid.Items.Count - 1; i++)
                {
                    dataGrid.SelectedIndex = i;

                    int Chat_Id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[4].ToString());

                    if (Chat_Id == ID_Chat)
                    {
                        string fio = "";
                        User user = FindUser(Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString()));
                        if (user.Othcestvo == "" || String.IsNullOrWhiteSpace(user.Othcestvo))
                            fio = user.Familiya + " " + user.Ima[0] + ". ";
                        else
                            fio = user.Familiya + " " + user.Ima[0] + ". " + user.Othcestvo[0] + ". ";

                        string Messedge = $"[{(dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString()}] {fio}: {(dataGrid.SelectedItem as DataRowView).Row.ItemArray[3].ToString()}";

                        Chat.Items.Add(Messedge);

                        sendMes.IsEnabled = true;
                    }
                }
            }
        }

        private void LoadChat()
        {
            Chat.Items.Clear();
            ID_Chat = FindIDChat();
            DataGrid dataGrid = new DataGrid();
            MessedgesTableAdapter adapter = new MessedgesTableAdapter();
            DataBasedChatDataSet.MessedgesDataTable table = new DataBasedChatDataSet.MessedgesDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;

            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;

                int Chat_Id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[4].ToString());

                if(Chat_Id == ID_Chat)
                {
                    string fio = "";
                    User user = FindUser(Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString()));
                    if (user.Othcestvo == "" || String.IsNullOrWhiteSpace(user.Othcestvo))
                         fio = user.Familiya + " " + user.Ima[0] + ". ";
                    else
                     fio = user.Familiya + " " + user.Ima[0] + ". " + user.Othcestvo[0] + ". ";

                    string Messedge = $"[{(dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString()}] {fio}: {(dataGrid.SelectedItem as DataRowView).Row.ItemArray[3].ToString()}";

                    Chat.Items.Add(Messedge);
                    sendMes.IsEnabled = true;
                }
            }
        }
        //DateTime.Now.ToString("HH:mm:ss")
        private User FindUser(int ID_User)
        {
            User user = new User();
            DataGrid dataGrid = new DataGrid();
            UserTableAdapter adapter = new UserTableAdapter();
            DataBasedChatDataSet.UserDataTable table = new DataBasedChatDataSet.UserDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;

            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;
                int user_ID = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());
                if(user_ID == ID_User)
                {
                    user.Id = user_ID;
                    user.Familiya = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString();
                    user.Ima = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString();
                    user.Othcestvo = (dataGrid.SelectedItem as DataRowView).Row.ItemArray[3].ToString();
                }


            }


            return user;
        }
        int ID_Chat = 0;
        private int FindIDChat()
        {

            DataGrid dataGrid = new DataGrid();
            ChatTableAdapter adapter = new ChatTableAdapter();
            DataBasedChatDataSet.ChatDataTable table = new DataBasedChatDataSet.ChatDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;

            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;
                int firstID = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString());
                int secondID = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString());
                if(firstID == User.Id && secondID == Friends[Chats.SelectedIndex].Id)
                {
                    return Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());
                }
                if (firstID == Friends[Chats.SelectedIndex].Id && secondID == User.Id)
                {
                    return Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());
                }
            }
            return 0;
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Chats.SelectedItem != null)
            {
                sendMes.IsEnabled = false;
                TimerC.Stop();
                Chat.Items.Clear();
                new FriendsTableAdapter().DeleteQuery(FindIdFriends());
                WhoIsTalking.Text = "";
                Chats.SelectedItem = null;
            }
            else
                MessageBox.Show("Сначала выберете друга, которого хотите удалить");
        }

        private int FindIdFriends()
        {
            DataGrid dataGrid = new DataGrid();
            FriendsTableAdapter adapter = new FriendsTableAdapter();
            DataBasedChatDataSet.FriendsDataTable table = new DataBasedChatDataSet.FriendsDataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table;

            for (int i = 0; i < dataGrid.Items.Count - 1; i++)
            {
                dataGrid.SelectedIndex = i;
                int First_Id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[1].ToString());
                int Second_Id = Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[2].ToString());
                if (First_Id == User.Id && Second_Id == Friends[Chats.SelectedIndex].Id)
                {
                    return Convert.ToInt32((dataGrid.SelectedItem as DataRowView).Row.ItemArray[0].ToString());
                }
            }

            return 0;
        }

        private void sendMes_Click(object sender, RoutedEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(mes.Text))
            {
                new MessedgesTableAdapter().InsertQuery(Convert.ToString(DateTime.Now.ToString("HH:mm:ss")), User.Id, mes.Text, ID_Chat);
                if(Chat.Items.Count != 0)
                Chat.ScrollIntoView(Chat.Items[Chat.Items.Count - 1]);
                mes.Text = null;
            }
            

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            TimerC.Stop();
            TimerF.Stop();
            Change_User a = new Change_User(User);
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }
    }
}
