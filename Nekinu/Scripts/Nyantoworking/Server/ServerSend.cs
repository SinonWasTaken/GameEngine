namespace NekinuSoft.NyanToWorking.ServerSide
{
    public class ServerSend
    {
        public static void Welcome(int to_client, string message)
        {
            using (Packet packet = new Packet((int) ServerPackets.welcome))
            {
                packet.Write(message);
                packet.Write(to_client);

                SendTCPData(to_client, packet);
            }
        }

        private static void SendUDPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.Instance.Clients[toClient].Udp.SendData(packet);
        }
        
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
        
        private static void SendTCPData(int toClient, Packet packet)
        {
            packet.WriteLength();
            Server.Instance.Clients[toClient].Tcp.SendData(packet);
        }
    }
}