using System.Net;
using System.Net.Sockets;

namespace NekinuSoft.NyanToWorking.ServerSide
{
    public class Server : Component
    {
        //The instance of the server
        public static Server Instance;

        //the clients connected and their id
        private Dictionary<int, Client> clients;

        //the max amount of connections
        private int max_players;
        //the port that is being listened on
        private int port;
        
        private TcpListener listener;
        private UdpClient udp_listener;
        
        //Delegate for packets
        public delegate void PacketHandler(int id, Packet packet);
        //contains a list of all packets that can be sent
        private Dictionary<int, PacketHandler> packetHandlers;

        //constructor 
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
            //If there is a server already running, and it does not equal this instance
            if (Instance != null && Instance != this)
            {
                //Stop the server
                //Instance.StopServer();
            }

            //set the instance to this server
            Instance = this;
            
            //Starts the server
            Start_Server();
        }

        public void Start_Server()
        {
            Console.WriteLine("Starting");
            //Initializes the packet data and client list
            InitServerData();
            
            //begins the tcp listener
            listener.Start();
            //begins waiting for connections
            listener.BeginAcceptTcpClient(IPConnectCallBack, null);

            //begins waiting for data from udp
            udp_listener.BeginReceive(UDPCallBack, null);
            Console.WriteLine("Waiting for players");
        }

        private void UDPCallBack(IAsyncResult ar)
        {
            try
            {
                //Initializes a ip end point
                IPEndPoint point = new IPEndPoint(IPAddress.Any, 0);

                //gets the data from udp
                byte[] data = udp_listener.EndReceive(ar, ref point);
                //recalls this method. gets information from new connections
                udp_listener.BeginReceive(UDPCallBack, null);

                //if the data received is not complete
                if (data.Length < 4)
                {
                    //Disconnect the client
                    //Disconnect
                }

                //Creates a packet to read
                using (Packet packet = new Packet(data))
                {
                    //gets the packet id
                    int id = packet.ReadInt();

                    //if there is no id, then return out of the method
                    if (id == 0)
                    {
                        return;
                    }

                    //Checks to ensure that the client at id is null
                    if (clients[id].Udp.EndPoint == null)
                    {
                        //Then connects the client
                        clients[id].Udp.Connect(point);
                        return;
                    }

                    //if there is a client at the id, then check to see if the ip is the same
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

        //Called when a client attempts to connect
        private void IPConnectCallBack(IAsyncResult ar)
        {
            //Gets the client
            TcpClient client = listener.EndAcceptTcpClient(ar);
            //Begins waiting for a new connection
            listener.BeginAcceptTcpClient(IPConnectCallBack, null);

            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}!");
            
            //loops through the max players
            for (int i = 1; i <= max_players; i++)
            {
                //if the client at the current index is null
                if (clients[i].Tcp.Socket == null)
                {
                    //then connect the client
                    Console.WriteLine("Connected!");
                    clients[i].Tcp.Connect(client);
                    return;
                }
            }
            
            //If there isnt any spots left in the server
            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect to the server. Server is full!");
            return;
        }

        private void InitServerData()
        {
            //Initializes packet data
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived}
            };

            //Initializes clients
            for (int i = 1; i <= max_players; i++)
            {
                clients.Add(i, new Client(0));
            }
        }

        //Component method that will be called when a scene ends or an entity is destroyed
        public override void OnDestroy()
        {
            base.OnDestroy();

            for (int i = 1; i <= clients.Count; i++)
            {
                //Disconnects the clients
                if (clients[i].Id != 0)
                {
                    clients[i].Disconnect();
                }
            }
        }

        public void End()
        {
            for (int i = 1; i <= clients.Count; i++)
            {
                //Disconnects the clients
                if (clients[i].Id != 0)
                {
                    clients[i].Disconnect();
                }
                
                Console.WriteLine("Ending server");
                listener.Stop();
            }
        }

        public Dictionary<int, Client> Clients => clients;
        public Dictionary<int, PacketHandler> PacketHandlers => packetHandlers;

        //Sends udp data to a client with a specified packet
        public void SendUDPData(IPEndPoint endPoint, Packet packet)
        {
            try
            {
                //If the connection isnt null
                if (endPoint != null)
                {
                    //Send the data
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