using System.Net;
using System.Net.Sockets;

using System.Threading;

using Common.Network;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;

namespace MyGame.Network
{
    public class Client : IDisposable
    {
        private Socket clientSocket;

        List<NetPacket> packetsToProcess = new List<NetPacket>();

        public delegate void PacketHandler(NetPacket packet);

        private PacketHandler[] PacketHandlers = new PacketHandler[(int)NetConstants.PacketId.MAX_PACKET_ID];

        private Thread receiverThread;

        private DateTime connectionTime;

        static byte[] receiveBuffer = new byte[NetConstants.MAXIMUM_PACKET_CHUNK_LENGTH];
        private float timeToNextHeartbeat = NetConstants.HEARTBEAT_INTERVAL;

        enum State
        {
            Idle,
            Connecting,
            Connected
        }
        private State state = State.Idle;

        public Client()
        {
            clientSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            clientSocket.Blocking = false;

            RegisterPacketHandler(NetConstants.PacketId.Heartbeat, (NetPacket packet) =>
            {             
                if (state == State.Connecting)
                {
                    Console.WriteLine("Connection has been established!");
                    state = State.Connected;
                }
                 
                long timestamp = 0;
                if (! packet.data.Read(ref timestamp))
                {
                    Console.WriteLine("OMFG IT DOES NOT WORK :-(");
                }
               
                long ping = GetConnectionTimeMs() - timestamp;
                Console.WriteLine("HB! Ping: " + ping + "ms");
            });
        }

        public void Dispose()
        {
            state = State.Idle;
            receiverThread.Join();
            clientSocket.Close();
        }

        private long GetConnectionTimeMs()
        {
            TimeSpan runningTime = DateTime.Now - connectionTime;
            return (long)runningTime.TotalMilliseconds;
        }

        private void StartReceiverThread()
        {
            if (receiverThread != null)
            {
                receiverThread.Join();
            }
            receiverThread = new Thread(ReceiverThread, 1024);
            receiverThread.Start(this);
        }

        public void RegisterPacketHandler(NetConstants.PacketId packetId, PacketHandler dlg)
        {
            PacketHandlers[(int)packetId] += dlg;
        }

        static void ReceiverThread(object obj)
        {
            Client client = (Client)obj;

            while (client.state != Client.State.Idle)
            {
                // process connected state

                if (client.clientSocket.Available == 0)
                {
                    continue;
                }

                int receivedBytes = 0;
                try
                {
                    receivedBytes = client.clientSocket.Receive(receiveBuffer);
                }
                catch (Exception e)
                {
                    continue;
                }

                if (receivedBytes == 0)
                {
                    continue;
                }

                NetSerializator serializator = new NetSerializator(receiveBuffer, 0, receiveBuffer.Length);

                byte protocolMagic = 0xff;
                if (!serializator.Read(ref protocolMagic))
                {
                    continue;
                }
                if (protocolMagic != NetConstants.PROTOCOL_MAGIC)
                {
                    continue;
                }

                byte protocolVersion = 0xff;
                if (!serializator.Read(ref protocolVersion))
                {
                    continue;
                }

                if (protocolVersion != NetConstants.PROTOCOL_VERSION)
                {
                    continue;
                }

                UInt32 packetCounter = 0;
                if (! serializator.Read(ref packetCounter))
                {
                    continue;
                }

                byte packetId = 0xff;
                if (!serializator.Read(ref packetId))
                {
                    continue;
                }

                if (packetId >= (byte)NetConstants.PacketId.MAX_PACKET_ID)
                {
                    continue;
                }

                NetPacket packet = new NetPacket();

                packet.packetId = (NetConstants.PacketId)packetId;
                packet.data = serializator;

                lock (client.packetsToProcess)
                {
                    client.packetsToProcess.Add(packet);
                }
            }
        }

        public void Connect(string ip, int port)
        {
            connectionTime = DateTime.Now;
            state = State.Connecting;
            timeToNextHeartbeat = 0.0f;
            clientSocket.Connect(IPAddress.Parse(ip), port);
            StartReceiverThread();
        }
       
        public void Update(GameTime gameTime)
        {
            lock (packetsToProcess)
            {
                while (packetsToProcess.Count > 0)
                {
                    NetPacket packet = packetsToProcess[0];
                    packetsToProcess.RemoveAt(0);

                    PacketHandler handler = PacketHandlers[(int)packet.packetId];
                    if (handler != null)
                        handler.Invoke(packet);
                }
            }

            if (state != State.Idle)
            {
                timeToNextHeartbeat -= (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (timeToNextHeartbeat <= 0.0f)
                {
                    NetSerializator data = new NetSerializator(8);
                    data.Write(GetConnectionTimeMs());
                    Send(NetConstants.PacketId.Heartbeat, data);

                    timeToNextHeartbeat = NetConstants.HEARTBEAT_INTERVAL;
                }
            }
        }

        UInt32 packetCounter = 0;

        public void Send(NetConstants.PacketId packetId, NetSerializator data)
        {
            NetSerializator sendSerializator = new NetSerializator();
            sendSerializator.Write(NetConstants.PROTOCOL_MAGIC);
            sendSerializator.Write(NetConstants.PROTOCOL_VERSION);
            sendSerializator.Write(++packetCounter);
            sendSerializator.Write((byte)packetId);
            sendSerializator.Write(data.GetBuffer(), data.GetSize());

            clientSocket.Send(sendSerializator.GetBuffer());
        }
    }
}
