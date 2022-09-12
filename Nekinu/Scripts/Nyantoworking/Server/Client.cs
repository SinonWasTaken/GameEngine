using System.Net;
using System.Net.Sockets;

namespace NekinuSoft.NyanToWorking.ServerSide
{
    //The client that exists only on the server
    public class Client
    {
        public const int dataBufferSize = 4096;
        
        private int id;
        
        private TCP tcp;
        private UDP udp;

        public Client(int id)
        {
            this.id = id;

            tcp = new TCP(id);
            udp = new UDP(id);
        }

        public int Id => id;

        public TCP Tcp => tcp;
        public UDP Udp => udp;

        public void Disconnect()
        {
            Console.WriteLine($"{tcp.Socket.Client.RemoteEndPoint} Disconnected from server");
            
            tcp.Disconnect();
            udp.Disconnect();
        }
    }
    
    public class TCP
    {
        private readonly int id = -1;

        private TcpClient socket;
        private Packet received_packet;
        private NetworkStream stream;
        private byte[] receive_data;
            
        public TCP(int id)
        {
            this.id = id;
        }
    
        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = Client.dataBufferSize;
            socket.SendBufferSize = Client.dataBufferSize;
    
            stream = socket.GetStream();

            received_packet = new Packet();
            
            receive_data = new byte[Client.dataBufferSize];
    
            stream.BeginRead(receive_data, 0, Client.dataBufferSize, ReceiveCallBack, null);
                
            ServerSend.Welcome(id, "Welcome to the server!");
        }

        public void Disconnect()
        {
            socket.Close();
            receive_data = null;
            received_packet = null;
            stream = null;
            socket = null;
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
                Console.WriteLine($"Error sending data to player! {e}");
            }
        }
        
        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int byte_length = stream.EndRead(ar);
    
                if (byte_length <= 0)
                {
                    Server.Instance.Clients[id].Disconnect();
                    return;
                }
    
                byte[] data = new byte[byte_length];
                Array.Copy(receive_data, data, byte_length);
    
                received_packet.Reset(HandleData(data));
                
                stream.BeginRead(receive_data, 0, Client.dataBufferSize, ReceiveCallBack, null);
            }
            catch (Exception e)
            {
                Server.Instance.Clients[id].Disconnect();
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
                        Server.Instance.PacketHandlers[id](id, packet);
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
    
        public TcpClient Socket => socket;
    }

    public class UDP
    {
        private IPEndPoint endPoint;

        private int id;
        
        public UDP(int id)
        {
            this.id = id;
        }

        public void Connect(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
            
            using (Packet packet = new Packet())
            {
                SendData(packet);
            }
        }

        public void Disconnect()
        {
            endPoint = null;
        }
        
        public void SendData(Packet packet)
        {
            Server.Instance.SendUDPData(endPoint, packet);
        }

        public void HandleData(Packet packet)
        {
            int length = packet.ReadInt();
            byte[] bytes = packet.ReadBytes(length);
            
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(bytes))
                {
                    int id = packet.ReadInt();
                    Server.Instance.PacketHandlers[id](id, packet);
                }
            });
        }

        public IPEndPoint EndPoint => endPoint;
    }
}