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
using System.ComponentModel;


namespace MainUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            //初始化绑定

        }
        //左侧菜单变色控制
        public bool left_main,left_131,left_weather,left_QRJ,left_Dian,left_labr3,left_safe,left_ups;
        public Page p1, rs131_pg, labr3_pg, weather_pg, QRJ_pg, Dian_pg, Safe_pg, UPS_pg,setup_pg;
        System.Timers.Timer timer_rs131, timer_labr3,timer_weather,timer_Dian,timer_QRJ,timer_Safe,timer_UPS;
        private IEnumerable <DataItem> data;

        RSS131 rs131 = null;

       


        #region 定义控件界面的数据源
        public class RSS131 : INotifyPropertyChanged
        {
            private string doserate;
            private string doserate_statue;
            private string datetime;
            private string hv;
            private string hv_statue;
            private string bv;
            private string bv_statue;
            private string tm;
            private string tm_statue;
            private int rain;

            public string Doserate
            {
                get { return doserate; }
                set
                {
                    doserate = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Doserate"));
                    }
                }
            }

            public string Doserate_statue
            {
                get { return doserate_statue; }
                set
                {
                    doserate_statue = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Doserate_statue"));
                    }
                }
            }


            public string Datetime
            {
                get { return datetime; }
                set
                {
                    datetime = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Datetime"));
                    }
                }
            }

            public string HV
            {
                get { return hv; }
                set
                {
                    hv = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("HV"));
                    }
                }
            }

            public string HV_statue
            {
                get { return hv_statue; }
                set
                {
                    hv_statue = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("HV_statue"));
                    }
                }
            }

            public string BV
            {
                get { return bv; }
                set
                {
                    bv = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("BV"));
                    }
                }
            }

            public string BV_statue
            {
                get { return bv_statue; }
                set
                {
                    bv_statue = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("BV_statue"));
                    }
                }
            }


            public string TM
            {
                get { return tm; }
                set
                {
                    tm = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TM"));
                    }
                }
            }

            public string TM_statue
            {
                get { return tm_statue; }
                set
                {
                    tm_statue = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TM_statue"));
                    }
                }
            }

            public int Rain
            {
                get { return rain; }
                set
                {
                    rain = value;
                    if (PropertyChanged != null)
                    {
                        this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Rain"));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

        }
              
        #endregion







        private void Window_Activated(object sender, System.EventArgs e)
        {
        	// 在此处添加事件处理程序实现。
          
			
        }
        #region 左侧菜单变色事件
        private void Label_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)//综述标签变浅色
        {
        	// 在此处添加事件处理程序实现。

            label_main.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));			
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)//综述标签变回原色
        {
            if (left_main==false)
            {
                label_main.Background = new SolidColorBrush(Color.FromArgb(255, 42, 63, 84));
            }
           
        }

        private void Label_weather_MouseMove(object sender, MouseEventArgs e)//气象站标签变浅色
        {
            label_weather.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));		
        }

        private void Label_weather_MouseLeave(object sender, MouseEventArgs e)//气象站标签变回原色
        {
            label_weather.Background = new SolidColorBrush(Color.FromArgb(255, 42, 63, 84));		
        }

      
        private void Label_131_MouseMove(object sender, MouseEventArgs e)//电离室标签变浅色
        {
            label_131.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
        }

        private void Label_131_MouseLeave(object sender, MouseEventArgs e)//电离室标签变回原色
        {
            if(left_131==false )
            {
                label_131.Background = new SolidColorBrush(Color.FromArgb(255, 42, 63, 84));
            }

            
        }

        private void Label_QRJ_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)//气溶胶标签变浅色
        {

            label_QRJ.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));

        }
        private void Label_QRJ_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)//气溶胶标签变回原色
        {
        	label_QRJ.Background = new SolidColorBrush(Color.FromArgb(255, 42, 63, 84));
        }

        private void Label_Dian_MouseMove(object sender, MouseEventArgs e)//碘采样标签变浅色
        {
            label_Dian.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
        }

        private void Label_Dian_MouseLeave(object sender, MouseEventArgs e)//碘采样标签变回原色
        {
             label_Dian.Background = new SolidColorBrush(Color.FromArgb(255, 42, 63, 84));
        
        }

        private void Label_Labr3_MouseMove(object sender, MouseEventArgs e)//LaBr3标签变浅色
        {
            label_Labr3.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
        }

        private void Label_Labr3_MouseLeave(object sender, MouseEventArgs e)//LaBr3标签变回原色
        {
            label_Labr3.Background = new SolidColorBrush(Color.FromArgb(255, 42, 63, 84));
        }

        private void Label_safe_MouseLeave(object sender, MouseEventArgs e)//安防标签变回原色
        {
            label_safe.Background = new SolidColorBrush(Color.FromArgb(255, 42, 63, 84));
        }

        private void Label_safe_MouseMove(object sender, MouseEventArgs e)//安防标签变浅色
        {
            label_safe.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
        }

      

        private void Label_UPS_MouseMove(object sender, MouseEventArgs e)//UPS标签变浅色
        {
            label_UPS.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
        }

        private void Label_UPS_MouseLeave(object sender, MouseEventArgs e)//UPS标签变回原色
        {
            label_UPS.Background = new SolidColorBrush(Color.FromArgb(255, 42, 63, 84));
        }


        #endregion

        #region 左侧菜单点击事件

        private void exit_png_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void config_tool_png_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            C1.Content = new Frame()
            {
                Content = setup_pg
            };
        }



        private void label_131_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            left_131 = true;
            left_main = left_weather = left_QRJ = left_Dian = left_labr3 = left_safe = left_ups = false;
            label_131.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
           
            C1.Content = new Frame()
            {
                Content = rs131_pg
            };

        }

        private void label_main_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            C1.Content = new Frame()
            {
                Content = p1
            };
            label_main.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
            left_main = true;
            left_131 = left_weather = left_QRJ = left_Dian = left_labr3 = left_safe = left_ups = false;
        }

        private void label_weather_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            C1.Content = new Frame()
            {
                Content = weather_pg
            };
            label_main.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
            left_weather = true;
            left_131 = left_main = left_QRJ = left_Dian = left_labr3 = left_safe = left_ups = false;
        }
        private void label_QRJ_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            C1.Content = new Frame()
            {
                Content = QRJ_pg
            };
        }
        private void label_Dian_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            C1.Content = new Frame()
            {
                Content = Dian_pg
            };
        }

        private void label_Labr3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            C1.Content = new Frame()
            {
                Content = labr3_pg
            };
        }

        private void label_safe_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            C1.Content = new Frame()
            {
                Content = Safe_pg
            };
        }

        private void label_UPS_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            C1.Content = new Frame()
            {
                Content = UPS_pg
            };
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            //所有page初始化
            p1 = new Page1();
            rs131_pg = new RS131();
            weather_pg = new weather();
            QRJ_pg = new QRJ();
            Dian_pg = new Dian();
            labr3_pg = new labr3();
            Safe_pg = new Safe();
            UPS_pg = new UPS();
            setup_pg = new Setup();
                     
            C1.Content = new Frame()
            {
                Content = p1

            };

            
            
            label_main.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
            left_main = true;
            left_131 = left_weather = left_QRJ = left_Dian = left_labr3 = left_safe = left_ups = false;
       
           
            label_main.Background = new SolidColorBrush(Color.FromArgb(255, 74, 94, 114));
            left_main = true;
            left_131 = left_weather = left_QRJ = left_Dian = left_labr3 = left_safe = left_ups = false;
            
            #region 电离室数据绑定
            /*
            //电离室剂量率绑定           
            rs131 = new RSS131();
            Binding rs131_bind = new Binding();
            rs131_bind.Source = rs131;
            rs131_bind.Path = new PropertyPath("Doserate");
            ((MainUI.Page1)(p1)).textBlock_rs131_value.SetBinding(TextBlock.TextProperty, rs131_bind);
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_doserate_value.SetBinding(TextBlock.TextProperty, rs131_bind);
            rs131.Doserate = "N/A";

            //电离室时间绑定
            Binding rs131_date_bind = new Binding();
            rs131_date_bind.Source = rs131;
            rs131_date_bind.Path = new PropertyPath("Datetime");
            ((MainUI.Page1)(p1)).textBlock_rs131_datetime.SetBinding(TextBlock.TextProperty, rs131_date_bind);
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_datetime1.SetBinding(TextBlock.TextProperty, rs131_date_bind);
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_datetime2.SetBinding(TextBlock.TextProperty, rs131_date_bind);
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_datetime3.SetBinding(TextBlock.TextProperty, rs131_date_bind);
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_datetime4.SetBinding(TextBlock.TextProperty, rs131_date_bind);
            rs131.Datetime = "0000-00-00 00:00:00";

            //电离室高压绑定
            Binding rs131_HV_bind = new Binding();
            rs131_HV_bind.Source = rs131;
            rs131_HV_bind.Path = new PropertyPath("HV");
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_HV.SetBinding(TextBlock.TextProperty, rs131_HV_bind);
            rs131.HV = "N/A";

            //电离室电池电压绑定
            Binding rs131_BV_bind = new Binding();
            rs131_BV_bind.Source = rs131;
            rs131_BV_bind.Path = new PropertyPath("BV");
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_BV.SetBinding(TextBlock.TextProperty, rs131_BV_bind);
            rs131.BV = "N/A";

            //电离室温度绑定
            Binding rs131_TMP_bind = new Binding();
            rs131_TMP_bind.Source = rs131;
            rs131_TMP_bind.Path = new PropertyPath("TM");
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_TMP.SetBinding(TextBlock.TextProperty, rs131_TMP_bind);
            rs131.TM = "N/A";

            //电离室剂量率状态绑定
            Binding rs131_doserate_statue_bind = new Binding();
            rs131_doserate_statue_bind.Source = rs131;
            rs131_doserate_statue_bind.Path = new PropertyPath("Doserate_statue");
            ((MainUI.Page1)(p1)).textBlock_rs131_statue.SetBinding(TextBlock.TextProperty, rs131_doserate_statue_bind);
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_doserate_statue.SetBinding(TextBlock.TextProperty, rs131_doserate_statue_bind);
            rs131.Doserate_statue = "N/A";

            //电离室高压状态绑定
            Binding rs131_HV_statue_bind = new Binding();
            rs131_HV_statue_bind.Source = rs131;
            rs131_HV_statue_bind.Path = new PropertyPath("HV_statue");
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_HV_statue.SetBinding(TextBlock.TextProperty, rs131_HV_statue_bind);
            rs131.HV_statue = "N/A";

            //电离室电池电压状态绑定
            Binding rs131_BV_statue_bind = new Binding();
            rs131_BV_statue_bind.Source = rs131;
            rs131_BV_statue_bind.Path = new PropertyPath("BV_statue");
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_BV_statue.SetBinding(TextBlock.TextProperty, rs131_BV_statue_bind);
            rs131.BV_statue = "N/A";

            //电离室温度状态绑定
            Binding rs131_tmp_statue_bind = new Binding();
            rs131_tmp_statue_bind.Source = rs131;
            rs131_tmp_statue_bind.Path = new PropertyPath("TM_statue");
            ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_TMP_statue.SetBinding(TextBlock.TextProperty, rs131_tmp_statue_bind);
            rs131.TM_statue = "N/A";
            */
            #endregion










            //曲线绑定



            if (((MainUI.RS131)(rs131_pg)).xmZoombar != null && ((MainUI.RS131)(rs131_pg)).rs131pg_live_line != null)
            {
                Binding binding = new Binding
                {
                    Source = ((MainUI.RS131)(rs131_pg)).rs131pg_live_line,
                    Path = new PropertyPath("HorizontalZoombar.Range"),
                    Mode = BindingMode.TwoWay
                };
                ((MainUI.RS131)(rs131_pg)).xmZoombar.SetBinding(XamZoombar.RangeProperty, binding);
                ((MainUI.RS131)(rs131_pg)).xmZoombar.Range = new Range { Minimum = 0.3, Maximum = 0.7 };
            }
              

            //启动后，激活各个仪器的timer定时器，从数据库定时获取数据
/*
            if (timer_rs131 == null)
            {
                timer_rs131 = new System.Timers.Timer();
                timer_rs131.Interval = 10000;
                timer_rs131.Elapsed += timer_rs131_Elapsed;
                timer_rs131.Start();
            }

            
            if (timer_labr3 == null)
            {
                timer_labr3 = new System.Timers.Timer();
                timer_labr3.Interval = 2000;
                timer_labr3.Elapsed += timer_labr3_Elapsed;
                timer_labr3.Start();
            }

            if(timer_weather==null)
            {
                timer_weather = new System.Timers.Timer();
                timer_weather.Interval = 2000;
                timer_weather.Elapsed += timer_weather_Elapsed;
                timer_weather.Start();
            }
            
            if(timer_Dian==null)
            {
                timer_Dian = new System.Timers.Timer();
                timer_Dian.Interval = 2000;
                timer_Dian.Elapsed += timer_Dian_Elapsed;
                timer_Dian.Start();
            }

            if (timer_QRJ == null)
            {
                timer_QRJ = new System.Timers.Timer();
                timer_QRJ.Interval = 2000;
                timer_QRJ.Elapsed += timer_QRJ_Elapsed;
                timer_QRJ.Start();
            }

            if (timer_Safe == null)
            {
                timer_Safe = new System.Timers.Timer();
                timer_Safe.Interval = 1000;
                timer_Safe.Elapsed += timer_Safe_Elapsed;
                timer_Safe.Start();
            }

            if(timer_UPS==null)
            {
                timer_UPS = new System.Timers.Timer();
                timer_UPS.Interval = 2000;
                timer_UPS.Elapsed += timer_UPS_Elapsed;
                timer_UPS.Start();
            }*/
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

        #region 读取数据的定时器
        void timer_rs131_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=UI_rs131;password=nuclover;Database=tianjin";
            string RSS131_PG1_value="";
            string RSS131_PG1_datetime = "";
            string RSS131_PG1_hv = "";
            string RSS131_PG1_tm = "";
            string RSS131_PG1_bv = "";
            string RSS131_PG1_Rain = "";
            string RSS131table = "tianjin." + DateTime.Now.ToString("yyyy") + "_rss131_value";
            string SQLRS131doserate_AL1="";
            string SQLRS131doserate_AL2 = "";
            string SQLRS131HV_lowAL = "";
            string SQLRS131HV_highAL = "";
            string SQLRS131BV_lowAL = "";
            string SQLRS131BV_highAL = "";
            string SQLRS131TMP_lowAL = "";
            string SQLRS131TMP_highAL = "";


           MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + RSS131table + " order by id desc limit 1";//更新最新数据
            string command2 = "SELECT * FROM " + RSS131table + " order by id desc limit 1500 ";
            string command3 = "SELECT * FROM tianjin.alartvalue";//查询报警阈值
            MySqlCommand cmdtrace = new MySqlCommand(command,con);
            MySqlCommand cmdtrace2 = new MySqlCommand(command2,con);
            MySqlCommand cmdtrace3= new MySqlCommand(command3, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace2 = null;
            MySqlDataReader readertrace3 = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
           
            while (readertrace.Read())
            {
                RSS131_PG1_value = (Double.Parse (readertrace[2].ToString ())*1000).ToString ();
                RSS131_PG1_datetime =Convert.ToDateTime(readertrace[1].ToString()).ToString ("yyyy-MM-dd HH:mm:ss");
                RSS131_PG1_hv = readertrace[3].ToString();
                RSS131_PG1_tm = readertrace[5].ToString();
                RSS131_PG1_bv = readertrace[4].ToString();
                RSS131_PG1_Rain= readertrace[6].ToString();

            }
            readertrace.Close();
            readertrace2 = cmdtrace2.ExecuteReader();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            dt1.Columns.Add("时间日期");
            dt1.Columns.Add("剂量率", typeof(double));

            dt2.Columns.Add("时间日期");
            dt2.Columns.Add("剂量率(nGy/h)");
            dt2.Columns.Add("静电计高压(V)");
            dt2.Columns.Add("电池电压(V)");
            dt2.Columns.Add("仪器内部温度(℃)");

            while (readertrace2.Read())
            {
                dt1.Rows.Add(readertrace2[1], double.Parse(readertrace2[2].ToString())*1000);
                dt2.Rows.Add(readertrace2[1], double.Parse(readertrace2[2].ToString()) * 1000, readertrace2[3], readertrace2[4], readertrace2[5]);

            }
            readertrace2.Close();

            readertrace3 = cmdtrace3.ExecuteReader();
            while (readertrace3.Read())
            {
                SQLRS131doserate_AL1 = readertrace3[1].ToString();
                SQLRS131doserate_AL2 = readertrace3[2].ToString();
                SQLRS131HV_lowAL = readertrace3[3].ToString();
                SQLRS131HV_highAL = readertrace3[4].ToString();
                SQLRS131BV_lowAL = readertrace3[5].ToString();
                SQLRS131BV_highAL = readertrace3[6].ToString();
                SQLRS131TMP_lowAL = readertrace3[7].ToString();
                SQLRS131TMP_highAL = readertrace3[8].ToString();

            }
            readertrace3.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
               
                rs131.Doserate = RSS131_PG1_value;   
                rs131.Datetime = RSS131_PG1_datetime.ToString ();
                rs131.HV = RSS131_PG1_hv;
                rs131.BV = RSS131_PG1_bv;
                rs131.TM = RSS131_PG1_tm;
                if(RSS131_PG1_Rain=="Yes")
                {
                    rs131.Rain = 1;
                }
                else
                {
                    rs131.Rain = 0;
                }

                #region 报警信息改变颜色
                if(Convert.ToDouble(RSS131_PG1_value)>=Convert.ToDouble(SQLRS131doserate_AL1)&& Convert.ToDouble(RSS131_PG1_value) < Convert.ToDouble(SQLRS131doserate_AL2))//剂量率触发一级报警
                {
                    ((MainUI.Page1)(p1)).textBlock_rs131_value.Foreground= new SolidColorBrush(Color.FromRgb( 233, 198, 79));//黄色
                    ((MainUI.Page1)(p1)).textBlock_rs131_statue.Foreground= new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_doserate_value.Foreground = new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_doserate_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                    rs131.Doserate_statue = "报警";
                }
                else if (Convert.ToDouble(RSS131_PG1_value) >= Convert.ToDouble(SQLRS131doserate_AL2))//剂量率触发二级报警
                {
                    ((MainUI.Page1)(p1)).textBlock_rs131_value.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.Page1)(p1)).textBlock_rs131_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_doserate_value.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_doserate_statue.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    rs131.Doserate_statue = "报警";
                }
                else if (Convert.ToDouble(RSS131_PG1_value)< Convert.ToDouble(SQLRS131doserate_AL1))//剂量率正常状态
                {
                    ((MainUI.Page1)(p1)).textBlock_rs131_value.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.Page1)(p1)).textBlock_rs131_statue.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_doserate_value.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_doserate_statue.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    rs131.Doserate_statue = "正常";
                }

                if(Convert.ToDouble (RSS131_PG1_hv)>=Convert.ToDouble(SQLRS131HV_highAL))//高压超过最高值报警
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_HV.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_HV_statue.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    rs131.HV_statue = "报警";
                }
                else if (Convert.ToDouble(RSS131_PG1_hv)<=Convert.ToDouble(SQLRS131HV_lowAL))//高压低过最低值报警
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_HV.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_HV_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    rs131.HV_statue = "报警";
                }
                else
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_HV.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_HV_statue.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    rs131.HV_statue = "正常";
                }

                if(Convert.ToDouble(RSS131_PG1_bv)>=Convert.ToDouble(SQLRS131BV_highAL))//电池电压超过最高值报警
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_BV.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_BV_statue.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    rs131.BV_statue = "报警";

                }
                else if(Convert.ToDouble(RSS131_PG1_bv)<=Convert.ToDouble(SQLRS131BV_lowAL))//电池电压低过最低值报警
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_BV.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_BV_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    rs131.BV_statue = "报警";
                }
                else
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_BV.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_BV_statue.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    rs131.BV_statue = "正常";
                }

                if(Convert.ToDouble (RSS131_PG1_tm)>=Convert.ToDouble (SQLRS131TMP_highAL))//温度超过最高温度报警
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_TMP.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_TMP_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    rs131.TM_statue = "报警";
                }
                else if(Convert.ToDouble (RSS131_PG1_tm)<=Convert.ToDouble(SQLRS131TMP_lowAL))//温度低于最低温度报警
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_TMP.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_TMP_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    rs131.TM_statue = "报警";
                }
                else
                {
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_TMP.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.RS131)(rs131_pg)).textBlock_PGRS131_TMP_statue.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    rs131.TM_statue = "正常";
                }
                #endregion


                ((MainUI.RS131)(rs131_pg)).rs131pg_live_grid.DataSource = dt2.DefaultView;

                // var data = from row in dt1
                data = from row in dt1
                       .Rows.OfType<DataRow>()
                           select new DataItem()
                           {
                               Label = (string)row["时间日期"],
                               Value = (double)row["剂量率"]
                           };
               
                ((MainUI.RS131)(rs131_pg)).rs131pg_live_line.DataContext = data;
                ((MainUI.RS131)(rs131_pg)).rs131pg_live_line2.DataContext = data;

                /*
                if (((MainUI.RS131)(rs131_pg)).xmZoombar != null && ((MainUI.RS131)(rs131_pg)).rs131pg_live_line != null)
                {
                    Binding binding = new Binding
                    {
                        Source =  ((MainUI.RS131)(rs131_pg)).rs131pg_live_line,
                        Path = new PropertyPath("HorizontalZoombar.Range"),
                        Mode = BindingMode.TwoWay
                    };
                    ((MainUI.RS131)(rs131_pg)).xmZoombar.SetBinding(XamZoombar.RangeProperty, binding);
                   ((MainUI.RS131)(rs131_pg)).xmZoombar.Range = new Range { Minimum = 0.3, Maximum = 0.7 };
                }
                */
            }), null);
        }
      

        void timer_weather_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=UI_weather;password=nuclover;Database=tianjin";
            string Weather_tabel = "tianjin." + DateTime.Now.ToString("yyyy") + "_weather_value";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + Weather_tabel + " order by datetime desc limit 1";//更新最新数据
            string command3 = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlCommand cmdtrace3 = new MySqlCommand(command3, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace3 = null;
            con.Open();
            string weather_datetime, weather_windspeed, weather_winddirect, weather_pressure, weather_tmp, weather_hm, weather_rainfall, weather_ifrain;
            weather_datetime= weather_windspeed= weather_winddirect= weather_pressure= weather_tmp= weather_hm= weather_rainfall= weather_ifrain="";
            string SQLWeatherTMPAL1, SQLWeatherTMPAL2, SQLWeatherWindspeedAL, SQLWeatherWetAL, SQLWeatherRainAL;
            SQLWeatherTMPAL1=SQLWeatherTMPAL2=SQLWeatherWindspeedAL=SQLWeatherWetAL=SQLWeatherRainAL="";

            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                weather_datetime = readertrace[1].ToString();
                weather_windspeed = readertrace[2].ToString();
                weather_winddirect = readertrace[3].ToString();
                weather_pressure = readertrace[4].ToString();
                weather_tmp = readertrace[5].ToString();
                weather_hm = readertrace[6].ToString();
                weather_rainfall = readertrace[7].ToString();
                weather_ifrain = readertrace[8].ToString();
            }
            readertrace.Close();

            readertrace3 = cmdtrace3.ExecuteReader();
            while (readertrace3.Read())
            {
                SQLWeatherTMPAL1 = readertrace3[13].ToString();//低温报警阈值
                SQLWeatherTMPAL2 = readertrace3[14].ToString();//高温报警阈值
                SQLWeatherWindspeedAL= readertrace3[15].ToString();//风速报警阈值
                SQLWeatherWetAL= readertrace3[16].ToString();//湿度报警阈值
                SQLWeatherRainAL = readertrace3[17].ToString();//降雨量报警阈值

            }
            readertrace3.Close();

            con.Close();
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if(Convert.ToDouble(weather_tmp)<=Convert.ToDouble(SQLWeatherTMPAL1)||Convert.ToDouble(weather_tmp)>=Convert.ToDouble(SQLWeatherTMPAL2))
                {
                    ((MainUI.Page1)(p1)).PG1_weather_tmp_label.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_weather_tmp_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                if(Convert.ToDouble(weather_windspeed)>=Convert.ToDouble(SQLWeatherWindspeedAL))
                {
                    ((MainUI.Page1)(p1)).PG1_weather_windspeed_label.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_weather_windspeed_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                if(Convert.ToDouble(weather_hm)>=Convert.ToDouble(SQLWeatherWetAL))
                {
                    ((MainUI.Page1)(p1)).PG1_weather_wet_label.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_weather_wet_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                if (Convert.ToDouble(weather_rainfall)>=Convert.ToDouble(SQLWeatherRainAL))
                {
                    ((MainUI.Page1)(p1)).PG1_weather_rainfall_label.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_weather_rainfall_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                #region 转化风向角
                string windd, windd2;
                double wind = double.Parse(weather_winddirect);
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
                #endregion

                ((MainUI.Page1)(p1)).PG1_weather_tmp_label.Content = weather_tmp + " ℃";
                ((MainUI.Page1)(p1)).PG1_weather_windspeed_label.Content = weather_windspeed + "m/s";
                ((MainUI.Page1)(p1)).PG1_weather_winddirect_label.Content = windd;
                ((MainUI.Page1)(p1)).PG1_weather_wet_label.Content = weather_hm + " %";
                ((MainUI.Page1)(p1)).PG1_weather_rainfall_label.Content = weather_rainfall.Replace('\r',' ') + "mm";
                ((MainUI.Page1)(p1)).PG1_weather_presure_label.Content = weather_pressure + "kPa";
                ((MainUI.Page1)(p1)).PG1_weather_datetime_label.Content = weather_datetime;
                if(weather_ifrain=="Yes")
                {
                    ((MainUI.Page1)(p1)).PG1_weather_ifrain_label.Content = "降雨";
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_weather_ifrain_label.Content = "无";
                }


            }), null);

        }

        void timer_Dian_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=UI_Dian;password=nuclover;Database=tianjin";
            string Dian_tabel = "tianjin." + DateTime.Now.ToString("yyyy") + "_dian_value";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + Dian_tabel + " order by datetime desc limit 1";//更新最新数据
            string command3 = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlCommand cmdtrace3 = new MySqlCommand(command3, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace3 = null;
            con.Open();
            string Dian_datetime, Dian_worktype, Dian_floatrate, Dian_CountFlow;
            Dian_datetime = Dian_worktype = Dian_floatrate = Dian_CountFlow = "";
            string SQLDianAL;
            SQLDianAL = "";
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                Dian_datetime = readertrace[1].ToString();
                Dian_worktype = readertrace[6].ToString();
                Dian_floatrate = readertrace[7].ToString();
                Dian_CountFlow = readertrace[9].ToString();
            }
            readertrace.Close();

            readertrace3 = cmdtrace3.ExecuteReader();
            while (readertrace3.Read())
            {
                SQLDianAL = readertrace3[11].ToString();
            }
            readertrace3.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (Dian_worktype == "运行")
                {
                    ((MainUI.Page1)(p1)).PG1_Dian_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_Dian_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }

                if (Convert.ToDouble(Dian_floatrate) < Convert.ToDouble(SQLDianAL))
                {
                    ((MainUI.Page1)(p1)).PG1_DianFlowRate_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_DianFlowRate_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                ((MainUI.Page1)(p1)).PG1_Dian_statue_label.Content = Dian_worktype;
                ((MainUI.Page1)(p1)).PG1_DianFlowRate_label.Content = Dian_floatrate + " m³/h";
                ((MainUI.Page1)(p1)).PG1_Dian_FlowCount_label.Content = Dian_CountFlow + " m³";
                ((MainUI.Page1)(p1)).PG1_Dian_datetime_label.Content = Dian_datetime;


            }), null);
        }

        void timer_QRJ_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=UI_QRJ;password=nuclover;Database=tianjin";
            string QRJ_tabel = "tianjin." + DateTime.Now.ToString("yyyy") + "_qrj_value";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + QRJ_tabel + " order by datetime desc limit 1";//更新最新数据
            string command3 = "SELECT * FROM tianjin.alartvalue";            
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlCommand cmdtrace3 = new MySqlCommand(command3, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace3 = null;
            con.Open();
            string QRJ_datetime, QRJ_worktype, QRJ_floatrate, QRJ_CountFlow;
            QRJ_datetime = QRJ_worktype = QRJ_floatrate = QRJ_CountFlow = "";
            string SQLQRJAL;
            SQLQRJAL = "";           
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                QRJ_datetime = readertrace[1].ToString();
                QRJ_worktype = readertrace[6].ToString();
                QRJ_floatrate = readertrace[7].ToString();
                QRJ_CountFlow = readertrace[9].ToString();
            }
            readertrace.Close();

            readertrace3 = cmdtrace3.ExecuteReader();
            while (readertrace3.Read())
            {
                SQLQRJAL = readertrace3[12].ToString();
            }
            readertrace3.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if(QRJ_worktype=="运行")
                {
                    ((MainUI.Page1)(p1)).PG1_QRJ_statue_label.Foreground= new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_QRJ_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }

                if(Convert.ToDouble(QRJ_floatrate)<Convert.ToDouble(SQLQRJAL))
                {
                    ((MainUI.Page1)(p1)).PG1_QRJFlowRate_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_QRJFlowRate_label.Foreground=new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                ((MainUI.Page1)(p1)).PG1_QRJ_statue_label.Content = QRJ_worktype;
                ((MainUI.Page1)(p1)).PG1_QRJFlowRate_label.Content = QRJ_floatrate + " m³/h";
                ((MainUI.Page1)(p1)).PG1_QRJ_FlowCount_label.Content = QRJ_CountFlow + " m³";
                ((MainUI.Page1)(p1)).PG1_QRJ_datetime_label.Content = QRJ_datetime;


            }), null);



        }

        void timer_Safe_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string constr = "server=127.0.0.1;User Id=user_safe;password=nuclover;Database=tianjin";
            string safe_tabel = "tianjin." + DateTime.Now.ToString("yyyy") + "_safe_value";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + safe_tabel + " order by datetime desc limit 1";//更新最新数据
            string command3 = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlCommand cmdtrace3 = new MySqlCommand(command3, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace3 = null;
            con.Open();
            string safe_datetime, smoke, water, door1, door2, in_tm, in_wet;
            safe_datetime = smoke = water  = door1 = door2 = in_tm = in_wet = "";
            string SQLSafeTMP_AL, SQLSafeWet_AL;
            SQLSafeTMP_AL = SQLSafeWet_AL = "";
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

            readertrace3 = cmdtrace3.ExecuteReader();
            while (readertrace3.Read())
            {
                SQLSafeTMP_AL = readertrace3[19].ToString();
                SQLSafeWet_AL = readertrace3[18].ToString();
            }
            readertrace3.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (smoke == "报警")
                {
                    ((MainUI.Page1)(p1)).PG1_smoke_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_smoke_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                if (water == "报警")
                {
                    ((MainUI.Page1)(p1)).PG1_water_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_water_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
            
                if (door1 == "开门")
                {
                    ((MainUI.Page1)(p1)).PG1_door1_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_door1_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                if (door2 == "开门")
                {
                    ((MainUI.Page1)(p1)).PG1_door2_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_door2_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                if (Convert.ToDouble(in_tm) >= Convert.ToDouble(SQLSafeTMP_AL))
                {
                    ((MainUI.Page1)(p1)).PG1_safetmp_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_safetmp_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                if (Convert.ToDouble(in_wet) >= Convert.ToDouble(SQLSafeWet_AL))
                {
                    ((MainUI.Page1)(p1)).PG1_safewet_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色

                }
                else
                {
                    ((MainUI.Page1)(p1)).PG1_safewet_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }

                ((MainUI.Page1)(p1)).PG1_smoke_label.Content = smoke;
                ((MainUI.Page1)(p1)).PG1_water_label.Content = water;               
                ((MainUI.Page1)(p1)).PG1_door1_label.Content = door1;
                ((MainUI.Page1)(p1)).PG1_door2_label.Content = door2;
                ((MainUI.Page1)(p1)).PG1_safetmp_label.Content = Convert.ToDouble(in_tm).ToString("#0.0")+" ℃";
                ((MainUI.Page1)(p1)).PG1_safewet_label.Content = Convert.ToDouble(in_wet).ToString("#0.0")+ " %";
                ((MainUI.Page1)(p1)).PG1_safedatetime_label.Content = safe_datetime;
              
            }), null);


        }

        void timer_UPS_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string UPS_datetime, UPS_type, UPS_RemainTime,UPS_statue;
            UPS_datetime = UPS_type = UPS_RemainTime = UPS_statue = "";
            string SQLUPS_timeAL;
            SQLUPS_timeAL = "";
            string upstable = "tianjin." + DateTime.Now.ToString("yyyy") + "_ups_value";
            string constr = "server=127.0.0.1;User Id=UI_ups;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM " + upstable + " order by datetime desc limit 1";//更新最新数据
            string command2 = "SELECT * FROM tianjin.alartvalue";//查询报警信息
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlCommand cmdtrace2 = new MySqlCommand(command2, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace2 = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                UPS_datetime = readertrace[1].ToString();
                UPS_type = readertrace[9].ToString();
                UPS_RemainTime = readertrace[10].ToString();
                UPS_statue = readertrace[9].ToString();
            }
            readertrace.Close();

            readertrace2 = cmdtrace2.ExecuteReader();
            while(readertrace2.Read())
            {
                SQLUPS_timeAL = readertrace2[20].ToString();
            }
            readertrace2.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if(UPS_type.Contains("UPS在线式"))
                {
                    ((MainUI.Page1)(p1)).page1_UPS_type_label.Content = "市电";
                }
                else if(UPS_type.Contains("UPS后备式"))
                {
                    ((MainUI.Page1)(p1)).page1_UPS_type_label.Content = "电池";
                }

                if(Convert.ToDouble(UPS_RemainTime)>Convert.ToDouble(SQLUPS_timeAL))
                {
                    ((MainUI.Page1)(p1)).page1_UPS_time_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
                else
                {
                    ((MainUI.Page1)(p1)).page1_UPS_time_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }

                ((MainUI.Page1)(p1)).page1_UPS_time_label.Content = UPS_RemainTime + " 小时";

                if(UPS_statue.Contains("市电异常")||UPS_statue.Contains("电池电压低")||UPS_statue.Contains("UPS故障"))
                {
                    ((MainUI.Page1)(p1)).page1_UPS_statue_label.Foreground= new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.Page1)(p1)).page1_UPS_statue_label.Content = "故障";
                }
                else 
                {
                    ((MainUI.Page1)(p1)).page1_UPS_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.Page1)(p1)).page1_UPS_statue_label.Content = "正常";
                }
                ((MainUI.Page1)(p1)).page1_UPS_datetime_label.Content = UPS_datetime;

            }), null);

        }



        void timer_labr3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string Labr3_page1_doserate, Labr3_page1_nuclide, Labr3_datetime; ;         
            Labr3_page1_doserate = "";
            Labr3_page1_nuclide = "";
            Labr3_datetime = "";
            string Labr3_doserateAL1, Labr3_doserateAL2;
            Labr3_doserateAL1 = Labr3_doserateAL2 = "";
            string saratable = "tianjin."+DateTime.Now.ToString ("yyyy")+ "_sara_value";

            string constr = "server=127.0.0.1;User Id=UI_sara;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM "+saratable+" order by datetime desc limit 1";//更新最新数据
            string command2 = "SELECT * FROM tianjin.alartvalue";//查询报警信息
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlCommand cmdtrace2 = new MySqlCommand(command2, con);
            MySqlDataReader readertrace = null;
            MySqlDataReader readertrace2 = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();

            while (readertrace.Read())
            {
                Labr3_page1_doserate = readertrace[5].ToString();
                Labr3_page1_nuclide = readertrace[10].ToString();
                Labr3_datetime=readertrace[1].ToString ();
               
            }
            readertrace.Close();

            readertrace2 = cmdtrace2.ExecuteReader();
            while (readertrace2.Read ())
            {
                Labr3_doserateAL1 = readertrace2[9].ToString();
                Labr3_doserateAL2= readertrace2[10].ToString();
            }
            readertrace2.Close();
            con.Close();

            this.Dispatcher.BeginInvoke(new Action(() =>
            {

                if(Convert.ToDouble (ChangeDataToD(Labr3_page1_doserate) * 1000)>= Convert.ToDouble(Labr3_doserateAL1)&& Convert.ToDouble(ChangeDataToD(Labr3_page1_doserate) * 1000) < Convert.ToDouble(Labr3_doserateAL2))//剂量率超一级报警
                {
                    ((MainUI.Page1)(p1)).page1_labr3_doserate.Foreground= new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                    ((MainUI.Page1)(p1)).page1_labr3_statue.Foreground= new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                    ((MainUI.Page1)(p1)).page1_labr3_statue.Content = "报警";
                }
                else if(Convert.ToDouble(ChangeDataToD(Labr3_page1_doserate) * 1000)>= Convert.ToDouble(Labr3_doserateAL2))//剂量率超二级报警
                {
                    ((MainUI.Page1)(p1)).page1_labr3_doserate.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.Page1)(p1)).page1_labr3_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    ((MainUI.Page1)(p1)).page1_labr3_statue.Content = "报警";
                }
                else
                {
                    ((MainUI.Page1)(p1)).page1_labr3_doserate.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.Page1)(p1)).page1_labr3_statue.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    ((MainUI.Page1)(p1)).page1_labr3_statue.Content = "正常";
                }

                ((MainUI.Page1)(p1)).page1_labr3_doserate.Content = ChangeDataToD(Labr3_page1_doserate)*1000;
                ((MainUI.Page1)(p1)).page1_labr3_nuclide.Text = Labr3_page1_nuclide;
                ((MainUI.Page1)(p1)).page1_labr3_datetime.Content = Labr3_datetime;





            }), null);
        }



        public class DataItem//电离室曲线元素定义
        {
            public string Label { get; set; }
            public double Value { get; set; }
        }     

       



        #endregion












    }
}
