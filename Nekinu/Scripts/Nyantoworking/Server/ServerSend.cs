namespace NekinuSoft.NyanToWorking.ServerSide
{
    public class ServerSend
    {
        //Sends a welcome packet to a new client
        public static void Welcome(int to_client, string message)
        {
            //Creates a new packet with an id of 'ServerPackets.Welcome'
            using (Packet packet = new Packet((int) ServerPackets.welcome))
            {
                //writes a message 
                packet.Write(message);
                //Writes a id
                packet.Write(to_client);

                //Sends the packet through tcp
                SendTCPData(to_client, packet);
            }
        }

        //Sends data via UDP
        private static void SendUDPData(int toClient, Packet packet)
        {
            //writes the packet information
            packet.WriteLength();
            //sends the packet to a client
            Server.Instance.Clients[toClient].Udp.SendData(packet);
        }
        
        //Same as the method above, but it sends it to all connected clients
        private static void SendUDPDataToAll(Packet packet)
        {
            for (int i = 1; i <= Server.Instance.Clients.Count; i++)
            {
                if (Server.Instance.Clients[i].Udp.EndPoint != null)
                {
                    Server.Instance.Clients[i].Udp.SendData(packet);
                }
            }
        }
        
        //Same as the method above, but it sends it to all connected clients except for one
        private static void SendUDPDataToAll(Packet packet, int exception)
        {
            for (int i = 1; i <= Server.Instance.Clients.Count; i++)
            {
                if (i != exception)
                {
                    if (Server.Instance.Clients[i].Udp.EndPoint != null)
                    {
                        Server.Instance.Clients[i].Udp.SendData(packet);
                    }
                }
            }
        }
        
        //Same as the method above, but via TCP
        private static void SendTCPDataToAll(Packet packet)
        {
            for (int i = 1; i <= Server.Instance.Clients.Count; i++)
            {
                if (Server.Instance.Clients[i].Tcp.Socket != null)
                {
                    Server.Instance.Clients[i].Tcp.SendData(packet);
                }
            }
        }

        //Same as the method above, but it sends to all connected clients via TCP
        private static void SendTCPDataToAll(Packet packet, int exception)
        {
            for (int i = 1; i <= Server.Instance.Clients.Count; i++)
            {
                if (i != exception)
                {
                    if (Server.Instance.Clients[i].Tcp.Socket != null)
                    {
                        Server.Instance.Clients[i].Tcp.SendData(packet);
                    }
                }
            }
        }
        
        //Sends tcp data to one specific person
        private static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.Instance.Clients[toClient].Tcp.SendData(packet);
        }
    }
}