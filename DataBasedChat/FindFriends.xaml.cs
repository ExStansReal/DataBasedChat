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
    /// Логика взаимодействия для FindFriends.xaml
    /// </summary>
    public partial class FindFriends : Window
    {
        private User User;
        public FindFriends(User user)
        {
            InitializeComponent();
            User = user;
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
    }
}
