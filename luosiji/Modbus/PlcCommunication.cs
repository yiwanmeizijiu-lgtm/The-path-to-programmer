using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModbusProtocol;
namespace luosiji.Modbus
{
    /// <summary>
    /// plc通讯主要类，包含连接plc、断开plc、读写开始结束信号、读写锁付位置等方法
    /// </summary>
    public class PlcCommunication
    {
        ModbusTCP modbusTCP;
        PlcParam plcParam;
        public PlcCommunication(PlcParam _plcParam)
        {
            this.plcParam = _plcParam;
            modbusTCP = new ModbusTCP();
        }

        /// <summary>
        /// 连接plc
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool ConnectPlc(out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                result = modbusTCP.Connect(plcParam.PlcIp, plcParam.PlcPort, out errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 断开PLC
        /// </summary>c
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool DisConnectPlc(out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                result = modbusTCP.DisConnect(out errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 读取开始信号
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public int ReadStartTest(out bool result, out string errorMsg)
        {
            result = false;
            int leftRightVal = 0;
            errorMsg = string.Empty;
            try
            {
                byte[] bytes = modbusTCP.ReadHoldRegister_Byte(plcParam.SlaveAddress, PlcAddress.StartTest, 1, out result, out errorMsg);
                if (result && bytes != null)
                {
                    byte[] startTrigBytes = new byte[2];
                    Array.Copy(bytes, 0, startTrigBytes, 0, 2);
                    ushort[] val = ConvertByteArrayToShortArray(startTrigBytes);
                    leftRightVal = val[0];
                    result = true;
                }
            }
            catch (Exception ex)
            {

                errorMsg = ex.Message;
            }
            return leftRightVal;
        }

        /// <summary>
        ///  读取结束信号
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public int ReadEndTest(out bool result, out string errorMsg)
        {
            result = false;
            int value = 0;
            errorMsg = string.Empty;
            try
            {
                byte[] bytes = modbusTCP.ReadHoldRegister_Byte(plcParam.SlaveAddress, PlcAddress.EndTest, 1, out result, out errorMsg);
                if (bytes != null)
                {
                    byte[] startTrigBytes = new byte[2];
                    Array.Copy(bytes, 0, startTrigBytes, 0, 2);
                    ushort[] val = ConvertByteArrayToShortArray(startTrigBytes);
                    value = val[0];
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errorMsg = ex.Message;
            }
            return value;

        }



        /// <summary>
        /// 写开始信号
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool WriteStartTest(out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                result = modbusTCP.WriteSingleHoldRegister(plcParam.SlaveAddress, PlcAddress.StartTest, 2, out errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 写结束信号
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool WriteEndTest(out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                result = modbusTCP.WriteSingleHoldRegister(plcParam.SlaveAddress, PlcAddress.EndTest, 2, out errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }



        /// <summary>
        /// 设置锁付开始Z轴位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool WriteStartPosZ(float pos, out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                byte[] writeBytes = BitConverter.GetBytes(pos);
                byte[] reverseBytes = DisplayData.ConvertData(writeBytes, DataFormat.float_CDAB);
                result = modbusTCP.WriteMulitpleHoldRegister_Byte(plcParam.SlaveAddress, PlcAddress.ScrewStartPos, reverseBytes, out errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        ///  设置锁付结束Z轴位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool WriteEndPosZ(float pos, out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                byte[] writeBytes = BitConverter.GetBytes(pos);
                byte[] reverseBytes = DisplayData.ConvertData(writeBytes, DataFormat.float_CDAB);
                result = modbusTCP.WriteMulitpleHoldRegister_Byte(plcParam.SlaveAddress, PlcAddress.ScrewEndPos, reverseBytes, out errorMsg);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return result;
        }



        /// <summary>
        /// 将字节数组转成uint16数组
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static ushort[] ConvertByteArrayToShortArray(byte[] bytes)
        {
            List<ushort> list = new List<ushort>();
            int registerNums = (bytes.Length) / 2;
            int bytesNums = bytes.Length;
            for (int i = 0; i < bytesNums; i = i + 2)
            {
                byte[] byteArray = new byte[2];
                byteArray[1] = bytes[i];//高位字节在左
                byteArray[0] = bytes[i + 1];
                ushort value = BitConverter.ToUInt16(byteArray, 0);
                list.Add(value);
            }
            return list.ToArray();
        }
    }
}
