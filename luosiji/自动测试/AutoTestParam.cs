using luosiji.Modbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luosiji.自动测试
{
    /// <summary>
    /// 自动测试参数类，包含串口和PLC通信对象    
    /// </summary>
    public class AutoTestParam
    {
        public SerialPort serialPort;
        public PlcCommunication PlcCommunication;
    }
}
