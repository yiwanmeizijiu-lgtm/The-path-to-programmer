using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///第一步数据模型（数据本身）参数类 
namespace luosiji
{
    public class Param
    {
        /// <summary>
        /// plc的IP地址
        /// </summary>
        public string PlcIp { get; set; }

        /// <summary>
        /// plc的端口
        /// </summary>
        public int PlcPort { get; set; } = 502;


        /// <summary>
        /// 锁付z轴起始位置
        /// </summary>
        public float StartPos { get; set; }


        /// <summary>
        /// 锁付z轴结束位置
        /// </summary>
        public float EndPos { get; set; }

        /// <summary>
        /// 合格扭力上限
        /// </summary>
        public float ScrewUpLimit { get; set; }
    }
}
