using System.Net.Sockets;
using System.Net;

using Common.Network;
using System;

namespace Server
{
    class GameServer
    {
        bool isRunning = true;

        Socket socket;

        GameServer()
        {

            Log("Setting up socket..");
            socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            Log("Socket constructed.");
            EndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7991);
            socket.Bind(endpoint);
            Log("Socket bound");
            //socket.Listen(1000);
            Log("Socket listening at port 7991");
        }

        void Log(string message)
        {
            System.Console.WriteLine("[GameServer] " + message);
        }

        byte[] receiveBuffer = new byte[NetConstants.MAXIMUM_PACKET_CHUNK_LENGTH];

        UInt32 packetCounter = 0;       

        bool Pulse()
        {
            EndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            socket.ReceiveFrom(receiveBuffer, ref sender);

            // TODO: Chunks
            NetSerializator serializator = new NetSerializator(receiveBuffer, 0, receiveBuffer.Length);

            byte protocolMagic = 0xff;
            if (!serializator.Read(ref protocolMagic))
            {
                Log("Packet corrupted");
                return isRunning;
            }
            if (protocolMagic != NetConstants.PROTOCOL_MAGIC)
            {
                Log("Invalid protocol magic!");
                return isRunning;
            }

            byte protocolVersion = 0xff;
            if (!serializator.Read(ref protocolVersion))
            {
                Log("Packet corrupted");
                return isRunning;
            }

            if (protocolVersion != NetConstants.PROTOCOL_VERSION)
            {
                Log("Invalid protocol version!");
                return isRunning;
            }

            UInt32 clientPacketCounter = 0;
            if (! serializator.Read(ref clientPacketCounter))
            {
                Log("Unable to read packet counter.");
                return isRunning;
            }

            byte packetId = 0xff;
            if (! serializator.Read(ref packetId))
            {
                Log("Unable to read packet id");
                return isRunning;
            }


            //System.Console.WriteLine("[Received] packet id " + packetId);

            if (packetId == (byte)NetConstants.PacketId.CharacterSync)
            {
                NetSerializator reply = new NetSerializator();
                reply.Write(NetConstants.PROTOCOL_MAGIC);
                reply.Write(NetConstants.PROTOCOL_VERSION);
                reply.Write(packetCounter++);
                reply.Write((byte)NetConstants.PacketId.CharacterSyncAck);
                socket.SendTo(reply.GetBuffer(), sender);
            }
            else if (packetId == (byte)NetConstants.PacketId.Heartbeat)
            {
                long timestamp = -1;
                serializator.Read(ref timestamp);

                Log("HB - " + timestamp + "ms");

                NetSerializator reply = new NetSerializator();
                reply.Write(NetConstants.PROTOCOL_MAGIC);
                reply.Write(NetConstants.PROTOCOL_VERSION);
                reply.Write(packetCounter++);
                reply.Write((byte)NetConstants.PacketId.Heartbeat);
                reply.Write(timestamp);
                socket.SendTo(reply.GetBuffer(), sender);
            }
            return isRunning;
        }

        static void Main(string[] args)
        {
            GameServer server = new GameServer();
            while (server.Pulse());
        }
    }
}
