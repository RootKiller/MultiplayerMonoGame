using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Network
{
    public class NetConstants
    {
        public const byte PROTOCOL_MAGIC = 0x69;
        public const byte PROTOCOL_VERSION = 0;

        public const int MAXIMUM_PACKET_CHUNK_LENGTH = 256;

        public const float HEARTBEAT_INTERVAL = 1.0f;

        public enum PacketId
        {
            Heartbeat,
            CharacterSync,
            CharacterSyncAck,

            MAX_PACKET_ID
        }

        public enum PacketFlags
        {
            /// <summary>
            /// This packet may not be received by the other side. We send and forget.
            /// </summary>
            Unreliable = 0,

            /// <summary>
            /// The network code will do everything to deliver the packet.
            /// </summary>
            Reliable = 1<<1,

            /// <summary>
            /// The packets contain additional sequence counter and must be received in proper order.
            /// 
            /// In case connected with unreliable there may be holes between them so for example the other
            /// side will receive packet 1, 8 and 12. When reliable will always be received in 1, 2, 3, etc order.
            /// 
            /// If packet with bigger sequence counter comes first for unreliable packets packets received after that are
            /// dropped by the receiver. For reliable packets the packets with bigger sequence counter waits for the rest
            /// of packets to come before sending it to be processed.
            /// 
            /// For this packet there is additional uint32 send which is sequence counter.
            /// </summary>
            Sequenced = 1<<2,

            /// <summary>
            /// Packet with high priority - it's always put as first to process right when arrive.
            /// </summary>
            HighPriority = 1<<3,

            /// <summary>
            /// Is this packet split? If so the network code will try to glue it when next part arrives.
            /// 
            /// For this there is additional data send - uint32 send with the packet which is crc32 checksum of the content
            /// and uint32 which is an unique id of the packet so we can later distinguish which data is our packet data.
            /// </summary>
            IsSplit = 1<<4,
        }

    }
}
