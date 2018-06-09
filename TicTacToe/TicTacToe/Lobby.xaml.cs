using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TicTacToe
{
    /// <summary>
    /// Логика взаимодействия для Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        TcpClient client;
        ClientIO clientIO;
        Thread lisen;
        private Game game;

        public Lobby()
        {
            InitializeComponent();
            TcpClient cl = new TcpClient();
            client = cl;
        }

        public Lobby(string nick)
        {
            InitializeComponent();
            TcpClient cl = new TcpClient();
            client = cl;
            NickBox.Text = nick;
            ConnButt_Click(this, EventArgs.Empty);
        }
        private void PlayButt_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in PlayerList.Items)
            {
                ListBoxItem lbi = (ListBoxItem)PlayerList.ItemContainerGenerator.ContainerFromItem(item);

                if (lbi.IsSelected && (string)lbi.Content != NickBox.Text)
                {
                    clientIO.SendData("play,"+ NickBox.Text + "," + (string)lbi.Content);
                    Game game = new Game(client, true, NickBox.Text, (string)lbi.Content);
                    Dispatcher.BeginInvoke((Action)(() => game.Show()));
                    lisen.Abort();
                    Close();
                }
                    
            }
        }

        private void ConnButt_Click(object sender, EventArgs e)
        {
            string nick = NickBox.Text;

            Regex regex = new Regex(@"\w");
            Match match = regex.Match(nick);

            

            if (match.Success && nick.Length<11)
            {
                try
                {
                    client.Connect("127.0.0.1", 5555);
                }
                catch
                {
                    NickLable.Content = "Не удалось подключиться к серверу";
                    NickLable.Foreground = Brushes.Red;
                    return;
                }

                NickLable.Content = "Вы подключены";
                NickLable.Foreground = Brushes.Green;
                NickBox.IsEnabled = false;
                ConnButt.IsEnabled = false;

                PlayerList.Items.Add(nick);

                clientIO = new ClientIO(client);

                lisen = new Thread(() => ListendForCommand());
                lisen.Start();
                clientIO.SendData("conn,"+ nick );
            }
            else if (nick.Length > 10)
            {
                NickLable.Content = "Максимальная длина имени 10 символов";
                NickLable.Foreground = Brushes.Red;
            }
            else
            {
                NickLable.Content = "Некорректное имя";
                NickLable.Foreground = Brushes.Red;
            }
        }

        private void ListendForCommand()
        {
           while (client.Connected)
            {
                string data = clientIO.ReadData();
                string[] command = data.Split(',');
                if (command[0] == "plist.add")
                {
                    Dispatcher.BeginInvoke((Action)(() => PlayerList.Items.Add(command[1])));
                }
                if (command[0] == "plist.sing")
                {
                    for (int i = 1; i < command.Length - 1; i++)
                    {
                        Dispatcher.Invoke((Action)(() => PlayerList.Items.Add(command[i])));
                    }
                }
                if (command[0] == "plist.del")
                {
                    int i = 0;
                    foreach (var item in PlayerList.Items)
                    {
                        if (PlayerList.Items[i].Equals(command[1]))
                        {
                            Dispatcher.Invoke(() => PlayerList.Items.RemoveAt(i));
                            break;
                        }
                        i++;
                    }
                }    // play,true,nick
                if ( command[0] == "play")
                {
                    string Ynick = null;
                    Dispatcher.Invoke((Action)(() => Ynick = NickBox.Text ));
                    bool YP1 = Convert.ToBoolean(command[1]);
                    clientIO.SendData("disc," + Ynick);
                    Dispatcher.Invoke((Action)(() =>  game = new Game(client, YP1, Ynick, command[2])));
                    Dispatcher.Invoke((Action)(() =>  game.Show()));
                    Dispatcher.Invoke((Action)(() =>  Close()));
                    return;
                }
            }
        }

        private void ExitButt_Click(object sender, RoutedEventArgs e)
        {
            clientIO.SendData("disc," + NickBox.Text);
            Close();

        }
    }
}
