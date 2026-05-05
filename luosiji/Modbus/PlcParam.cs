using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luosiji.Modbus
{
    public class PlcParam
    {

        public PlcParam(string plcIp, int plcPort)
        {
            this.PlcIp = plcIp;
            this.PlcPort = plcPort;
        }
        /// <summary>
        /// ModbusTcp的Ip地址
        /// </summary>
        public string PlcIp { get; set; }

        /// <summary>
        /// ModbusTcp的端口号，默认502
        /// </summary>
        public int PlcPort { get; set; } = 502;

        /// <summary>
        /// 从站地址
        /// </summary>
        public byte SlaveAddress { get; set; } = 1;
    }
}
