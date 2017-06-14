using System;
using System.IO;
using System.Reflection;

namespace Common.Network
{
    /// <summary>
    /// Network serializator for sending data over network (sends data using little endian)
    /// </summary>
    public class NetSerializator
    {
        private MemoryStream memoryStream;
        private bool isReading;

        public NetSerializator(int initialCapacity = 64)
        {
            memoryStream = new MemoryStream(initialCapacity);
            isReading = false;
        }

        public NetSerializator(byte[] data, int start, int length)
        {
            ReadBuffer(data, start, length);
        }

        public byte[] GetBuffer()
        {
            return memoryStream.GetBuffer();
        }

        public long GetSize()
        {
            return memoryStream.Length;
        }

        public bool IsEmpty()
        {
            return GetSize() == 0;
        }

        private void ReadBuffer(byte[] buffer, int start, int length)
        {
            memoryStream = new MemoryStream(buffer, start, length, false);
            isReading = true;
        }

        public void Clear()
        {
            memoryStream = new MemoryStream();
        }

        private void EnsureBytesEndianess(ref byte[] data)
        {
            if (BitConverter.IsLittleEndian)
            {
                return;
            }

            // swap the bytes - not fastest but sufficient for now
            Array tmp = data;
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = (byte)tmp.GetValue(data.Length - i - 1);
            }
        }

        public bool Write(byte[] bytes, long size) 
        {
            if (isReading)
            {
                return false;
            }

            EnsureBytesEndianess(ref bytes);

            long previousStreamSize = memoryStream.Length;
            memoryStream.Write(bytes, 0, (int) size);
            return (previousStreamSize + size) == memoryStream.Length;
        }

        public bool Read(ref byte[] bytes, long size)
        {
            if (!isReading)
            {
                return false;
            }

            long bytesLeft = memoryStream.Length - memoryStream.Position;
            if (bytesLeft < size)
            {
                return false;
            }

            int readCount = memoryStream.Read(bytes, 0, (int) size);
            if (readCount != size)
            {
                return false;
            }

            EnsureBytesEndianess(ref bytes);
            return true;    
        }

        public bool Write(bool value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }

        public bool Write(char value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }
        public bool Write(byte value)
        {
            byte[] data = { value };
            return Write(data, data.Length);
        }

        public bool Write(double value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }

        // TODO
        //public bool Write(float value)
        //{
        //    byte[] data = BitConverter.GetBytes(value);
        //    return Write(data, data.Length);
        //}

        public bool Write(Int16 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }

        public bool Write(Int32 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }

        public bool Write(Int64 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }

        public bool Write(UInt16 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }

        public bool Write(UInt32 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }

        public bool Write(UInt64 value)
        {
            byte[] data = BitConverter.GetBytes(value);
            return Write(data, data.Length);
        }

        public bool Write(String value)
        {
            int length = value.Length;
            if (!Write(length))
            {
                return false;
            }

            byte[] content = System.Text.Encoding.UTF8.GetBytes(value);            
            return Write(content, length);
        }

        public bool Read(ref bool value)
        {
            long size = sizeof(bool);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToBoolean(bytes, 0);
            return true;
        }

        public bool Read(ref char value)
        {
            long size = sizeof(char);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToChar(bytes, 0);
            return true;
        }

        public bool Read(ref byte value)
        {
            long size = sizeof(byte);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = bytes[0];
            return true;
        }

        public bool Read(ref double value)
        {
            long size = sizeof(double);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToDouble(bytes, 0);
            return true;
        }

        // TODO
        //public bool Read(ref double value)
        //{
        //    long size = sizeof(double);

        //    byte[] bytes = new byte[size];
        //    if (!Read(ref bytes, size))
        //    {
        //        return false;
        //    }

        //    value = BitConverter.ToDouble(bytes, 0);
        //    return true;
        //}

        public bool Read(ref Int16 value)
        {
            long size = sizeof(Int16);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToInt16(bytes, 0);
            return true;
        }

        public bool Read(ref Int32 value)
        {
            long size = sizeof(Int32);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToInt32(bytes, 0);
            return true;
        }

        public bool Read(ref Int64 value)
        {
            long size = sizeof(Int64);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToInt64(bytes, 0);
            return true;
        }

        public bool Read(ref UInt16 value)
        {
            long size = sizeof(UInt16);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToUInt16(bytes, 0);
            return true;
        }

        public bool Read(ref UInt32 value)
        {
            long size = sizeof(UInt32);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToUInt32(bytes, 0);
            return true;
        }

        public bool Read(ref UInt64 value)
        {
            long size = sizeof(UInt64);

            byte[] bytes = new byte[size];
            if (!Read(ref bytes, size))
            {
                return false;
            }

            value = BitConverter.ToUInt64(bytes, 0);
            return true;
        }

        public bool Read(ref String value)
        {
            int length = 0;
            if (!Read(ref length))
            {
                return false;
            }

            if (length == 0)
            {
                return true;
            }

            byte[] content = new byte[length];
            if (!Read(ref content, length))
            {
                return false;
            }

            value = System.Text.Encoding.UTF8.GetString(content);
            return true;
        }

        private bool IsSupportedType(Type T)
        {
            return (T == typeof(bool) || T == typeof(char) || T == typeof(double)
                || T == typeof(Int16) || T == typeof(Int32) || T == typeof(Int64)
                || T == typeof(UInt16) || T == typeof(UInt32) || T == typeof(UInt64)
                || T == typeof(String) || T == typeof(byte));
        }

        public bool Read(ref object theObject)
        {
            return false;

        }
    }
}
