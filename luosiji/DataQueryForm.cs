using MySql.Data.MySqlClient;
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
    public partial class DataQueryForm : Form
    {
        public DataQueryForm()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }
        //查询功能
        private void Query()
        {
            //初始化图表
            chart1.Series[0].Points.Clear();

            DateTime startime= dateTimePicker_strart.Value;
            DateTime endtime= dateTimePicker_end.Value;

            MySqlParameter[] mySqlparameters = new MySqlParameter[2];
            mySqlparameters[0]= new MySqlParameter("@start_time", startime);
            mySqlparameters[1] = new MySqlParameter("@end_time", endtime);
            string str = "SELECT torque FROM torque_info WHERE test_time>=@start_time AND test_time<=@end_time";
            
            
            DataSet dataSet= MySqlHelper.GetDataSet(str, mySqlparameters);

           List <double> torqueValueList= new List<double>();
            if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count>0)
            {
                for (int i=0; i < dataSet.Tables[0].Rows.Count; i++)
                {
                    double torqueValue = dataSet.Tables[0].Rows[i].Field<double>("torque");
                    torqueValueList.Add(torqueValue);
                }
            
            }
            //判断值大于0才调用显示
            if (torqueValueList.Count>0)
            {

                showUi(torqueValueList);
            
            }

        }

        //这个是属性显示界面
        private void showUi(List<double> torqueValueList)
        { 
            List<double> xlist= new List<double>();
            for (int i=0;i< torqueValueList.Count; i++)
            { 
            xlist.Add(i);

            }
            chart1.Series[0].Points.DataBindXY(xlist, torqueValueList);

        }

        private void btn_query_Click(object sender, EventArgs e)
        {
            Query();
        }
        /// <summary>
        /// 初始化图标
        /// </summary>
        private void InitialUi()
        {

            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].Color = Color.LimeGreen;
            chart1.Series[0].LegendText = "扭力";
            chart1.Series[0].BorderWidth = 3;

        }

        private void DataQueryForm_Load(object sender, EventArgs e)
        {
            InitialUi();
        }
    }
}
