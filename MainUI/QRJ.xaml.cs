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
using System.Xml;
using System.Xml.Linq;

namespace MainUI
{
	public partial class QRJ
	{
		public QRJ()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}
        public class DataItem//电离室曲线元素定义
        {
            public string Label { get; set; }
            public string Value { get; set; }
        }   
        System.Timers.Timer timer_QRJ;
        System.Timers.Timer timer_button;
        private void rs131pg_live_line_WindowRectChanged(object sender, Infragistics.RectChangedEventArgs e)
		{
			// 在此处添加事件处理程序实现。
		}

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            button_stop.IsEnabled = false;
            button_pause.IsEnabled = false;
            button_goon.IsEnabled = false;
            button_save.IsEnabled = false;



            if (timer_QRJ == null)
            {
                timer_QRJ = new System.Timers.Timer();
                timer_QRJ.Interval = 4000;
                timer_QRJ.Elapsed += timer_QRJ_Elapsed;
                timer_QRJ.Start();
            }

            if (timer_button == null)
            {
                timer_button = new System.Timers.Timer();
                timer_button.Interval = 5000;
                timer_button.Elapsed += timer_button_Elapsed;
                timer_button.Start();
            }

        }


        void timer_button_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=UI_QRJ2;password=nuclover;Database=tianjin";
            string DianTabel2 = "tianjin." + DateTime.Now.ToString("yyyy") + "_qrj_value";
            string workNow;
            workNow = "";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + DianTabel2 + " order by id desc limit 1";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                workNow = readertrace[6].ToString();
            }
            readertrace.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (workNow == "运行")
                {
                    button_start.IsEnabled = false;
                    button_stop.IsEnabled = true;
                    button_pause.IsEnabled = true;
                    button_goon.IsEnabled = false;
                    button_save.IsEnabled = false;
                }
                else if (workNow == "停止")
                {
                    button_start.IsEnabled = true;
                    button_stop.IsEnabled = false;
                    button_pause.IsEnabled = false;
                    button_goon.IsEnabled = false;
                    button_save.IsEnabled = true;
                }
                else if (workNow == "暂停")
                {
                    button_start.IsEnabled = false;
                    button_stop.IsEnabled = true;
                    button_pause.IsEnabled = false;
                    button_goon.IsEnabled = true;
                    button_save.IsEnabled = false;

                }
                else
                {
                    button_start.IsEnabled = false;
                    button_stop.IsEnabled = false;
                    button_pause.IsEnabled = false;
                    button_goon.IsEnabled = false;
                    button_save.IsEnabled = false;
                }
            }), null);
        }



        void timer_QRJ_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=UI_QRJ2;password=nuclover;Database=tianjin";
            string DianTabel = "tianjin." + DateTime.Now.ToString("yyyy") + "_qrj_running_value";
            string DianTabel2 = "tianjin." + DateTime.Now.ToString("yyyy") + "_qrj_value";
            string ID, datatimeA, starttime, endtime, flowrate, flowrate_set, flowrate_AL, flowcount;
            ID = datatimeA=starttime = endtime = flowrate = flowrate_set = flowrate_AL = flowcount = "";
            string workstatue, worktype, counttime, powerstatue, motowork, poweralarm, mototmpalarm, falarm, cutstatue, VA, VB, VC, envirpress, envirtmp, wet, motocurrent, mototmp, airtmp;
            workstatue = worktype = counttime = powerstatue = motowork = poweralarm = mototmpalarm = falarm = cutstatue = VA = VB = VC = envirpress = envirtmp = wet = motocurrent = mototmp = airtmp = "";
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("时间日期");
            dt1.Columns.Add("瞬时流量");
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string commamd = "SELECT * FROM " + DianTabel + " order by id desc limit 1";
            string command2 = "SELECT * FROM " + DianTabel2 + " limit 1500";//更新最新数据
            string command3 = "SELECT * FROM tianjin.alartvalue";
            string command4 = "SELECT * FROM " + DianTabel2 + " order by id desc limit 1";
            MySqlCommand cmdtrace = new MySqlCommand(commamd, con);
            MySqlCommand cmdtrace2 = new MySqlCommand(command2, con);
            MySqlCommand cmdtrace3 = new MySqlCommand(command3, con);
            MySqlCommand cmdtrace4 = new MySqlCommand(command4, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace2 = null;
            MySqlDataReader readertrace3 = null;
            MySqlDataReader readertrace4 = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                datatimeA = readertrace[1].ToString();
                ID = readertrace[2].ToString();
                starttime = readertrace[3].ToString();
                endtime = readertrace[4].ToString();
                flowrate = readertrace[7].ToString();
                flowrate_set = readertrace[8].ToString();
                flowcount = readertrace[9].ToString();
            }
            readertrace.Close();

            readertrace2 = cmdtrace2.ExecuteReader();
            while (readertrace2.Read())
            {
                dt1.Rows.Add(readertrace2[1].ToString(), readertrace2[7].ToString());
            }
            readertrace2.Close();

            readertrace3 = cmdtrace3.ExecuteReader();
            while (readertrace3.Read())
            {
                flowrate_AL = readertrace3[11].ToString();
            }
            readertrace3.Close();

            readertrace4 = cmdtrace4.ExecuteReader();
            while (readertrace4.Read())
            {
                workstatue = readertrace4[6].ToString();
                worktype = readertrace4[5].ToString();
                counttime = readertrace4[11].ToString();
                powerstatue = readertrace4[22].ToString();
                motowork = readertrace4[24].ToString();
                poweralarm = readertrace4[26].ToString();
                mototmpalarm = readertrace4[25].ToString();
                falarm = readertrace4[27].ToString();
                cutstatue = readertrace4[23].ToString();
                VA = readertrace4[13].ToString();
                VB = readertrace4[14].ToString();
                VC = readertrace4[15].ToString();
                envirpress = readertrace4[20].ToString();
                envirtmp = readertrace4[19].ToString();
                wet = readertrace4[18].ToString();
                motocurrent = readertrace4[21].ToString();
                mototmp = readertrace4[16].ToString();
                airtmp = readertrace4[17].ToString();

            }
            con.Close();
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                ID_label.Content = ID;
                BeginTime_label.Content = starttime;
                endTime_label.Content = endtime;
                flowrate_label.Content = flowrate;
                flowrate_set_label.Content = flowrate_set;
                flowrate_AL_label.Content = flowrate_AL;
                textbox_flowAL.Text = flowrate_AL;
                countFlow_label.Content = flowcount;

                if (workstatue == "运行")
                {
                    workstatue_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    workstatue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                if (powerstatue == "接通")
                {
                    powerstatue_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    powerstatue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                if (motowork == "正常")
                {
                    motowork_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    motowork_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                if (poweralarm == "正常")
                {
                    poweralarm_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    poweralarm_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                if (mototmpalarm == "正常")
                {
                    mototmpalarm_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    mototmpalarm_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                if (falarm == "正常")
                {
                    falarm_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    falarm_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                if (cutstatue == "闭合")
                {
                    cutstatue_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    cutstatue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }

                workstatue_label.Content = workstatue;
                worktype_label.Content = worktype;
                counttime_label.Content = counttime;
                powerstatue_label.Content = powerstatue;
                motowork_label.Content = motowork;
                poweralarm_label.Content = poweralarm;
                mototmpalarm_label.Content = mototmpalarm;
                falarm_label.Content = falarm;
                cutstatue_label.Content = cutstatue;
                VA_label.Content = VA + " V";
                VB_label.Content = VB + " V";
                VC_label.Content = VC + " V";
                envirpress_label.Content = envirpress + " kPa";
                envirtmp_label.Content = envirtmp;
                wet_label.Content = wet + " %RH";
                motocurrent_label.Content = motocurrent + " A";
                mototmp_label.Content = mototmp + " ℃";
                airtmp_label.Content = airtmp + " ℃";
                dian_datetime_label1.Content = datatimeA;
                dian_datetime_label2.Content = datatimeA;
                dian_datetime_label3.Content = datatimeA;
                dian_datetime_label4.Content = datatimeA;
                dian_datetime_label5.Content = datatimeA;
                dian_datetime_label6.Content = datatimeA;



                var data = from row in dt1
                   .Rows.OfType<DataRow>()
                           select new DataItem()
                           {
                               Label = (string)row["时间日期"],
                               Value = (string)row["瞬时流量"]
                           };
                QRJ_live_line.DataContext = data;

            }), null);




        }

        private void button_start_Click(object sender, RoutedEventArgs e)
        {

            button_start.IsEnabled = false;
            button_stop.IsEnabled = false;
            button_pause.IsEnabled = false;
            button_goon.IsEnabled = false;

            XmlDocument run_xml3 = new XmlDocument();
            run_xml3.Load(@"C:\config\QRJRun\QRJRun.xml");
            XmlNode node3 = run_xml3.SelectSingleNode("Config/command");
            node3.InnerText = "3";
            run_xml3.Save(@"C:\config\QRJRun\QRJRun.xml");

        }

        private void button_pause_Click(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            button_stop.IsEnabled = false;
            button_pause.IsEnabled = false;
            button_goon.IsEnabled = false;

            XmlDocument run_xml3 = new XmlDocument();
            run_xml3.Load(@"C:\config\QRJRun\QRJRun.xml");
            XmlNode node3 = run_xml3.SelectSingleNode("Config/command");
            node3.InnerText = "4";
            run_xml3.Save(@"C:\config\QRJRun\QRJRun.xml");
        }

        private void button_stop_Click(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            button_stop.IsEnabled = false;
            button_pause.IsEnabled = false;
            button_goon.IsEnabled = false;

            XmlDocument run_xml3 = new XmlDocument();
            run_xml3.Load(@"C:\config\QRJRun\QRJRun.xml");
            XmlNode node3 = run_xml3.SelectSingleNode("Config/command");
            node3.InnerText = "5";
            run_xml3.Save(@"C:\config\QRJRun\QRJRun.xml");
        }

        private void button_goon_Click(object sender, RoutedEventArgs e)
        {
            button_start.IsEnabled = false;
            button_stop.IsEnabled = false;
            button_pause.IsEnabled = false;
            button_goon.IsEnabled = false;

            XmlDocument run_xml3 = new XmlDocument();
            run_xml3.Load(@"C:\config\QRJRun\QRJRun.xml");
            XmlNode node3 = run_xml3.SelectSingleNode("Config/command");
            node3.InnerText = "3";
            run_xml3.Save(@"C:\config\QRJRun\QRJRun.xml");
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}