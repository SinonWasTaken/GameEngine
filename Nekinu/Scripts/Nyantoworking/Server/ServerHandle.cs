namespace NekinuSoft.NyanToWorking.ServerSide
{
    public class ServerHandle
    {
        
        //Called when a client successfully connects to the server
        public static void WelcomeReceived(int client, Packet packet)
        {
            //Reads the client message
            string msg = packet.ReadString();
            //reads the client id
            int id = packet.ReadInt();

            //If the client and packet id arent equal
            if (id != client)
            {
                Console.WriteLine($"Player {id} has assumed wrong id {client}");
            }
            
            //Writes the client message
            Console.WriteLine($"{Server.Instance.Clients[id].Tcp.Socket.Client.RemoteEndPoint} has connected successfully. Now has ID of {id}");
        }
    }
}