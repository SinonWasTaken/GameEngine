namespace NekinuSoft.NyanToWorking.ServerSide
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int client, Packet packet)
        {
            string msg = packet.ReadString();
            int id = packet.ReadInt();

            if (id != client)
            {
                Console.WriteLine($"Player {id} has assumed wrong id {client}");
            }
            
            Console.WriteLine($"{Server.Instance.Clients[id].Tcp.Socket.Client.RemoteEndPoint} has connected successfully. Now has ID of {id}");
        }
    }
}