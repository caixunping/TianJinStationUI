using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using Infragistics;
using Infragistics.Controls;

namespace MainUI
{
	public partial class UPS
	{
		public UPS()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}

        System.Timers.Timer timer_UPS;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (timer_UPS == null)
            {
                timer_UPS = new System.Timers.Timer();
                timer_UPS.Interval = 10000;
                timer_UPS.Elapsed += timer_ups_Elapsed;
                timer_UPS.Start();
            }
        }
        void timer_ups_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=UI_ups;password=nuclover;Database=tianjin";
            string ups_datetime, inputvol, unitoutput, outputvol, output, inputf, unitvol, upstmp, upsstatue, remaintime;
            ups_datetime= inputvol= unitoutput=outputvol= output= inputf= unitvol= upstmp= upsstatue= remaintime="";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string upstable = "tianjin."+DateTime.Now.ToString ("yyyy")+ "_ups_value";
            string command = "SELECT * FROM "+upstable+" order by datetime desc limit 1";//更新最新数据            
            MySqlCommand cmdtrace = new MySqlCommand(command, con);           
            MySqlDataReader readertrace = null;          
            con.Open();
             readertrace = cmdtrace.ExecuteReader();

             while (readertrace.Read())
             {
                 ups_datetime=readertrace[1].ToString ();
                inputvol = readertrace[2].ToString();
                unitoutput = readertrace[3].ToString();
                outputvol = readertrace[4].ToString();
                output = readertrace[5].ToString();
                inputf = readertrace[6].ToString();
                unitvol = readertrace[7].ToString();
                upstmp = readertrace[8].ToString();
                upsstatue = readertrace[9].ToString();
                remaintime = readertrace[10].ToString();               
             }
             readertrace.Close();                      
             con.Close();
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                bv_UI.Content = unitvol;
                bv_UI_Copy.Content = inputvol;
                bv_UI_Copy1.Content = inputf;
                OV_UI.Content = outputvol;
                OC_UI.Content = (Convert.ToDouble(output)*100).ToString ();
                OV_UI_Copy.Content = unitoutput;
                RT_UI.Content = remaintime;
                US_UI.Content = upstmp;
                PS_UI.Content = upsstatue;
                ups_datetime_label1.Content = ups_datetime;
                ups_datetime_label2.Content = ups_datetime;
                ups_datetime_label3.Content = ups_datetime;
                ups_datetime_label4.Content = ups_datetime;
                ups_datetime_label5.Content = ups_datetime;
                ups_datetime_label6.Content = ups_datetime;
                ups_datetime_label7.Content = ups_datetime;             
                ups_datetime_label9.Content = ups_datetime;                                        
            }), null);



        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string select_startdate = ups_startdate_picker.SelectedDate.ToString();
            string select_enddate = ups_enddate_picker.SelectedDate.ToString();
            string select_startdate2 = Convert.ToDateTime(select_startdate).ToString("yyyy-MM-dd");
            string select_enddate2 = Convert.ToDateTime(select_enddate).ToString("yyyy-MM-dd");
            string upstable = "tianjin." + DateTime.Now.ToString("yyyy") + "_ups_value";

            string starttime = select_startdate2 + " " + ups_his_start_hour.Text + ":" + ups_his_start_minute.Text + ":00";
            string endtime= select_enddate2+" "+ ups_his_end_hour.Text+":"+ ups_his_end_minute.Text + ":00";

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("监测日期");
            dt1.Columns.Add("输入电压");
            dt1.Columns.Add("放电电压");
            dt1.Columns.Add("输出电压");
            dt1.Columns.Add("输出负载");
            dt1.Columns.Add("输入频率");
            dt1.Columns.Add("单元电压");
            dt1.Columns.Add("温度");
            dt1.Columns.Add("状态");
            dt1.Columns.Add("剩余时间");

            string constr = "server=127.0.0.1;User Id=UI_ups;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + upstable + " where datetime between '" + starttime + "' and '" + endtime + "'";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();

            while (readertrace.Read())
            {
                dt1.Rows.Add(readertrace[1].ToString(), readertrace[2].ToString(), readertrace[3].ToString(), readertrace[4].ToString(), readertrace[5].ToString(), readertrace[6].ToString(), readertrace[7].ToString(), readertrace[8].ToString(), readertrace[9].ToString(), readertrace[10].ToString());

            }
            readertrace.Close();
            con.Close();
            UPS_grid.DataSource = dt1.DefaultView;
        }
    }
}