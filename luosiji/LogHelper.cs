using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luosiji
{
    /// <summary>
    /// 本地日志数据加载帮助类
    /// </summary>
    /// 创造文件夹
    public  static class LogHelper
    {
        public static void WriteLog(string msg)
        {
            if (Directory.Exists("Log") == false)
            {
                Directory.CreateDirectory("Log");
            }
            string logName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            string logFullPath = Path.Combine("Log", logName);
            string content = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ": " + msg;
            ///true → 追加写（不会覆盖）
           /// false → 覆盖写（清空重写）
            using (StreamWriter sw = new StreamWriter(logFullPath, true))
            {
                sw.WriteLine(content);
            }
        }

    }
}
