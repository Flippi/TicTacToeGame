using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



namespace TicTacToeServer
{
    class Program
    {
        public static List<ClientInfo> clientsInfo = new List<ClientInfo>();

        static void Main(string[] args)
        {
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            // IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            const int PORT_NO = 5555;
            bool serverRunning = true;
            var listener = new TcpListener(ipAddress, PORT_NO);
            TcpClient client;
            ServerIO serverIO = new ServerIO();
            Console.WriteLine("Start");
            listener.Start();
            while (serverRunning) 
            {
                client = listener.AcceptTcpClient();
                string data = serverIO.ReadData(client);
                string[] command = data.Split(',');

                if ( command[0] == "conn")
                {
                    string playersNicks = "plist.sing,";

                    foreach (var item in clientsInfo)
                    {
                        playersNicks = playersNicks + item.nickname + ",";
                    }
                    playersNicks.Substring(0, playersNicks.Length - 1);
                    serverIO.SendData(client, playersNicks);

                    foreach (var item in clientsInfo)
                    {
                        serverIO.SendData(item.clientTcp, "plist.add," + command[1]);
                    }

                    clientsInfo.Add(new ClientInfo() { clientTcp = client, nickname = command[1] });

                    ParameterizedThreadStart _start = new ParameterizedThreadStart(ClientInLobby);
                    var thread = new Thread(_start);
                    object Cl = client;
                    thread.Start(Cl);
                }
                
            }
        }

        public static void ClientInLobby(object Client)
        {
            TcpClient client = (TcpClient)Client;
            ServerIO serverIO = new ServerIO();
            while (client.Connected)
            {
                string data = serverIO.ReadData(client);
                string[] command = data.Split(',');

                if (command[0] == "play")
                {
                    string FirstPlNick = command[1];
                    string SecondPlNick = command[2];
                    
                    var secondClient = clientsInfo.Find(x => x.nickname.Contains(SecondPlNick));
                    
                    serverIO.SendData(secondClient.clientTcp, "play,false," + FirstPlNick);

                    ParameterizedThreadStart _start = new ParameterizedThreadStart(Game);
                    var thread = new Thread(_start);

                    TcpClient[] cl = new TcpClient[2];
                    cl[0] = client;
                    cl[1] = secondClient.clientTcp;
                    object TCPclients = cl;

                    command[0] = "disc";

                    thread.Start(TCPclients);
                }
                if (command[0] == "disc")
                {
                    string nick = command[1];

                    var discClient = clientsInfo.Find(x => x.nickname.Contains(nick));
                    clientsInfo.Remove(discClient);

                    foreach (var item in clientsInfo)
                    {
                        serverIO.SendData(item.clientTcp, "plist.del," + nick);
                    }
                    return;
                }
            }
        }

        public static void Game(object Cl)
        {
            TcpClient[] clients = (TcpClient[])Cl;
            TcpClient client1 = clients[0];
            TcpClient client2 = clients[1];
            ServerIO serverIO = new ServerIO();
            int p1Score = 0;
            int p2Score = 0;
            string msg;

            while (client1.Connected && client2.Connected)
            {
                msg = serverIO.ReadData(client1);
                string[] command = msg.Split(',');
                if (command.Length > 2 && command[2] == "win")
                {
                    p1Score++;
                    Console.WriteLine(p1Score + "  :  " + p2Score);
                }
                serverIO.SendData(client2, msg);
                if ( command[0] == "disc")
                {
                    return;
                }
                msg = serverIO.ReadData(client2);
                command = msg.Split(',');
                if (command.Length > 2 && command[2] == "win")
                {
                    p2Score++;
                    Console.WriteLine(p1Score + "  :  " + p2Score);
                }
                serverIO.SendData(client1, msg);
                if (command[0] == "disc")
                {
                    return;
                }
            }

        }

    }

    class ClientInfo
    {
        public TcpClient clientTcp;
        public string nickname;
    }

}
