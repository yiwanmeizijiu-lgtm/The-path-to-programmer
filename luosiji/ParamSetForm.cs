using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luosiji
{
    public partial class ParamSetForm : Form
    {
        public ParamSetForm()
        {
            InitializeComponent();
            LoadParam();
        }
        //加载参数到界面这个是从本地去读取的
        private void LoadParam()
        {
            tbx_ip.Text = GlobalParam.GetInstance().param.PlcIp;
            tbx_port.Text = GlobalParam.GetInstance().param.PlcPort.ToString();
            tbx_startPos.Text = GlobalParam.GetInstance().param.StartPos.ToString();
            tbx_endPos.Text = GlobalParam.GetInstance().param.EndPos.ToString();
            tbx_UpLimit.Text = GlobalParam.GetInstance().param.ScrewUpLimit.ToString();
        }

        //保存界面参数到全局参数
        private void SaveParam()
        {
            try
            {
                GlobalParam.GetInstance().param.PlcIp = tbx_ip.Text;
                GlobalParam.GetInstance().param.PlcPort = Convert.ToInt32(tbx_port.Text);
                GlobalParam.GetInstance().param.StartPos = Convert.ToSingle(tbx_startPos.Text);
                GlobalParam.GetInstance().param.EndPos = Convert.ToSingle(tbx_endPos.Text);
                GlobalParam.GetInstance().param.ScrewUpLimit = Convert.ToSingle(tbx_UpLimit.Text);
                //提前保存全局的参数GlobalParam
                GlobalParam.GetInstance().SaveParam(out string errorMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }
        }

       

        private void btn_save_Click(object sender, EventArgs e)
        {
            SaveParam();
        }
    }
}
