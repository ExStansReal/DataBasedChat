using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Messenger.xaml
    /// </summary>
    public partial class Messenger : Window
    {
        private User User;
        public Messenger(User inToUser)
        {
            InitializeComponent();
            User = inToUser;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FindFriends a = new FindFriends(User);
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow a = new MainWindow();
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Zayavki a = new Zayavki(User);
            a.Top = this.Top;
            a.Left = this.Left;
            this.Hide();
            a.Show();
        }
    }
}
