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
	public partial class weather
	{
		public weather()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}
        System.Timers.Timer timer_weather;
        public bool IsZoombarChanging;

        public class DataItem//气象曲线元素定义
        {
            public string Label { get; set; }
            public double Value { get; set; }
        }     
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            if (timer_weather == null)
            {
                timer_weather = new System.Timers.Timer();
                timer_weather.Interval = 10000;
                timer_weather.Elapsed += timer_weather_Elapsed;
                timer_weather.Start();
            }
        }


        void timer_weather_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
             string constr = "server=127.0.0.1;User Id=UI_weather2;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string upstable = "tianjin." + DateTime.Now.ToString("yyyy") + "_weather_value";
            string command = "SELECT * FROM "+ upstable + " order by id desc limit 1";//更新最新数据
            string command2 = "SELECT * FROM " + upstable + " order by id  limit 1450";//更新24小时数据，1分钟时间间隔
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlCommand cmdtrace2 = new MySqlCommand(command2, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace2 = null;
            con.Open();
            string weahter_datetime,weather_tm, weather_hm, windspeed, winddirect, pressure, rainfall, ifrain ;
            weahter_datetime=weather_tm = weather_hm = windspeed = winddirect = pressure = rainfall = ifrain = "";
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("时间日期");
            dt1.Columns.Add("温度");
            dt1.Columns.Add("湿度");
            dt1.Columns.Add("风速");
            dt1.Columns.Add("风向");
            dt1.Columns.Add("大气压");          
            dt1.Columns.Add("降雨量");
            dt1.Columns.Add("感雨");

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("时间日期");
            dt2.Columns.Add("数值", typeof(double));

            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                weahter_datetime=readertrace[1].ToString();
                windspeed = readertrace[2].ToString();
                winddirect = readertrace[3].ToString();
                pressure = readertrace[4].ToString();
                weather_tm = readertrace[5].ToString();
                weather_hm = readertrace[6].ToString();
                rainfall = readertrace[7].ToString();
                ifrain = readertrace[8].ToString();
            }
            readertrace.Close();

              readertrace2 = cmdtrace2.ExecuteReader();
              while (readertrace2.Read())
              {
                  dt1.Rows.Add(readertrace2[1].ToString (),readertrace2[5].ToString (),readertrace2[6].ToString (),readertrace2[2].ToString (),readertrace2[3].ToString (),readertrace2[4].ToString (),readertrace2[7].ToString (),readertrace2[8].ToString ());
                  dt2.Rows.Add(readertrace2[1].ToString (),readertrace2[5].ToString ());
              }
              readertrace2.Close();
              
                      

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                weather_datetime_lable1.Content = weahter_datetime;
                weather_datetime_lable2.Content = weahter_datetime;
                weather_datetime_lable3.Content = weahter_datetime;
                weather_datetime_lable4.Content = weahter_datetime;
                weather_datetime_lable5.Content = weahter_datetime;
                weather_datetime_lable6.Content = weahter_datetime;
                weather_datetime_lable7.Content = weahter_datetime;
                tm.Content = weather_tm;
                hum.Content = weather_hm;
                winspeed.Content = windspeed;
                atom.Content = Math.Round(Decimal.Parse(pressure)/10,1);
                rainfall_UI.Content = rainfall;
                if(ifrain=="Yes")
                {
                    ifrain_label.Content = ifrain;
                }
                else
                {
                    ifrain_label.Content = "No";
                }

                #region 风向角转换
                string windd,windd2;
                double wind = double.Parse(winddirect);
                if (wind >= 11.26 && wind <= 33.75)
                {
                    windd = "北东北";
                    windd2 = "NNE";
                }
                else if (wind >= 33.76 && wind <= 56.25)
                {
                    windd = "东北";
                    windd2 = "NE";
                }
                else if (wind >= 56.26 && wind <= 78.75)
                {
                    windd = "东东北";
                    windd2 = "ENE";
                }
                else if (wind >= 78.76 && wind <= 101.25)
                {
                    windd = "东";
                    windd2 = "E";
                }
                else if (wind <= 101.26 && wind >= 123.75)
                {
                    windd = "东东南";
                    windd2 = "ESE";
                }
                else if (wind >= 123.76 && wind <= 146.25)
                {
                    windd = "东南";
                    windd2 = "SE";
                }
                else if (wind >= 146.26 && wind <= 168.75)
                {
                    windd = "南东南";
                    windd2 = "SSE";
                }
                else if (wind >= 168.76 && wind <= 191.25)
                {
                    windd = "南";
                    windd2 = "S";
                }
                else if (wind >= 191.26 && wind <= 213.75)
                {
                    windd = "南西南";
                    windd2 = "SSW";
                }
                else if (wind >= 213.76 && wind <= 236.25)
                {
                    windd = "西南";
                    windd2 = "SW";
                }
                else if (wind >= 236.26 && wind <= 258.75)
                {
                    windd = "西西南";
                    windd2 = "WSW";
                }
                else if (wind >= 258.76 && wind <= 281.25)
                {
                    windd = "西";
                    windd2 = "W";
                }
                else if (wind >= 281.76 && wind <= 303.75)
                {
                    windd = "西西北";
                    windd2 = "WNW";
                }
                else if (wind >= 303.76 && wind <= 326.25)
                {
                    windd = "西北";
                    windd2 = "NW";
                }
                else if (wind >= 326.26 && wind <= 348.75)
                {
                    windd = "北西北";
                    windd2 = "NNW";
                }
                else
                {
                    windd = "北";
                    windd2 = "N";
                }
                windir.Content = windd;
                windir2.Content = windd2;
                #endregion 
                weather_live_grid.DataSource = dt1.DefaultView;
                
                
                var data = from row in dt2
                     .Rows.OfType<DataRow>()
                           select new DataItem()
                           {
                               Label = (string)row["时间日期"],
                               Value = (double)row["数值"]
                           };
                weather_live_line.DataContext = data;
                weather_live_line2.DataContext = data;
                if (xmZoombar != null && weather_live_line != null)
                {
                    Binding binding = new Binding
                    {
                        Source = weather_live_line,
                        Path = new PropertyPath("HorizontalZoombar.Range"),
                        Mode = BindingMode.TwoWay
                    };
                    xmZoombar.SetBinding(XamZoombar.RangeProperty, binding);
                    xmZoombar.Range = new Range { Minimum = 0.3, Maximum = 0.7 };
                }

            }), null);
        }

        private void weather_live_line_WindowRectChanged(object sender, RectChangedEventArgs e)
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

        private void xmZoombar_ZoomChanged(object sender, ZoomChangedEventArgs e)
        {
            if (this.weather_live_line == null) return;
            if (IsZoombarChanging) return;
            try
            {
                IsZoombarChanging = true;
                Range zoombarRange = e.NewRange;
                this.weather_live_line.WindowRect = new Rect
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string select_startdate = ups_startdate_picker.SelectedDate.ToString();
            string select_enddate = ups_enddate_picker.SelectedDate.ToString();
            string select_startdate2 = Convert.ToDateTime(select_startdate).ToString("yyyy-MM-dd");
            string select_enddate2 = Convert.ToDateTime(select_enddate).ToString("yyyy-MM-dd");
            string upstable = "tianjin." + DateTime.Now.ToString("yyyy") + "_weather_value";

            string starttime = select_startdate2 + " " + ups_his_start_hour.Text + ":" + ups_his_start_minute.Text + ":00";
            string endtime = select_enddate2 + " " + ups_his_end_hour.Text + ":" + ups_his_end_minute.Text + ":00";

            string constr = "server=127.0.0.1;User Id=UI_weather3;password=nuclover;Database=tianjin";

            DataTable dt1 = new DataTable();
            dt1.Columns.Add("时间日期");
            dt1.Columns.Add("温度");
            dt1.Columns.Add("湿度");
            dt1.Columns.Add("风速");
            dt1.Columns.Add("风向");
            dt1.Columns.Add("大气压");
            dt1.Columns.Add("降雨量");
            dt1.Columns.Add("感雨");
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM "+ upstable + " where datetime between  '" + starttime + "' and '" + endtime + "'";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                dt1.Rows.Add(readertrace[1].ToString(), readertrace[5].ToString(), readertrace[6].ToString(), readertrace[2].ToString(), readertrace[3].ToString(), readertrace[4].ToString(), readertrace[7].ToString(), readertrace[8].ToString());

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