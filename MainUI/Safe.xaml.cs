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
	public partial class Safe
	{
		public Safe()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}
        public bool IsZoombarChanging;
        System.Timers.Timer timer_safe;

        public class DataItem//气象曲线元素定义
        {
            public string Label { get; set; }
            public double Value { get; set; }
        }     
        private void weather_live_line_WindowRectChanged(object sender, Infragistics.RectChangedEventArgs e)
        {
            if (IsZoombarChanging) return;
            try
            {
                IsZoombarChanging = true;
                Range newZoombarRange = new Range
                {
                    Minimum = e.NewRect.X,
                    Maximum = e.NewRect.X + e.NewRect.Width
                };
                if (this.xmZoombar != null)
                {
                    this.xmZoombar.Range = newZoombarRange;
                }
            }
            finally
            {
                IsZoombarChanging = false;
            }
        }

        private void xmZoombar_ZoomChanged(object sender, Infragistics.Controls.ZoomChangedEventArgs e)
        {
            if (this.safe_live_line == null) return;
            if (IsZoombarChanging) return;
            try
            {
                IsZoombarChanging = true;
                Range zoombarRange = e.NewRange;
                this.safe_live_line.WindowRect = new Rect
                {
                    Y = 0,
                    Height = 1,
                    X = zoombarRange.Minimum,
                    Width = (zoombarRange.Maximum - zoombarRange.Minimum)
                };
            }
            finally
            {
                IsZoombarChanging = false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (timer_safe == null)
            {
                timer_safe = new System.Timers.Timer();
                timer_safe.Interval = 1000;
                timer_safe.Elapsed += timer_safe_Elapsed;
                timer_safe.Start();
            }
        }
        void timer_safe_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=UI_safe;password=nuclover;Database=tianjin";
            string safe_tabel = "tianjin."+DateTime.Now.ToString ("yyyy")+ "_safe_value";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM "+ safe_tabel + " order by datetime desc limit 1";//更新最新数据
            string command2 = "SELECT * FROM " + safe_tabel + " order by datetime  limit 1450";//更新24小时数据，1分钟时间间隔
            string command3 = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlCommand cmdtrace2 = new MySqlCommand(command2, con);
            MySqlCommand cmdtrace3 = new MySqlCommand(command3, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace2 = null;
            MySqlDataReader readertrace3 = null;
            con.Open();
            string safe_datetime, smoke, water, door1, door2, in_tm, in_wet;
            safe_datetime = smoke = water  = door1 = door2 = in_tm = in_wet = "";
            string SQLSafeTMP_AL, SQLSafeWet_AL;
            SQLSafeTMP_AL = SQLSafeWet_AL = "";
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("时间日期");
            dt1.Columns.Add("烟感");
            dt1.Columns.Add("水浸");            
            dt1.Columns.Add("门禁1");
            dt1.Columns.Add("门禁2");
            dt1.Columns.Add("室内温度");
            dt1.Columns.Add("室内湿度");

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("时间日期");
            dt2.Columns.Add("数值", typeof(double));

             readertrace = cmdtrace.ExecuteReader();
             while (readertrace.Read())
             {
                 safe_datetime = readertrace[1].ToString();
                 smoke = readertrace[2].ToString();
                 water = readertrace[3].ToString();                
                 door1 = readertrace[4].ToString();
                 door2 = readertrace[5].ToString();
                 in_tm = readertrace[6].ToString();
                 in_wet = readertrace[7].ToString();
             }
             readertrace.Close();
             readertrace2 = cmdtrace2.ExecuteReader();
             while (readertrace2.Read())
             {
                 dt1.Rows.Add(readertrace2[1].ToString (),readertrace2[2].ToString (),readertrace2[3].ToString (),readertrace2[4].ToString (),readertrace2[5].ToString (),Convert.ToDouble( readertrace2[6].ToString ()).ToString ("#0.0"),Convert.ToDouble(readertrace2[7].ToString ()).ToString ("#0.0"));
                 dt2.Rows.Add(readertrace2[1].ToString (),Convert.ToDouble(readertrace2[6].ToString ()).ToString("#0.0"));
             }
             readertrace2.Close();

            readertrace3 = cmdtrace3.ExecuteReader();
            while(readertrace3.Read())
            {
                SQLSafeTMP_AL = readertrace3[19].ToString();
                SQLSafeWet_AL= readertrace3[18].ToString();
            }
            readertrace3.Close();
            con.Close();
             this.Dispatcher.BeginInvoke(new Action(() =>
            {

                if(smoke== "报警")
                {
                    smoke_UI.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    smoke_UI.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                if(water=="报警")
                {
                    water_UI.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    water_UI.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
               
                if (door1== "开门")
                {
                    door1_ui.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    door1_ui.Foreground= new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                if(door2== "开门")
                {
                    door2_ui.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    door2_ui.Foreground= new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                if(Convert.ToDouble(in_tm)>=Convert.ToDouble(SQLSafeTMP_AL))
                {
                    tm_ui.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    tm_ui.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                if (Convert.ToDouble(in_wet)>=Convert.ToDouble(SQLSafeWet_AL))
                {
                    hu_ui.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色

                }
                else
                {
                    hu_ui.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                smoke_UI.Content = smoke;
                water_UI.Content = water;
               
                door1_ui.Content = door1;
                door2_ui.Content = door2;
                tm_ui.Content = Convert.ToDouble(in_tm).ToString("#0.0");
                hu_ui.Content = Convert.ToDouble(in_wet).ToString("#0.0");
                safe_datetime_label1.Content = safe_datetime;
                safe_datetime_label2.Content = safe_datetime;
               
                safe_datetime_label4.Content = safe_datetime;
                safe_datetime_label5.Content = safe_datetime;
                safe_datetime_label6.Content = safe_datetime;
                safe_datetime_label7.Content = safe_datetime;
                safe_live_grid.DataSource = dt1.DefaultView;

                var data = from row in dt2
                   .Rows.OfType<DataRow>()
                           select new DataItem()
                           {
                               Label = (string)row["时间日期"],
                               Value = (double)row["数值"]
                           };
                safe_live_line.DataContext = data;
                safe_live_line2.DataContext = data;
                if (xmZoombar != null && safe_live_line != null)
                {
                    Binding binding = new Binding
                    {
                        Source = safe_live_line,
                        Path = new PropertyPath("HorizontalZoombar.Range"),
                        Mode = BindingMode.TwoWay
                    };
                    xmZoombar.SetBinding(XamZoombar.RangeProperty, binding);
                    xmZoombar.Range = new Range { Minimum = 0.3, Maximum = 0.7 };
                }





            }), null);
             








        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string select_startdate = ups_startdate_picker.SelectedDate.ToString();
            string select_enddate = ups_enddate_picker.SelectedDate.ToString();
            string select_startdate2 = Convert.ToDateTime(select_startdate).ToString("yyyy-MM-dd");
            string select_enddate2 = Convert.ToDateTime(select_enddate).ToString("yyyy-MM-dd");
            string upstable = "tianjin." + DateTime.Now.ToString("yyyy") + "_rss131_value";

            string starttime = select_startdate2 + " " + ups_his_start_hour.Text + ":" + ups_his_start_minute.Text + ":00";
            string endtime = select_enddate2 + " " + ups_his_end_hour.Text + ":" + ups_his_end_minute.Text + ":00";

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("时间日期");
            dt1.Columns.Add("烟感");
            dt1.Columns.Add("水浸");
            dt1.Columns.Add("市电");
            dt1.Columns.Add("门禁1");
            dt1.Columns.Add("门禁2");
            dt1.Columns.Add("室内温度");
            dt1.Columns.Add("室内湿度");

            string constr = "server=127.0.0.1;User Id=UI_safe;password=nuclover;Database=tianjin";
            string safe_tabel = "tianjin." + DateTime.Now.ToString("yyyy") + "_safe_value";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + safe_tabel + " where datetime between  '" + starttime + "' and '" + endtime + "'";//更新最新数据
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                dt1.Rows.Add(readertrace[1].ToString(), readertrace[2].ToString(), readertrace[3].ToString(), readertrace[4].ToString(), readertrace[5].ToString(), readertrace[6].ToString(), Convert.ToDouble(readertrace[7].ToString()).ToString("#0.0"), Convert.ToDouble(readertrace[8].ToString()).ToString("#0.0"));

            }
            readertrace.Close();
            con.Close();
            this.Dispatcher.BeginInvoke(new Action(() =>
            {


                rs131pg_his_grid.DataSource = dt1.DefaultView;
            }), null);

        }
    }
}