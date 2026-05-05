using luosiji.Modbus;
using luosiji.自动测试;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace luosiji
{
    public partial class MainForm : Form
    {
        string errorMsg;
        //plc连接
        PlcCommunication plcCommunication;
        //串口类
        SerialPort serialPort=new SerialPort();
        //本地
        CsvFileHelper csvFileHelper = new CsvFileHelper();
        //先给个null，等到点击开始测试按钮时才实例化，避免占用资源
        AutoTest autoTest = null;
        public MainForm()
        {
            InitializeComponent();
            GlobalDelegate.UpdateUiLogDelegate = UpdateUiLog;
            GlobalDelegate.UpdateDataDelegate = UpdataData;


            if (GlobalParam.GetInstance().LoadParam(out errorMsg) == false)
            {
                LogHelper.WriteLog(errorMsg);
                MessageBox.Show(errorMsg);
            }
            else
            {
               LogHelper.WriteLog("参数加载成功");
            }
            PlcParam plcParam = new PlcParam(GlobalParam.GetInstance().param.PlcIp, GlobalParam.GetInstance().param.PlcPort);
            plcCommunication = new PlcCommunication(plcParam);

        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            string errorMsg;
            GlobalDelegate.UpdateUiLogDelegate("参数加载成功");
          
            if (plcCommunication.ConnectPlc(out  errorMsg))
            {
                toolStripStatusLabel1_plc.Text= "plc已连接";
                toolStripStatusLabel1_plc.BackColor = Color.Green;
            }
            else
            {
                toolStripStatusLabel1_plc.Text = "plc已断开";
                toolStripStatusLabel1_plc.BackColor = Color.Red;
            }
            InitailPortName();
            InitialBaudRate();
            InitialDataBits();
            InitialStopBits();
            InitialParityBits();
            InitialUi();



        }

      
        /// <summary>
        /// 设置按钮展示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_set_Click(object sender, System.EventArgs e)
        {
            using (ParamSetForm paramSetForm = new ParamSetForm())
            {
                paramSetForm.ShowDialog();
            }
        }
        /// <summary>
        /// /日志模块必须在UI线程更新界面，所以需要使用Invoke方法来确保线程安全地更新日志显示。
        /// </summary>
        /// <param name="msg"></param>
        private void UpdateUiLog(string msg)
        {
            this.Invoke(new Action(() =>
            {
                string[] lines = richTextBox1.Lines;
                if (lines.Length > 200)
                {
                    richTextBox1.Clear();
                }
                string str = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " " + msg + "\n";
                richTextBox1.AppendText(str);
            }));
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="bytes"></param>
        private void UpdataData(List<byte>bytes)
        {
            UpdataChart(bytes);
            UpdateDataGridView( bytes);
            

        }

        /// <summary>
        /// 更新曲线
        /// </summary>
        /// <param name="bytes"></param>
        private void UpdataChart(List<byte>bytes)
        {
            List<double> xList = new List<double>();
            for (int i = 0; i < bytes.Count; i++)
            {
                xList.Add(i);
            }
            //跨线程更新ui
            this.Invoke(new Action(() =>
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[0].Points.DataBindXY(xList, bytes);
            }));
        }
        /// <summary>
        /// 更新表格
        /// </summary>
        /// <param name="bytes"></param>
        private void UpdateDataGridView(List<byte> bytes)
        {
            byte maxVal = bytes.Max();//最大扭力
            double averageVal = bytes.Select(item => Convert.ToInt32(item)).Average();//平均扭力
            UpdateResult(maxVal);
            //存储数据库
            SaveDataToDatabase(maxVal);
            csvFileHelper.SaveData(maxVal, out errorMsg);
           
            ///跨线程更新Ui
            this.Invoke(new Action(() => {

                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = index;
                dataGridView1.Rows[index].Cells[1].Value = maxVal;
                dataGridView1.Rows[index].Cells[2].Value = averageVal;
                dataGridView1.Rows[index].Cells[3].Value = DateTime.Now.ToString();
            }));
        }
        /// <summary>
        /// 更新结果
        /// </summary>
        /// <param name="maxTorque"></param>
        private void UpdateResult(double maxTorque)
        {
            this.Invoke(new Action(() => {
                if (maxTorque >= GlobalParam.GetInstance().param.ScrewUpLimit)
                {
                    lbl_result.Text = "OK";
                    lbl_result.BackColor = Color.LimeGreen;
                }
                else
                {
                    lbl_result.Text = "NG";
                    lbl_result.BackColor = Color.Red;
                }
            }));

        }

/// <summary>
/// 对表格控件进行操作，在加载的时候进行
/// </summary>
        private void InitialUi()
        {
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].Color = Color.Red;
            chart1.Series[0].LegendText = "扭力";

            //初始化表格控件
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("序号", "序号");
            dataGridView1.Columns.Add("最大扭力", "最大扭力");
            dataGridView1.Columns.Add("平均扭力", "平均扭力");
            dataGridView1.Columns.Add("测试时间", "测试时间");
            dataGridView1.RowHeadersVisible = false;
        }


        #region 串口通信

        /// <summary>
        /// 端口号
        /// </summary>
        private void InitailPortName()
        {
            string[] ports = SerialPort.GetPortNames();
            comboBox1_port.Items.Clear();
            if (ports != null && ports.Length > 0)
            {
                comboBox1_port.Items.AddRange(ports);
                comboBox1_port.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// 波特率
        /// </summary>
        private void InitialBaudRate()
        {
            int[] baudRate = new int[] { 4800, 9600, 115200 };
            comboBox_baud.Items.Clear();
            comboBox_baud.Items.AddRange(baudRate.Select(item => item.ToString()).ToArray());
            comboBox_baud.SelectedIndex = 1;
        }

        /// <summary>
        /// 数据位
        /// </summary>
        private void InitialDataBits()
        {
            int[] databits = new int[] { 6, 7, 8 };
            comboBox_databit.Items.Clear();
            comboBox_databit.Items.AddRange(databits.Select(item => item.ToString()).ToArray());
            comboBox_databit.SelectedIndex = 2;
        }

        /// <summary>
        /// 停止位
        /// </summary>
        private void InitialStopBits()
        {
            double[] stopBits = new double[] { 1, 1.5, 2 };
            comboBox_stop.Items.Clear();
            comboBox_stop.Items.AddRange(stopBits.Select(item => item.ToString()).ToArray());
            comboBox_stop.SelectedIndex = 0;
        }

        /// <summary>
        /// 校验位
        /// </summary>
        private void InitialParityBits()
        {

            string[] parity = new string[] { "None", "Odd", "Even" };
            comboBox_parity.Items.Clear();
            comboBox_parity.Items.AddRange(parity.Select(item => item.ToString()).ToArray());
            comboBox_parity.SelectedIndex = 0;
        }

        #endregion
        //屏幕打开按钮
        private void btn_openport_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_openport.Text == "打开")
                {
                    if (serialPort.IsOpen == false)
                    {
                        //配置串口参数并打开串口
                        serialPort.PortName = comboBox1_port.SelectedItem.ToString();
                        serialPort.BaudRate = Convert.ToInt32(comboBox_baud.SelectedItem);
                        //数据位
                        serialPort.DataBits = Convert.ToInt32(comboBox_databit.SelectedItem);
                        //停止位
                        StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox_stop.SelectedItem.ToString());
                        serialPort.StopBits = stopBits;
                        Parity parity = (Parity)Enum.Parse(typeof(Parity), comboBox_parity.SelectedItem.ToString());
                        serialPort.Parity = parity;
                        serialPort.Open();
                        btn_openport.Text = "关闭";
                        toolStripStatusLabel1_screw.Text = "螺丝机已连接";
                        toolStripStatusLabel1_screw.BackColor = Color.Lime;
                    }
                }  
                else
                {
                    serialPort.Close();
                    btn_openport.Text = "打开";
                    toolStripStatusLabel1_screw.Text = "螺丝机已断开";
                    toolStripStatusLabel1_screw.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (GlobalParam.GetInstance().SaveParam(out errorMsg)==false)
            { 
            
            MessageBox.Show(errorMsg);
           
            }
        }

        private bool SaveDataToDatabase(double maxTorque)
        {
            bool result = false;
            MySqlParameter[] mySqlParameters = new MySqlParameter[2];
            mySqlParameters[0] = new MySqlParameter("@max_torque", maxTorque);
            mySqlParameters[1] = new MySqlParameter("@time", DateTime.Now);
            string sql = "insert into torque_info(torque,test_time) values(@max_torque,@time)";
            int rows = MySqlHelper.ExecuteSql(sql, mySqlParameters);
            if (rows > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private void toolStripMenuItem_query_Click(object sender, EventArgs e)
        {
            try 
            {

                using (DataQueryForm dataQueryForm = new DataQueryForm())
                {
                    dataQueryForm.ShowDialog();

                }
            }
            catch (Exception ex)
            {
            MessageBox.Show(ex.Message);
            }
           
        }
        //开始测试按钮
        private void btn_autoTest_Click(object sender, EventArgs e)
        {
            if (btn_autoTest.Text == "开始测试")
            {
                //前面先行赋值null，等到点击开始测试按钮时才实例化，避免占用资源
                AutoTestParam autoTestParam = new AutoTestParam();
                //把串口和plc进行传参进入autoTestParam
                autoTestParam.serialPort = this.serialPort;
                autoTestParam.PlcCommunication = plcCommunication;
                //把创建好的参数传入AutoTest
                autoTest = new AutoTest(autoTestParam);
                btn_autoTest.Text = "停止测试";
            }
            else
            {
                //调用了AutoTest的类中停止测试的这样一个功能
                autoTest.StopTest();
                btn_autoTest.Text = "开始测试";
            }
        }
    }
}
