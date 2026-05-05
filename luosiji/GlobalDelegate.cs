using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace luosiji
{
    /// <summary>
    /// 定义委托（全局访问）
    /// </summary>
    public class GlobalDelegate

    {
        public static Action<string> UpdateUiLogDelegate;
        public static Action<List<byte>> UpdateDataDelegate;

    }
}
