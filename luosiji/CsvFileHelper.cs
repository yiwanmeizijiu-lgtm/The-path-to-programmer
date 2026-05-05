using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luosiji
{
    public class CsvFileHelper
    {
        /// <summary>
        /// 数据存储文件
        /// </summary>
        /// <param name="maxTorque"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SaveData(double maxTorque, out string errorMsg)
        {
            bool result = false;
            errorMsg = string.Empty;
            try
            {
                //创建本地文件夹
                string dir = Path.Combine(Application.StartupPath, "Data");
                //本地csv文件
                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }
                //文件名称
                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".csv";
                //文件全路径
                string fileFullPath = Path.Combine(dir, fileName);
                if (File.Exists(fileFullPath) == false)
                {
                    //创建文件并写入表头覆盖模式
                    using (StreamWriter sw = new StreamWriter(fileFullPath, false, Encoding.UTF8))
                    {
                        //逗号分隔符，第一行写入表头
                        sw.WriteLine("时间,最大扭力");
                    }
                }
                   //追加模式写入数据
                using (StreamWriter sw = new StreamWriter(fileFullPath, true, Encoding.UTF8))
                {
                    string content = $"{DateTime.Now.ToString()},{maxTorque}";
                    sw.WriteLine(content);
                }
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
