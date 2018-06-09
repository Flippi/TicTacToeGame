using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Game : Window
    {
        private MarkType[] mResults;

        bool YouPlayer1;
        bool YouTurn;
        bool mGameEnded;
        int GameCount;
        string YouName = "Flippes";
        string Pl2Name = "Alex EGr";
        int p1Score = 0;
        int p2Score = 0;
        TcpClient client;
        ClientIO clientIO;
        Thread lisen;

        public Game( TcpClient cl,bool YPlayer1, string YName, string OpponentName)
        {
            InitializeComponent();
            client = cl;
            //client.Connect("127.0.0.1", 5555);
            clientIO = new ClientIO(client);
            lisen = new Thread(() => ListendForCommand());
            lisen.Start();
            YouName = YName;
            Pl2Name = OpponentName;
            State.Content = "";
            GameCount = 0;
            YouPlayer1 = YPlayer1;
            if (YPlayer1)
                YouTurn = true;
            else
                YouTurn = false;
            NewGame();
        }
        public Game()
        {
            TcpClient cl = new TcpClient();
            InitializeComponent();
            client = cl;
            client.Connect("127.0.0.1", 5555);
            clientIO = new ClientIO(client);
            lisen = new Thread(() => ListendForCommand());
            lisen.Start();
            State.Content = "";
            GameCount = 0;
            YouPlayer1 = true;
            YouTurn = true;
            NewGame();
        }

        private void NewGame()
        {
            mResults = new MarkType[9];

            for (var i = 0; i < mResults.Length; i++)
                mResults[i] = MarkType.Free;

            GameCount++;

            ScoreLable.Content = YouName + "  " + p1Score + " : "  + p2Score + "  " + Pl2Name;

            if (YouTurn)
            {
                State.Content = "Ваш ход";
                State.Foreground = Brushes.Green;
            } 
            else
            {
                State.Content = "Ход противника";
                State.Foreground = Brushes.Red;
            }
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            mGameEnded = false;
        }

        private void ListendForCommand()
        {
            while (client.Connected)
            {
                string data = clientIO.ReadData();
                string[] command = data.Split(',');
                if (command[0] != "")
                {
                    if ( command[0] == "mv")
                    {
                        int index = Convert.ToInt32(command[1]);

                        mResults[index] = YouPlayer1 ? MarkType.Nought : MarkType.Cross;

                        switch (index)
                        {
                            case 0:
                                Dispatcher.BeginInvoke((Action)(() => Button0_0.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button0_0.Foreground = Brushes.Red));
                                break;
                            case 1:
                                Dispatcher.BeginInvoke((Action)(() => Button1_0.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button1_0.Foreground = Brushes.Red));
                                break;
                            case 2:
                                Dispatcher.BeginInvoke((Action)(() => Button2_0.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button2_0.Foreground = Brushes.Red));
                                break;
                            case 3:
                                Dispatcher.BeginInvoke((Action)(() => Button0_1.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button0_1.Foreground = Brushes.Red));
                                break;
                            case 4:
                                Dispatcher.BeginInvoke((Action)(() => Button1_1.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button1_1.Foreground = Brushes.Red));
                                break;
                            case 5:
                                Dispatcher.BeginInvoke((Action)(() => Button2_1.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button2_1.Foreground = Brushes.Red));
                                break;
                            case 6:
                                Dispatcher.BeginInvoke((Action)(() => Button0_2.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button0_2.Foreground = Brushes.Red));
                                break;
                            case 7:
                                Dispatcher.BeginInvoke((Action)(() => Button1_2.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button1_2.Foreground = Brushes.Red));
                                break;
                            case 8:
                                Dispatcher.BeginInvoke((Action)(() => Button2_2.Content = YouPlayer1 ? "O" : "X"));
                                if (YouPlayer1)
                                    Dispatcher.BeginInvoke((Action)(() => Button2_2.Foreground = Brushes.Red));
                                break;
                            default:
                                break;   
                        }

                        Dispatcher.Invoke((Action)(() => CheckForWinner()));

                        if (command.Length < 3 )
                        {
                            Dispatcher.BeginInvoke((Action)(() => State.Content = "Ваш ход"));
                            Dispatcher.BeginInvoke((Action)(() => State.Foreground = Brushes.Green));
                        }
                       
                        YouTurn = true;
                    }

                    if (command[0] == "win")
                    {
                        NewGame();
                    }

                    if ( command[0] == "disc")
                    {
                        MessageBox.Show("Противник отключился. Нажмите ОК чтобы вернуться в лобби");
                        Dispatcher.Invoke((Action)(() =>  BackToLobby_Click(this, EventArgs.Empty)));
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            if (!YouTurn)
            {
                return;
            }

            var button = (Button)sender;

            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);
            var index = column + (row * 3);

            if (mResults[index] != MarkType.Free)
                return;

            mResults[index] = YouPlayer1 ? MarkType.Cross : MarkType.Nought;

            button.Content = YouPlayer1 ? "X" : "O";

            if (!YouPlayer1)
                button.Foreground = Brushes.Red;
            string CheckWinRet = CheckForWinner();
            if ( CheckWinRet == "")
            {
                clientIO.SendData("mv," + index);
                State.Content = "Ход противника";
                State.Foreground = Brushes.Red;
            }
            else
            {
                clientIO.SendData("mv," + index + "," + CheckWinRet );
            }
            YouTurn = false;
        }

        private void ChangeOnesTurn()
        {
            if (YouPlayer1)
            {
                if (GameCount % 2 != 0)
                {
                    YouTurn = true;
                }
                else
                {
                    YouTurn = false;
                }
            }
            else
            {
                if (GameCount % 2 == 0)
                {
                    YouTurn = true;
                }
                else
                {
                    YouTurn = false;
                }
            }
        }

        private string CheckForWinner()
        {
            //Horizontal Wins

            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[1] & mResults[2]) == mResults[0])
            {
                mGameEnded = true;
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
            }

            if (mResults[3] != MarkType.Free && (mResults[3] & mResults[4] & mResults[5]) == mResults[3])
            {
                mGameEnded = true;
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
            }

            if (mResults[6] != MarkType.Free && (mResults[6] & mResults[7] & mResults[8]) == mResults[6])
            {
                mGameEnded = true;
                Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
            }

            // Vertical Wins
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[3] & mResults[6]) == mResults[0])
            {
                mGameEnded = true;
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;
            }

            if (mResults[1] != MarkType.Free && (mResults[1] & mResults[4] & mResults[7]) == mResults[1])
            {
                mGameEnded = true;
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;
            }

            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[5] & mResults[8]) == mResults[2])
            {
                mGameEnded = true;
                Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;
            }

            //Diagonal Wins

            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[4] & mResults[8]) == mResults[0])
            {
                mGameEnded = true;
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;
            }

            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[4] & mResults[6]) == mResults[2])
            {
                mGameEnded = true;
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;
            }

            if (mGameEnded)
            {
                if (YouTurn)
                {
                    State.Content = "Вы выйграли!";
                    State.Foreground = Brushes.Green;
                    if (YouPlayer1) p1Score++;
                    else p2Score++;
                    ScoreLable.Content = YouName + "  " + p1Score + " : " + p2Score + "  " + Pl2Name;
                    return "win";
                }
                else
                {
                    State.Content = "Вы проиграли";
                    State.Foreground = Brushes.Red;
                    if (!YouPlayer1) p1Score++;
                    else p2Score++;
                    ScoreLable.Content = YouName + "  " + p1Score + " : " + p2Score + "  " + Pl2Name;
                    return "lose";
                }
            }

            // No Winners

            if (!mResults.Any(f => f == MarkType.Free))
            {
                mGameEnded = true;

                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    button.Background = Brushes.Orange;
                });

                State.Content = "Ничья";
                State.Foreground = Brushes.Orange;
                return "draw";
            }
            return "";
        }

        private void BackToLobby_Click(object sender, EventArgs e)
        {
            clientIO.SendData("disc");
            lisen.Abort();
            Lobby lobby = new Lobby(YouName);
            lobby.Show();
            Close();
        }
    }
}
