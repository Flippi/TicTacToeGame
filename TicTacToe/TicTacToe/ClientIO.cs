using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TicTacToe
{
    public class ClientIO
    {
        TcpClient client;

        public ClientIO(TcpClient cl)
        {
            client = cl;
        }

        public void SendData(string msg)
        {
            string textToSend = msg;
            byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
            var nwStream = client.GetStream();
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            return;
        }

        public string ReadData ()
        {
            var nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return dataReceived;
        }
    }
}
