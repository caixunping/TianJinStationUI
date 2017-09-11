using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Infragistics;
using Infragistics.Controls.Charts;
using Infragistics.Controls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;

namespace MainUI
{
	public partial class RS131
	{
		public RS131()
		{
			this.InitializeComponent();

			// 在此点之下插入创建对象所需的代码。
		}
        public bool IsZoombarChanging;
        public bool IsZoombarhisChanging;
        private void xmZoombar_ZoomChanged(object sender, Infragistics.Controls.ZoomChangedEventArgs e)
        {

            if (this.rs131pg_live_line == null) return;
            if (IsZoombarChanging) return;
            try
            {
                IsZoombarChanging = true;
                Range zoombarRange = e.NewRange;
                this.rs131pg_live_line.WindowRect = new Rect
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

        private void rs131pg_live_line_WindowRectChanged(object sender, RectChangedEventArgs e)
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




      

        private void rs131pg_his_line_WindowRectChanged(object sender, RectChangedEventArgs e)
        {
            if (IsZoombarhisChanging) return;
            try
            {
                IsZoombarhisChanging = true;
                Range newZoombarRange = new Range
                {
                    Minimum = e.NewRect.X,
                    Maximum = e.NewRect.X + e.NewRect.Width
                };
                if (this.xmZoombar_his != null)
                {
                    this.xmZoombar_his.Range = newZoombarRange;
                }
            }
            finally
            {
                IsZoombarhisChanging = false;
            }
        }

        private void xmZoombar_his_ZoomChanged(object sender, ZoomChangedEventArgs e)
        {
            if (this.rs131pg_his_line == null) return;
            if (IsZoombarhisChanging) return;
            try
            {
                IsZoombarhisChanging = true;
                Range zoombarRange = e.NewRange;
                this.rs131pg_his_line.WindowRect = new Rect
                {
                    Y = 0,
                    Height = 1,
                    X = zoombarRange.Minimum,
                    Width = (zoombarRange.Maximum - zoombarRange.Minimum)
                };
            }
            finally
            {
                IsZoombarhisChanging = false;
            }
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
            DataTable dt2 = new DataTable();
            dt1.Columns.Add("时间日期");
            dt1.Columns.Add("剂量率", typeof(double));

            dt2.Columns.Add("时间日期");
            dt2.Columns.Add("剂量率(nGy/h)");
            dt2.Columns.Add("静电计高压(V)");
            dt2.Columns.Add("电池电压(V)");
            dt2.Columns.Add("仪器内部温度(℃)");

            string constr = "server=127.0.0.1;User Id=UI_rs131his;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + upstable + " where DateTime between '" + starttime + "' and '" + endtime + "'";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();

            while (readertrace.Read())
            {
                dt1.Rows.Add(readertrace[1], double.Parse(readertrace[2].ToString()) * 1000);
                dt2.Rows.Add(readertrace[1], double.Parse(readertrace[2].ToString()) * 1000, readertrace[3], readertrace[4], readertrace[5]);
            }
            readertrace.Close();
            con.Close();
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                
                var data2 = from row in dt1               
                     .Rows.OfType<DataRow>()
                       select new DataItem()
                       {
                           Label = (string)row["时间日期"],
                           Value = (double)row["剂量率"]
                       };
                rs131pg_his_line.DataContext = data2;
                rs131pg_his_line2.DataContext = data2;
                if (xmZoombar_his != null && rs131pg_his_line != null)
                {
                    Binding binding = new Binding
                    {
                        Source = rs131pg_his_line,
                        Path = new PropertyPath("HorizontalZoombar.Range"),
                        Mode = BindingMode.TwoWay
                    };
                    xmZoombar_his.SetBinding(XamZoombar.RangeProperty, binding);
                    xmZoombar_his.Range = new Range { Minimum = 0.3, Maximum = 0.7 };
                }
                rs131pg_his_grid.DataSource = dt2.DefaultView;


            }), null);

        }
        public class DataItem//电离室曲线元素定义
        {
            public string Label { get; set; }
            public double Value { get; set; }
        }
    }
}