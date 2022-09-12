namespace NekinuSoft.NyanToWorking.ClientSide
{
    public class ClientSend
    {
        //Sends tcp data to the server
        private static void SendTCPData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.Tcp.SendData(packet);
        }

        //Sends udp data to the server
        private static void SendUDPData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.Udp.SendData(packet);
        }

        //Sends a welcome received message to the server. IE player has joined
        public static void WelcomeReceived()
        {
            using (Packet packet = new Packet((int) ClientPackets.welcomeReceived))
            {
                packet.Write("Joined the server!");
                packet.Write(Client.Instance.MyId);
                
                SendTCPData(packet);
            }
        }
    }
}