using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeServer
{
    class ServerIO
    {
        public string ReadData(TcpClient cl)
        {
            try
            {
                var client = cl;
                var p = client.GetStream();
                byte[] buffer1 = new byte[client.ReceiveBufferSize];
                int bytesRead = p.Read(buffer1, 0, client.ReceiveBufferSize);
                string dataReceived = Encoding.UTF8.GetString(buffer1, 0, bytesRead);
                return dataReceived;
            }
            catch
            {
                return  "disc";
            }
        }
        
        public void SendData(TcpClient cl, string msg)
        {
            var client = cl;
            var p = client.GetStream();
            string textToSend = msg;
            byte[] bytesToSend = Encoding.UTF8.GetBytes(textToSend);
            p.Write(bytesToSend, 0, bytesToSend.Length);
        }
    }
}
