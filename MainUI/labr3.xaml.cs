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
	public partial class labr3
	{
		public labr3()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}
        System.Timers.Timer timer_labr3;

        private void Button_Click(object sender, RoutedEventArgs e)//查询按钮
        {
            timer_labr3.Stop();
            try
            {
                string select_startdate = Labr3_startdate_picker.SelectedDate.ToString();
                string select_enddate = Labr3_enddate_picker.SelectedDate.ToString();
                string select_startdate2 = Convert.ToDateTime(select_startdate).ToString("yyyy-MM-dd");
                string select_enddate2 = Convert.ToDateTime(select_enddate).ToString("yyyy-MM-dd");
                string saratable = "tianjin." + DateTime.Now.ToString("yyyy") + "_sara_value";

                labr3_his_starttime.Text = select_startdate2 + " " + Labr3_his_start_hour.Text + ":" + Labr3_his_start_minute.Text + ":00";
                labr3_his_endtime.Text = select_enddate2 + " " + Labr3_his_end_hour.Text + ":" + Labr3_his_end_minute.Text + ":00";

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("监测日期");

                string constr = "server=127.0.0.1;User Id=UI_sara2;password=nuclover;Database=tianjin";
                MySqlConnection con;
                con = new MySql.Data.MySqlClient.MySqlConnection(constr);
                string command = "SELECT datetime FROM " + saratable + " where datetime between '" + labr3_his_starttime.Text + "' and '" + labr3_his_endtime.Text + "'";//更新最新数据
                MySqlCommand cmdtrace = new MySqlCommand(command, con);
                MySqlDataReader readertrace = null;
                con.Open();
                readertrace = cmdtrace.ExecuteReader();

                while (readertrace.Read())
                {
                    dt1.Rows.Add(readertrace[0].ToString());
                }
                readertrace.Close();
                con.Close();
                Labr3_nuclide_grid_Copy.DataSource = dt1.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询有误，请重新输入！");
            }
            
        }

        public class DataItem//曲线元素定义
        {
            public string Label { get; set; }//x
            public string Value { get; set; }//y
        }     

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (timer_labr3 == null)
            {
                timer_labr3 = new System.Timers.Timer();
                timer_labr3.Interval = 10000;
                timer_labr3.Elapsed += timer_labr3_Elapsed;
                timer_labr3.Start();
            }

        }
        private Decimal ChangeDataToD(string strData)//科学计数法转数字
        {
            Decimal dData = 0.0M;
            if (strData.Contains("e"))
            {
                dData = Convert.ToDecimal(Decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
            }
            return dData;
        }

        void timer_labr3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string Labr3_startTime, Labr3_endTime, Labr3_ID, Labr3_HV, Labr3_tm, Labr3_liveTime, Labr3_page1_doserate, Labr3_page1_nuclide, Labr3_datetime, Labr3_ch,Labr3_conf,Labr3_act,Labr3_unclide_doserate,Labr3_nuclide_ch,Labr3_en,Labr3_area; 
            Labr3_startTime = "";
            Labr3_endTime = "";
            Labr3_ID = "";
            Labr3_HV = "";
            Labr3_tm = "";
            Labr3_liveTime = "";
            Labr3_page1_doserate = "";
            Labr3_page1_nuclide = "";
            Labr3_datetime = "";
            Labr3_ch = "";
            Labr3_conf="";
            Labr3_act="";
            Labr3_unclide_doserate="";
            Labr3_nuclide_ch="";
            Labr3_en="";
            Labr3_area="";
            string saratable = "tianjin."+DateTime.Now.ToString("yyyy")+ "_sara_value"; 


            string constr = "server=127.0.0.1;User Id=UI_sara2;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM "+ saratable + " order by datetime desc limit 1";//更新最新数据
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();

            while (readertrace.Read())
            {                              
                Labr3_datetime = readertrace[1].ToString();
                Labr3_ID = readertrace[2].ToString();
                Labr3_startTime = readertrace[3].ToString();
                Labr3_endTime = readertrace[4].ToString();
                Labr3_page1_doserate = readertrace[5].ToString();
                Labr3_HV = readertrace[6].ToString();
                Labr3_tm = readertrace[7].ToString();
                Labr3_ch = readertrace[8].ToString();
                Labr3_liveTime = readertrace[9].ToString();
                Labr3_page1_nuclide = readertrace[10].ToString();
                Labr3_act=readertrace[11].ToString ();
                Labr3_conf=readertrace[12].ToString ();
                Labr3_unclide_doserate=readertrace[13].ToString ();
                Labr3_nuclide_ch=readertrace[14].ToString ();
                Labr3_en=readertrace[15].ToString ();
                Labr3_area=readertrace[16].ToString ();
            }
            readertrace.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                page_labr3_doserate_label.Content =Math.Round (ChangeDataToD(Labr3_page1_doserate) * 1000,1);
                SARA_datetime_label1.Content = Labr3_datetime;
                page_labr3_date_label.Content = DateTime.Parse(Labr3_datetime).ToString("yyyy-MM-dd");
                page_labr3_time_label.Content = DateTime.Parse(Labr3_datetime).ToString("HH:mm:ss");
                page_labr3_ID_label.Content = Labr3_ID;
                page_labr3_HV_label.Content = Labr3_HV;
                page_labr3_startdate_label.Content = DateTime.Parse(Labr3_startTime).ToString("yyyy-MM-dd");
                page_labr3_starttime_label.Content = DateTime.Parse(Labr3_startTime).ToString("HH:mm:ss");
                page_labr3_tm_label.Content = Labr3_tm;
                page_labr3_enddate_label.Content = DateTime.Parse(Labr3_endTime).ToString("yyyy-MM-dd");
                page_labr3_endtime_label.Content = DateTime.Parse(Labr3_endTime).ToString("HH:mm:ss");
                page_labr3_livetime_label.Content = Labr3_liveTime;

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("核素名称");
                dt1.Columns.Add("置信度");
                dt1.Columns.Add("贡献剂量率");
                dt1.Columns.Add("发现通道");
                dt1.Columns.Add("核素能量");
                dt1.Columns.Add("峰面积");
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Channel");
                dt2.Columns.Add("Counts");

                string[] nuclide_orgin = Labr3_page1_nuclide.Split(';');
                string[] conf_orgin = Labr3_conf.Split(';');
                string[] nu_doserate_orgin = Labr3_unclide_doserate.Split(';');
                string[] nu_ch_orgin = Labr3_nuclide_ch.Split(';');
                string[] nu_Labr3_en_orgin = Labr3_en.Split(';');
                string[] nu_Labr3_area_orgin = Labr3_area.Split(';');

                int count;
                count = nuclide_orgin.Length;
                for (int a = 0; a < count-1;a++ )
                {
                    dt1.Rows.Add(nuclide_orgin[a], conf_orgin[a], Math.Round(ChangeDataToD(nu_doserate_orgin[a]) * 1000, 1), nu_ch_orgin[a], nu_Labr3_en_orgin[a], nu_Labr3_area_orgin[a]);
                }

                Labr3_nuclide_grid.DataSource = dt1.DefaultView;
                string[] ch_orgin = Labr3_ch.Split(',');
                int count_ch = ch_orgin.Length;
                for (int a = 0; a < count_ch;a++ )
                {
                    dt2.Rows.Add(a,ch_orgin[a]);
                }
                var data = from row in dt2
                    .Rows.OfType<DataRow>()
                           select new DataItem()
                           {
                               Label = (string)row["Channel"],
                               Value = (string)row["Counts"]
                           };

                Labr3_live_line.DataContext = data;




            }), null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//最新数据功能
        {
            string Labr3_startTime, Labr3_endTime, Labr3_ID, Labr3_HV, Labr3_tm, Labr3_liveTime, Labr3_page1_doserate, Labr3_page1_nuclide, Labr3_datetime, Labr3_ch, Labr3_conf, Labr3_act, Labr3_unclide_doserate, Labr3_nuclide_ch, Labr3_en, Labr3_area;
            Labr3_startTime = "";
            Labr3_endTime = "";
            Labr3_ID = "";
            Labr3_HV = "";
            Labr3_tm = "";
            Labr3_liveTime = "";
            Labr3_page1_doserate = "";
            Labr3_page1_nuclide = "";
            Labr3_datetime = "";
            Labr3_ch = "";
            Labr3_conf = "";
            Labr3_act = "";
            Labr3_unclide_doserate = "";
            Labr3_nuclide_ch = "";
            Labr3_en = "";
            Labr3_area = "";
            string saratable = "tianjin."+DateTime.Now.ToString ("yyyy")+ "_sara_value";

            string constr = "server=127.0.0.1;User Id=UI_sara2;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM "+ saratable + " order by datetime desc limit 1";//更新最新数据
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();

            while (readertrace.Read())
            {
                Labr3_datetime = readertrace[1].ToString();
                Labr3_ID = readertrace[2].ToString();
                Labr3_startTime = readertrace[3].ToString();
                Labr3_endTime = readertrace[4].ToString();
                Labr3_page1_doserate = readertrace[5].ToString();
                Labr3_HV = readertrace[6].ToString();
                Labr3_tm = readertrace[7].ToString();
                Labr3_ch = readertrace[8].ToString();
                Labr3_liveTime = readertrace[9].ToString();
                Labr3_page1_nuclide = readertrace[10].ToString();
                Labr3_act = readertrace[11].ToString();
                Labr3_conf = readertrace[12].ToString();
                Labr3_unclide_doserate = readertrace[13].ToString();
                Labr3_nuclide_ch = readertrace[14].ToString();
                Labr3_en = readertrace[15].ToString();
                Labr3_area = readertrace[16].ToString();
            }
            readertrace.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                page_labr3_doserate_label.Content = Math.Round(ChangeDataToD(Labr3_page1_doserate) * 1000, 1);
                SARA_datetime_label1.Content = Labr3_datetime;
                page_labr3_date_label.Content = DateTime.Parse(Labr3_datetime).ToString("yyyy-MM-dd");
                page_labr3_time_label.Content = DateTime.Parse(Labr3_datetime).ToString("HH:mm:ss");
                page_labr3_ID_label.Content = Labr3_ID;
                page_labr3_HV_label.Content = Labr3_HV;
                page_labr3_startdate_label.Content = DateTime.Parse(Labr3_startTime).ToString("yyyy-MM-dd");
                page_labr3_starttime_label.Content = DateTime.Parse(Labr3_startTime).ToString("HH:mm:ss");
                page_labr3_tm_label.Content = Labr3_tm;
                page_labr3_enddate_label.Content = DateTime.Parse(Labr3_endTime).ToString("yyyy-MM-dd");
                page_labr3_endtime_label.Content = DateTime.Parse(Labr3_endTime).ToString("HH:mm:ss");
                page_labr3_livetime_label.Content = Labr3_liveTime;

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("核素名称");
                dt1.Columns.Add("置信度");
                dt1.Columns.Add("贡献剂量率");
                dt1.Columns.Add("发现通道");
                dt1.Columns.Add("核素能量");
                dt1.Columns.Add("峰面积");
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Channel");
                dt2.Columns.Add("Counts");

                string[] nuclide_orgin = Labr3_page1_nuclide.Split(';');
                string[] conf_orgin = Labr3_conf.Split(';');
                string[] nu_doserate_orgin = Labr3_unclide_doserate.Split(';');
                string[] nu_ch_orgin = Labr3_nuclide_ch.Split(';');
                string[] nu_Labr3_en_orgin = Labr3_en.Split(';');
                string[] nu_Labr3_area_orgin = Labr3_area.Split(';');

                int count;
                count = nuclide_orgin.Length;
                for (int a = 0; a < count - 1; a++)
                {
                    dt1.Rows.Add(nuclide_orgin[a], conf_orgin[a], Math.Round(ChangeDataToD(nu_doserate_orgin[a]) * 1000, 1), nu_ch_orgin[a], nu_Labr3_en_orgin[a], nu_Labr3_area_orgin[a]);
                }

                Labr3_nuclide_grid.DataSource = dt1.DefaultView;
                string[] ch_orgin = Labr3_ch.Split(',');
                int count_ch = ch_orgin.Length;
                for (int a = 0; a < count_ch; a++)
                {
                    dt2.Rows.Add(a, ch_orgin[a]);
                }
                var data = from row in dt2
                    .Rows.OfType<DataRow>()
                           select new DataItem()
                           {
                               Label = (string)row["Channel"],
                               Value = (string)row["Counts"]
                           };

                Labr3_live_line.DataContext = data;




            }), null);
            if(timer_labr3.Enabled ==false)
            {
                timer_labr3.Start();
            }
        }

       
        private void Labr3_nuclide_grid_Copy_CellActivated(object sender, Infragistics.Windows.DataPresenter.Events.CellActivatedEventArgs e)
        {
            string selectvalue = (Labr3_nuclide_grid_Copy.ActiveRecord as Infragistics.Windows.DataPresenter.DataRecord).Cells[0].Value.ToString();
            DateTime d1 = DateTime.Parse(selectvalue);
            string selectdatetime = d1.ToString("yyyy-MM-dd HH:mm:ss");

            string Labr3_startTime, Labr3_endTime, Labr3_ID, Labr3_HV, Labr3_tm, Labr3_liveTime, Labr3_page1_doserate, Labr3_page1_nuclide, Labr3_datetime, Labr3_ch, Labr3_conf, Labr3_act, Labr3_unclide_doserate, Labr3_nuclide_ch, Labr3_en, Labr3_area;
            Labr3_startTime = "";
            Labr3_endTime = "";
            Labr3_ID = "";
            Labr3_HV = "";
            Labr3_tm = "";
            Labr3_liveTime = "";
            Labr3_page1_doserate = "";
            Labr3_page1_nuclide = "";
            Labr3_datetime = "";
            Labr3_ch = "";
            Labr3_conf = "";
            Labr3_act = "";
            Labr3_unclide_doserate = "";
            Labr3_nuclide_ch = "";
            Labr3_en = "";
            Labr3_area = "";
            string saratable = "tianjin." + DateTime.Now.ToString("yyyy") + "_sara_value";

            string constr = "server=127.0.0.1;User Id=UI_sara2;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + saratable + " where datetime='"+ selectdatetime + "' order by datetime desc ";//更新最新数据
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();

            while (readertrace.Read())
            {
                Labr3_datetime = readertrace[1].ToString();
                Labr3_ID = readertrace[2].ToString();
                Labr3_startTime = readertrace[3].ToString();
                Labr3_endTime = readertrace[4].ToString();
                Labr3_page1_doserate = readertrace[5].ToString();
                Labr3_HV = readertrace[6].ToString();
                Labr3_tm = readertrace[7].ToString();
                Labr3_ch = readertrace[8].ToString();
                Labr3_liveTime = readertrace[9].ToString();
                Labr3_page1_nuclide = readertrace[10].ToString();
                Labr3_act = readertrace[11].ToString();
                Labr3_conf = readertrace[12].ToString();
                Labr3_unclide_doserate = readertrace[13].ToString();
                Labr3_nuclide_ch = readertrace[14].ToString();
                Labr3_en = readertrace[15].ToString();
                Labr3_area = readertrace[16].ToString();
            }
            readertrace.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                page_labr3_doserate_label.Content = Math.Round(ChangeDataToD(Labr3_page1_doserate) * 1000, 1);
                SARA_datetime_label1.Content = Labr3_datetime;
                page_labr3_date_label.Content = DateTime.Parse(Labr3_datetime).ToString("yyyy-MM-dd");
                page_labr3_time_label.Content = DateTime.Parse(Labr3_datetime).ToString("HH:mm:ss");
                page_labr3_ID_label.Content = Labr3_ID;
                page_labr3_HV_label.Content = Labr3_HV;
                page_labr3_startdate_label.Content = DateTime.Parse(Labr3_startTime).ToString("yyyy-MM-dd");
                page_labr3_starttime_label.Content = DateTime.Parse(Labr3_startTime).ToString("HH:mm:ss");
                page_labr3_tm_label.Content = Labr3_tm;
                page_labr3_enddate_label.Content = DateTime.Parse(Labr3_endTime).ToString("yyyy-MM-dd");
                page_labr3_endtime_label.Content = DateTime.Parse(Labr3_endTime).ToString("HH:mm:ss");
                page_labr3_livetime_label.Content = Labr3_liveTime;

                DataTable dt1 = new DataTable();
                dt1.Columns.Add("核素名称");
                dt1.Columns.Add("置信度");
                dt1.Columns.Add("贡献剂量率");
                dt1.Columns.Add("发现通道");
                dt1.Columns.Add("核素能量");
                dt1.Columns.Add("峰面积");
                DataTable dt2 = new DataTable();
                dt2.Columns.Add("Channel");
                dt2.Columns.Add("Counts");

                string[] nuclide_orgin = Labr3_page1_nuclide.Split(';');
                string[] conf_orgin = Labr3_conf.Split(';');
                string[] nu_doserate_orgin = Labr3_unclide_doserate.Split(';');
                string[] nu_ch_orgin = Labr3_nuclide_ch.Split(';');
                string[] nu_Labr3_en_orgin = Labr3_en.Split(';');
                string[] nu_Labr3_area_orgin = Labr3_area.Split(';');

                int count;
                count = nuclide_orgin.Length;
                for (int a = 0; a < count - 1; a++)
                {
                    dt1.Rows.Add(nuclide_orgin[a], conf_orgin[a], Math.Round(ChangeDataToD(nu_doserate_orgin[a]) * 1000, 1), nu_ch_orgin[a], nu_Labr3_en_orgin[a], nu_Labr3_area_orgin[a]);
                }

                Labr3_nuclide_grid.DataSource = dt1.DefaultView;
                string[] ch_orgin = Labr3_ch.Split(',');
                int count_ch = ch_orgin.Length;
                for (int a = 0; a < count_ch; a++)
                {
                    dt2.Rows.Add(a, ch_orgin[a]);
                }
                var data = from row in dt2
                    .Rows.OfType<DataRow>()
                           select new DataItem()
                           {
                               Label = (string)row["Channel"],
                               Value = (string)row["Counts"]
                           };

                Labr3_live_line.DataContext = data;




            }), null);

        }
    }
}