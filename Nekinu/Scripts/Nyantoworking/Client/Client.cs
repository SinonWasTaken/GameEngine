using System.Net;
using System.Net.Sockets;
using Nekinu;

namespace NekinuSoft.NyanToWorking.ClientSide
{
    public class Client : Component
    {
        public static Client Instance;
        public const int dataBufferSize = 4096;

        [SerializedProperty] private string ip_to_connect_to = "127.0.0.1";
        
        [SerializedProperty] private int port = 5243;
        private int my_id;


        private TCP tcp;
        private UDP udp;

        private bool is_connected;

        public delegate void PacketHandler(Packet packet);
        private Dictionary<int, PacketHandler> packetHandlers;

        public override void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Instance.Destroy();
            }

            Instance = this;

            tcp = new TCP();
            udp = new UDP();
        }

        public void ConnectToServer()
        {
            InitClientData();
            tcp.Connect();
            is_connected = true;
        }

        public void SetID(int id)
        {
            my_id = id;
        }
        
        private void InitClientData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.welcome, ClientHandle.Welcome}
            };
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            
            Disconnect();
        }

        public void Disconnect()
        {
            if (is_connected)
            {
                Console.WriteLine("Disconnecting");
                
                is_connected = false;
                tcp.Socket.Close();
                udp.Socket.Close();
            }
        }

        public UDP Udp => udp;
        public TCP Tcp => tcp;
        public int MyId => my_id;
        public string IpToConnectTo => ip_to_connect_to;
        public int Port => port;
        public Dictionary<int, PacketHandler> PacketHandlers => packetHandlers;
    }

    public class TCP
    {
        private TcpClient socket;
        private Packet received_packet;
        private NetworkStream stream;

        private byte[] receive_data;

        public void Connect()
        {
            socket = new TcpClient()
            {
                ReceiveBufferSize = Client.dataBufferSize,
                SendBufferSize = Client.dataBufferSize
            };

            receive_data = new byte[Client.dataBufferSize];

            socket.BeginConnect(Client.Instance.IpToConnectTo, Client.Instance.Port, ConnectCallBack, socket);
        }

        private void ConnectCallBack(IAsyncResult ar)
        {
            socket.EndConnect(ar);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            received_packet = new Packet();
            
            stream.BeginRead(receive_data, 0, Client.dataBufferSize, ReceiveCallBack, null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int byte_length = stream.EndRead(ar);
    
                if (byte_length <= 0)
                {
                    Client.Instance.Disconnect();
                    return;
                }
    
                byte[] data = new byte[byte_length];
                Array.Copy(receive_data, data, byte_length);
    
                received_packet.Reset(HandleData(data));
                
                stream.BeginRead(receive_data, 0, Client.dataBufferSize, ReceiveCallBack, null);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        private bool HandleData(byte[] data)
        {
            int packet_length = 0;

            //Set the packet data from received data
            received_packet.SetBytes(data);
            
            //Check if the data is the length of an int
            if (received_packet.UnreadLength() >= 4)
            {
                //read in the length of the packet
                packet_length = received_packet.ReadInt();

                //if the length is 0, then the packet can be reset
                if (packet_length <= 0)
                {
                    return true;
                }
            }

            //so long as there is a full amount of data left over, the while loop continues
            while (packet_length > 0 && packet_length <= received_packet.UnreadLength())
            {
                byte[] packet_bytes = received_packet.ReadBytes(packet_length);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packet_bytes))
                    {
                        int id = packet.ReadInt();
                        Client.Instance.PacketHandlers[id](packet);
                    }
                });

                packet_length = 0;
                
                if (received_packet.UnreadLength() >= 4)
                {
                    //read in the length of the packet
                    packet_length = received_packet.ReadInt();

                    //if the length is 0, then the packet can be reset
                    if (packet_length <= 0)
                    {
                        return true;
                    }
                }
            }

            return packet_length <= 1 ? true : false;
        }

        public void SendData(Packet packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to server! {e}");
            }
        }

        public void Disconnect()
        {
            Client.Instance.Disconnect();

            stream = null;
            receive_data = null;
            received_packet = null;
            socket = null;
        }
        
        public TcpClient Socket => socket;
    }

    public class UDP
    {
        private UdpClient socket;
        private IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(Client.Instance.IpToConnectTo), Client.Instance.Port);
        }

        public void Connect(int port)
        {
            socket = new UdpClient(port);
            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallBack, null);

            using (Packet packet = new Packet())
            {
                SendData(packet);
            }
        }

        public void SendData(Packet packet)
        {
            try
            {
                packet.InsertInt(Client.Instance.MyId);
                if (socket != null)
                {
                    socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending data to server! {e}");
            }
        }
        
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                byte[] data = socket.EndReceive(ar, ref endPoint);
                socket.BeginReceive(ReceiveCallBack, null);

                if (data.Length < 4)
                {
                    Client.Instance.Disconnect();
                    return;
                }

                HandleData(data);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        private void HandleData(byte[] data)
        {
            using (Packet packet = new Packet(data))
            {
                int length = packet.ReadInt();
                data = packet.ReadBytes(length);
            }
            
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(data))
                {
                    int id = packet.ReadInt();
                    Client.Instance.PacketHandlers[id](packet);
                }
            });
        }

        public void Disconnect()
        {
            Client.Instance.Disconnect();

            socket = null;
            endPoint = null;
        }
        
        public UdpClient Socket => socket;
    }
}