using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luosiji.Modbus
{
    public static class DisplayData
    {
        public static byte[] ConvertData(byte[] bytes, DataFormat dataFormat)
        {
            byte[] newBytes = new byte[bytes.Length];
            switch (dataFormat)
            {
                case DataFormat.float_ABCD://大端正序
                    if (BitConverter.IsLittleEndian)//是不是小端存储
                    {
                        newBytes = bytes.Reverse().ToArray();
                    }
                    else
                    {
                        newBytes = bytes;
                    }
                    break;
                case DataFormat.float_DCBA://小端正序
                    if (BitConverter.IsLittleEndian)//是不是小端存储
                    {
                        newBytes = bytes;
                    }
                    else
                    {
                        newBytes = bytes.Reverse().ToArray();
                    }
                    break;
                case DataFormat.float_BADC://大端反序
                    if (BitConverter.IsLittleEndian)//是不是小端存储
                    {
                        byte[] tempBytes = bytes.Reverse().ToArray();
                        newBytes[0] = tempBytes[1];
                        newBytes[1] = tempBytes[0];
                        newBytes[2] = tempBytes[3];
                        newBytes[3] = tempBytes[2];
                    }
                    else
                    {
                        newBytes[0] = bytes[1];
                        newBytes[1] = bytes[0];
                        newBytes[2] = bytes[3];
                        newBytes[3] = bytes[2];
                    }
                    break;
                case DataFormat.float_CDAB://小端反序
                    if (BitConverter.IsLittleEndian)//是不是小端存储
                    {
                        newBytes[0] = bytes[1];
                        newBytes[1] = bytes[0];
                        newBytes[2] = bytes[3];
                        newBytes[3] = bytes[2];
                    }
                    else
                    {
                        byte[] tempBytes = bytes.Reverse().ToArray();
                        newBytes[0] = tempBytes[1];
                        newBytes[1] = tempBytes[0];
                        newBytes[2] = tempBytes[3];
                        newBytes[3] = tempBytes[2];
                    }
                    break;
            }
            return newBytes;
        }

    }

    public enum DataFormat
    {
        float_ABCD,
        float_DCBA,
        float_BADC,
        float_CDAB,
    }
}
