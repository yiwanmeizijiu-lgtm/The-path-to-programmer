using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luosiji
{
    /// <summary>
    /// 全局参数，参数的存储参数管理者（操作+存取+全局访问）
    /// </summary>
    class GlobalParam
    {
        //单例模式
        private static GlobalParam instance;
        //就是这一个类
        public Param param = new Param();

        public static GlobalParam GetInstance()
        {
            if (instance == null)
            {
                instance = new GlobalParam();
            }
            return instance;
        }

        /// <summary>
        /// json文件存取参数
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool LoadParam(out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                string fileName = "setting.dat";
                string str = File.ReadAllText(fileName);
                param = JsonConvert.DeserializeObject<Param>(str);
                result = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message + ex.StackTrace;
            }
            return result;
        }
        /// <summary>
        /// 序列话保存参数
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveParam(out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                string fileName = "setting.dat";
                string str = JsonConvert.SerializeObject(param);
                File.WriteAllText(fileName, str);
                result = true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message + ex.StackTrace;
            }
            return result;
        }

    }
}
