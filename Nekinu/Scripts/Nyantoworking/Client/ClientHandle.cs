using System.Net;

namespace NekinuSoft.NyanToWorking.ClientSide
{
    public class ClientHandle
    {
        //Called when the client connects to a server
        public static void Welcome(Packet packet)
        {
            //Reads the message from the server
            string message = packet.ReadString();
            //and gets the packet id 
            int id = packet.ReadInt();
            
            //writes the message to the console
            Console.WriteLine($"Message received from server! {message}");
            //Sets the clients id
            Client.Instance.SetID(id);
            //Sends a message to the server
            ClientSend.WelcomeReceived();
            
            //Connects to the server
            Client.Instance.Udp.Connect(((IPEndPoint)Client.Instance.Tcp.Socket.Client.LocalEndPoint).Port);
        }
    }
}