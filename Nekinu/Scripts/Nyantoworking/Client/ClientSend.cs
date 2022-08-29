namespace NekinuSoft.NyanToWorking.ClientSide
{
    public class ClientSend
    {
        private static void SendTCPData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.Tcp.SendData(packet);
        }

        private static void SendUDPData(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.Udp.SendData(packet);
        }

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