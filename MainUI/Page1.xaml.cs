using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.ComponentModel;

namespace MainUI
{
    public partial class Page1
    {
        public Page1()
        {
            this.InitializeComponent();

            // 在此点之下插入创建对象所需的代码。

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = @"C:\config\value_tmp";
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.EnableRaisingEvents = true;

            FileSystemWatcher watcher_QRJ = new FileSystemWatcher();
            watcher_QRJ.Path = @"C:\config\QRJRun";
            watcher_QRJ.Changed += new FileSystemEventHandler(watcherQRJ_Changed);
            watcher_QRJ.EnableRaisingEvents = true;

            FileSystemWatcher watcher_Dian = new FileSystemWatcher();
            watcher_Dian.Path = @"C:\config\dianRun";
            watcher_Dian.Changed += new FileSystemEventHandler(watcherDian_Changed);
            watcher_Dian.EnableRaisingEvents = true;
        }

        public class RSS131 : INotifyPropertyChanged
        {
            private string doserate;
            private string doserate_statue;

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
            public event PropertyChangedEventHandler PropertyChanged;

        }



        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            XmlDataProvider xml_RS131 = FindResource("xml_RS131Date") as XmlDataProvider;
            XmlDataProvider xml_Weather = FindResource("xml_WeatherDate") as XmlDataProvider;          
            XmlDataProvider xml_SARA = FindResource("xml_SARADate") as XmlDataProvider;
            XmlDataProvider xml_Safe = FindResource("xml_SafeDate") as XmlDataProvider;
            XmlDataProvider xml_UPS = FindResource("xml_UPSDate") as XmlDataProvider;

            xml_RS131.Refresh();
            xml_Weather.Refresh();          
            xml_SARA.Refresh();
            xml_Safe.Refresh();
            xml_UPS.Refresh();

            #region 数据处理

            #region 读取RS131剂量率，并根据报警阈值设置颜色
            //读取剂量率报警阈值
            string constr = "server=127.0.0.1;User Id=UI_rs131;password=nuclover;Database=tianjin";
            string SQLRS131doserate_AL1 = "";
            string SQLRS131doserate_AL2 = "";
            string doesrate = "";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM tianjin.alartvalue";//查询报警阈值
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            con.Open();
            MySqlDataReader readertrace = null;
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                SQLRS131doserate_AL1 = readertrace[1].ToString();
                SQLRS131doserate_AL2 = readertrace[2].ToString();
            }
            readertrace.Close();
            con.Close();

            try
            {
                var RS131_tt = xml_RS131.Document.ChildNodes[1].ChildNodes;
                var RS131_tt2 = RS131_tt[0].ChildNodes;
                foreach (XmlNode RS131_tt3 in RS131_tt2)
                {
                    if (RS131_tt3.Name == "doserate")
                    {
                        doesrate = RS131_tt3.InnerText;

                    }
                }
            }
            catch(Exception ex)
            {

            }                  
            double RS131VL_1, RS131VL_2, doserate_value;
            if (double.TryParse(SQLRS131doserate_AL1, out RS131VL_1))
            {

            }

            if (double.TryParse(SQLRS131doserate_AL2, out RS131VL_2))
            {

            }

            if (double.TryParse(doesrate, out doserate_value))
            {

            }


            //委托更新窗体
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                RSS131 rs131 = null;
                rs131 = new RSS131();
                rs131.Doserate = (doserate_value * 1000).ToString("f2");
                textBlock_rs131_value.DataContext = rs131;
                if (doserate_value < (RS131VL_1 / 1000))
                {
                    textBlock_rs131_value.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    textBlock_rs131_statue.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                    textBlock_rs131_statue.Text = "正常";
                }
                else if (doserate_value >= (RS131VL_1 / 1000) && doserate_value < (RS131VL_2 / 1000))
                {
                    textBlock_rs131_value.Foreground = new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                    textBlock_rs131_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                    textBlock_rs131_statue.Text = "超标";
                }
                else
                {
                    textBlock_rs131_value.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    textBlock_rs131_statue.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                    textBlock_rs131_statue.Text = "超标";
                }



            }));

            #endregion

            #region 转化风向角
            string winddirect_str = "";
            double winddirect;
            string winddirect_convert = "";
            var Weather_tt = xml_Weather.Document.ChildNodes[1].ChildNodes;
            var Weather_tt2 = Weather_tt[0].ChildNodes;
            foreach (XmlNode Weather_tt3 in Weather_tt2)
            {
                if (Weather_tt3.Name == "winddirect")
                {
                    winddirect_str = Weather_tt3.InnerText;

                }
            }
            if (double.TryParse(winddirect_str, out winddirect))
            {
                #region 转化风向角
                string windd, windd2;

                if (winddirect >= 11.26 && winddirect <= 33.75)
                {
                    windd = "北东北";
                    windd2 = "NNE";
                }
                else if (winddirect >= 33.76 && winddirect <= 56.25)
                {
                    windd = "东北";
                    windd2 = "NE";
                }
                else if (winddirect >= 56.26 && winddirect <= 78.75)
                {
                    windd = "东东北";
                    windd2 = "ENE";
                }
                else if (winddirect >= 78.76 && winddirect <= 101.25)
                {
                    windd = "东";
                    windd2 = "E";
                }
                else if (winddirect <= 101.26 && winddirect >= 123.75)
                {
                    windd = "东东南";
                    windd2 = "ESE";
                }
                else if (winddirect >= 123.76 && winddirect <= 146.25)
                {
                    windd = "东南";
                    windd2 = "SE";
                }
                else if (winddirect >= 146.26 && winddirect <= 168.75)
                {
                    windd = "南东南";
                    windd2 = "SSE";
                }
                else if (winddirect >= 168.76 && winddirect <= 191.25)
                {
                    windd = "南";
                    windd2 = "S";
                }
                else if (winddirect >= 191.26 && winddirect <= 213.75)
                {
                    windd = "南西南";
                    windd2 = "SSW";
                }
                else if (winddirect >= 213.76 && winddirect <= 236.25)
                {
                    windd = "西南";
                    windd2 = "SW";
                }
                else if (winddirect >= 236.26 && winddirect <= 258.75)
                {
                    windd = "西西南";
                    windd2 = "WSW";
                }
                else if (winddirect >= 258.76 && winddirect <= 281.25)
                {
                    windd = "西";
                    windd2 = "W";
                }
                else if (winddirect >= 281.76 && winddirect <= 303.75)
                {
                    windd = "西西北";
                    windd2 = "WNW";
                }
                else if (winddirect >= 303.76 && winddirect <= 326.25)
                {
                    windd = "西北";
                    windd2 = "NW";
                }
                else if (winddirect >= 326.26 && winddirect <= 348.75)
                {
                    windd = "北西北";
                    windd2 = "NNW";
                }
                else
                {
                    windd = "北";
                    windd2 = "N";
                }

                winddirect_convert = windd + " " + windd2;
                #endregion

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    PG1_weather_winddirect_label.Content = winddirect_convert;
                }));
            }


            #endregion

            #endregion
        }

        void watcherQRJ_Changed(object sender, FileSystemEventArgs e)
        {
            XmlDataProvider xml_QRJ = FindResource("xml_QRJDate") as XmlDataProvider;
            xml_QRJ.Refresh();
          
            #region 气溶胶运行状态颜色
           
            var QRJ_tt = xml_QRJ.Document.ChildNodes[1].ChildNodes;          
            string QRJWorkStatue = "";
            int QRJWorkFlag;

         

            /*
            foreach (XmlNode QRJ_tt3 in QRJ_tt)
            {
                if (QRJ_tt3.Name == "workstatue")
                {
                    QRJWorkStatue = QRJ_tt3.InnerText;

                }
            }
            if (QRJWorkStatue == "停止")
            {
                QRJWorkFlag = 0;
            }
            else if (QRJWorkStatue == "运行")
            {
                QRJWorkFlag = 1;
            }
            else
            {
                QRJWorkFlag = 2;
            }
            */
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (PG1_QRJ_statue_label.Content.ToString() == "运行")
                {
                    QRJWorkFlag = 1;
                }
                else if (PG1_QRJ_statue_label.Content.ToString () == "停止")
                {
                    QRJWorkFlag = 0;
                }
                else
                {
                    QRJWorkFlag = 2;
                }



                if (QRJWorkFlag == 0)
                {
                    PG1_QRJ_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else if (QRJWorkFlag == 2)
                {
                    PG1_QRJ_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                }
                else if (QRJWorkFlag == 1)
                {
                    PG1_QRJ_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
            }));



            #endregion
        }

        void watcherDian_Changed(object sender, FileSystemEventArgs e)
        {
            XmlDataProvider xml_Dian = FindResource("xml_DianDate") as XmlDataProvider;
            xml_Dian.Refresh();
            #region 气碘运行状态颜色
            var Dian_tt = xml_Dian.Document.ChildNodes[1].ChildNodes;           
            string DianWorkStatue = "";
            int DianWorkFlag;
            foreach (XmlNode Dian_tt3 in Dian_tt)
            {
                if (Dian_tt3.Name == "workstatue")
                {
                    DianWorkStatue = Dian_tt3.InnerText;

                }
            }
            if (DianWorkStatue == "停止")
            {
                DianWorkFlag = 0;
            }
            else if (DianWorkStatue == "运行")
            {
                DianWorkFlag = 1;
            }
            else
            {
                DianWorkFlag = 2;
            }
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (DianWorkFlag == 0)
                {
                    PG1_Dian_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 94, 79));//红色
                }
                else if (DianWorkFlag == 2)
                {
                    PG1_Dian_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(233, 198, 79));//黄色
                }
                else if (DianWorkFlag == 1)
                {
                    PG1_Dian_statue_label.Foreground = new SolidColorBrush(Color.FromRgb(26, 187, 156));//绿色
                }
            }));
            #endregion
        }
    }
}