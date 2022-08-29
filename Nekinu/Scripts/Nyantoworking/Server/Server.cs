using System.Net;
using System.Net.Sockets;

namespace NekinuSoft.NyanToWorking.ServerSide
{
    public class Server : Component
    {
        public static Server Instance;

        private Dictionary<int, Client> clients;

        private int max_players;
        private int port;

        private TcpListener listener;
        private UdpClient udp_listener;
        
        public delegate void PacketHandler(int id, Packet packet);
        private Dictionary<int, PacketHandler> packetHandlers;

        public Server(int player, int port)
        {
            max_players = player;
            this.port = port;

            clients = new Dictionary<int, Client>();
            
            listener = new TcpListener(IPAddress.Any, port);
            udp_listener = new UdpClient(port);
        }
        
        public override void Awake()
        {
            if (Instance != null && Instance != this)
            {
                //Instance.StopServer();
            }

            Instance = this;
            
            Start_Server();
        }

        private void Start_Server()
        {
            Console.WriteLine("Starting server!");
            InitServerData();
            
            listener.Start();
            listener.BeginAcceptTcpClient(IPConnectCallBack, null);

            udp_listener.BeginReceive(UDPCallBack, null);
        }

        private void UDPCallBack(IAsyncResult ar)
        {
            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);

                byte[] data = udp_listener.EndReceive(ar, ref point);
                udp_listener.BeginReceive(UDPCallBack, null);

                if (data.Length < 4)
                {
                    //Disconnect
                }

                using (Packet packet = new Packet(data))
                {
                    int id = packet.ReadInt();

                    if (id == 0)
                    {
                        return;
                    }

                    if (clients[id].Udp.EndPoint == null)
                    {
                        clients[id].Udp.Connect(point);
                        return;
                    }

                    if (clients[id].Udp.EndPoint.ToString() == point.ToString())
                    {
                        clients[id].Udp.HandleData(packet);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending UDP data {e}");
            }
        }

        private void IPConnectCallBack(IAsyncResult ar)
        {
            TcpClient client = listener.EndAcceptTcpClient(ar);
            listener.BeginAcceptTcpClient(IPConnectCallBack, null);

            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}!");
            
            for (int i = 1; i <= max_players; i++)
            {
                if (clients[i].Tcp.Socket == null)
                {
                    Console.WriteLine("Connected!");
                    clients[i].Tcp.Connect(client);
                    return;
                }
            }
            
            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect to the server. Server is full!");
            return;
        }

        private void InitServerData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived}
            };

            for (int i = 1; i <= max_players; i++)
            {
                clients.Add(i, new Client(i));
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            for (int i = 1; i <= clients.Count; i++)
            {
                if (clients[i].Id != 0)
                {
                    clients[i].Disconnect();
                }
            }
        }

        public Dictionary<int, Client> Clients => clients;
        public Dictionary<int, PacketHandler> PacketHandlers => packetHandlers;

        public void SendUDPData(IPEndPoint endPoint, Packet packet)
        {
            try
            {
                if (endPoint != null)
                {
                    udp_listener.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to {endPoint} via UDP {e}");
            }
        }
    }
}