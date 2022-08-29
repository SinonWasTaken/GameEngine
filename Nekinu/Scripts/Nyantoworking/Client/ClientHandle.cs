using System.Net;

namespace NekinuSoft.NyanToWorking.ClientSide
{
    public class ClientHandle
    {
        public static void Welcome(Packet packet)
        {
            string message = packet.ReadString();
            int id = packet.ReadInt();
            
            Console.WriteLine($"Message received from server! {message}");
            Client.Instance.SetID(id);
            ClientSend.WelcomeReceived();
            
            Client.Instance.Udp.Connect(((IPEndPoint)Client.Instance.Tcp.Socket.Client.LocalEndPoint).Port);
        }
    }
}