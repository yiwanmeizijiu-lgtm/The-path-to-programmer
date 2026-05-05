using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luosiji.自动测试
{
    public class AutoTest
    {
        //保存外部传进来的资源和工具。
        private readonly AutoTestParam _autoTestParam;
        /// <summary>
        /// 声明状态机，包含测试的各个步骤，每个步骤对应一个状态，在Run方法中循环调用，根据当前状态执行相应的操作
        /// </summary>
        Action stateMachine;
        bool result = false;
        string errorMsg = string.Empty;
        //控制这个后台循环什么时候停。
        CancellationToken cancellationToken;
        CancellationTokenSource tokenSource;
        //数据缓存区，声明一个读取的
        List<byte> torqueList = new List<byte>();

        //构造函数，传入参数，初始化状态机和取消令牌，并启动后台线程运行Run方法
        public AutoTest(AutoTestParam autoTestParam) 
        {
            //把外部传进来的串口、PLC 等资源保存到当前对象里。
            this._autoTestParam=autoTestParam;  
            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;
            //首个方法应该是初始化
            stateMachine = InitialTest;
            //线程开启
            Task.Run(new Action(Run));
        }

        /// <summary>
        /// 自动测试流程开始后台线程一直开启
        /// </summary>
        private void Run()
        {
            while (true)
            {
                stateMachine();
                Thread.Sleep(5);
                //开始的令牌
                if (tokenSource.IsCancellationRequested)
                {      
                break;
                }
            }      
        }
        /// <summary>
        /// 读取测试开始信号，开始信号由PLC写入，当PLC写入开始信号后，状态机进入读取扭矩状态
        /// </summary>
        /// 刚刚开始的时候就会一直读取
        /// 这里是值返回的连接成功成为2
        private void ReadStarTest()
        {

            if (_autoTestParam.PlcCommunication.ReadStartTest(out  result, out  errorMsg) == 1)
            {
                GlobalDelegate.UpdateUiLogDelegate("读取到PLC开始信号");
                _autoTestParam.PlcCommunication.WriteStartTest(out errorMsg);
                stateMachine = ReadTorque;
            }

        }
        /// <summary>
        /// 开始读取扭力数据（业务）
        /// </summary>
        private void ReadTorque()
        {
            //先判断PLC是否写入结束信号，如果写入结束信号，说明测试结束，状态机进入更新界面状态；如果没有写入结束信号，说明测试还在进行中，继续读取串口数据并保存到缓存区
            if (_autoTestParam.PlcCommunication.ReadEndTest(out bool result, out string errorMsg) == 1)
            {
                GlobalDelegate.UpdateUiLogDelegate("读取到plc结束锁附信号");
                _autoTestParam.PlcCommunication.WriteEndTest(out errorMsg);
                stateMachine = UpdateUi;
            }
            else
            {
                int nums = _autoTestParam.serialPort.BytesToRead;
                if (nums > 0)
                {
                    byte[] bytes = new byte[nums];
                    _autoTestParam.serialPort.Read(bytes, 0, bytes.Length);
                    torqueList.AddRange(bytes);
                }
            }


        }

        /// <summary>
        /// 停止测试在主界面进行调用
        /// </summary>
        public void StopTest()
        {
            //取消的令牌
            tokenSource.Cancel();
        }


        /// <summary>
        /// 初始化测试
        /// </summary>
        private void InitialTest()
        {
            GlobalDelegate.UpdateUiLogDelegate("自动测试已开启");
            _autoTestParam.PlcCommunication.WriteStartPosZ(GlobalParam.GetInstance().param.StartPos,out  errorMsg);
            _autoTestParam.PlcCommunication.WriteStartPosZ(GlobalParam.GetInstance().param.EndPos, out   errorMsg);
            //初始化后读取开始
            stateMachine = ReadStarTest;
        }

       
    
        /// <summary>
        /// 更新界面
        /// </summary>
        private void UpdateUi()
        {
            GlobalDelegate.UpdateUiLogDelegate("开始更新界面数据");
            //清空数据缓存区，准备下一轮测试
            GlobalDelegate.UpdateDataDelegate(torqueList);
            torqueList.Clear();
            stateMachine = ReadStarTest;
        }

       


    }
}
