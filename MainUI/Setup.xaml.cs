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
using System.Xml;
using MySql.Data.MySqlClient;
using System.Data;

namespace MainUI
{
    /// <summary>
    /// Setup.xaml 的交互逻辑
    /// </summary>
    public partial class Setup : Page
    {
        public Setup()
        {
            InitializeComponent();
        }


        void ReadRSS131config()
        {
            //读取RSS131配置文件
            XmlDocument RS131_config_xml = new XmlDocument();
            RS131_config_xml.Load(@"C:\config\rs131_config.xml");

            //读取串口名称
            XmlNode PN = RS131_config_xml.SelectSingleNode("Config/portname");
            string RS131_serialportName = PN.InnerText;
            textBox_131portName.Text = RS131_serialportName;

            //读取串口波特率
            XmlNode BD = RS131_config_xml.SelectSingleNode("Config/buardrate");
            string serialbuardrate = BD.InnerText;
            textBox_131BR.Text = serialbuardrate;

            //读取计算平均值时间
            XmlNode TM = RS131_config_xml.SelectSingleNode("Config/timer");
            string TMstring = TM.InnerText;
            textBox_131timer.Text = TMstring;

            //读取单位类型  0=nGy/h;1=R/h；
            XmlNode unit_node = RS131_config_xml.SelectSingleNode("Config/unit");
            if (unit_node.InnerText == "0")
            {
                comboBox_rs131.SelectedIndex = 0;
            }
            else if (unit_node.InnerText == "1")
            {
                comboBox_rs131.SelectedIndex = 1;
            }


            //读取刻度因子
            XmlNode factor_node = RS131_config_xml.SelectSingleNode("Config/factor");
            textBox_131factor.Text = factor_node.InnerText;
        }
        void readRSS131ALART()
        {
            string constr = "server=127.0.0.1;User Id=UI_setup;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                textBox_131doseAL1.Text = readertrace[1].ToString();
                textBox_131doseAL2.Text = readertrace[2].ToString();
                textBox_131HVAL1.Text = readertrace[3].ToString();
                textBox_131HVAL2.Text = readertrace[4].ToString();
                textBox_131BVAL1.Text = readertrace[5].ToString();
                textBox_131BVAL2.Text = readertrace[6].ToString();
                textBox_131TMAL1.Text = readertrace[7].ToString();
                textBox_131TMAL2.Text = readertrace[8].ToString();
            }
            readertrace.Close();
            con.Close();
            }

        void ReadWeatherconfig()
        {
            XmlDocument config_xml = new XmlDocument();
            config_xml.Load(@"C:\config\weather_config.xml");

            //读取串口名称
            XmlNode PN = config_xml.SelectSingleNode("Config/portname");
            string portnmae = PN.InnerText;
            textBox_weatherportname.Text = portnmae;

            //读取采集时间
            XmlNode TM = config_xml.SelectSingleNode("Config/timer");
            string TMstring = TM.InnerText;
            textBox_weathertimer.Text = TMstring;

        }
        /*
        void readWeatherAlart()
        {
            string constr = "server=127.0.0.1;User Id=UI_setup;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                textBox_weathertmpAL1.Text = readertrace[13].ToString();
                textBox_weathertmpAL2.Text = readertrace[14].ToString();
                textBox_weatherwindAL.Text = readertrace[15].ToString();
                textBox_weatherwetAL.Text = readertrace[16].ToString();
                textBox_weatherrainfallAL.Text = readertrace[17].ToString();

            }
            readertrace.Close();
            con.Close();
        }
        */

        void readSARAconfig()
        {
            XmlDocument config_xml = new XmlDocument();
            config_xml.Load(@"C:\config\labr3_config.xml");

            //读取ip地址
            XmlNode ipadress_pn = config_xml.SelectSingleNode("Config/ip");
            string ipadress = ipadress_pn.InnerText;
            textBox_saraIP.Text = ipadress;

              //读取序列号
              XmlNode sn_pn = config_xml.SelectSingleNode("Config/sn");
            string sn = sn_pn.InnerText;
            textBox_SARASn.Text = sn;

              //读取下载文件存储目录
              XmlNode dir_pn = config_xml.SelectSingleNode("Config/dir");
            textBox_SARAPath.Text = dir_pn.InnerText;
        }

        void readSARAAlart()
        {
            string constr = "server=127.0.0.1;User Id=UI_setup;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                textBox_SARAAL1.Text = readertrace[9].ToString();
                textBox_SARAAL2.Text = readertrace[10].ToString();
            }
            readertrace.Close();
            con.Close();
        }

        void readQRJconfig()
        {
            XmlDocument config_xml = new XmlDocument();
            config_xml.Load(@"C:\config\QRJ_config.xml");

            //读取串口名称
            XmlNode PN = config_xml.SelectSingleNode("Config/portname");
            string serialportName = PN.InnerText;
            textBox_QRJPN.Text = serialportName;

            //读取信息更新时间间隔
            XmlNode TM = config_xml.SelectSingleNode("Config/timer");
            string TMstring = TM.InnerText;
            textBox_QRJtimer.Text = TMstring;
        }

        void readDianconfig()
        {
            XmlDocument config_xml = new XmlDocument();
            config_xml.Load(@"C:\config\Dian_config.xml");

            //读取串口名称
            XmlNode PN = config_xml.SelectSingleNode("Config/portname");
            string serialportName = PN.InnerText;
            textBox_DianPN.Text = serialportName;
            //读取信息更新时间间隔
            XmlNode TM = config_xml.SelectSingleNode("Config/timer");
            string TMstring = TM.InnerText;
            textBox_Diantimer .Text= TMstring;
        }


        void readSafeConfig()
        {
            XmlDocument config_xml = new XmlDocument();
            config_xml.Load(@"C:\config\safe_config.xml");
            XmlNode PN = config_xml.SelectSingleNode("Config/portname");
            string serialportName = PN.InnerText;
            textBox_safePN.Text = serialportName;

            //读取采集时间
            XmlNode TM = config_xml.SelectSingleNode("Config/timer");
            string TMstring = TM.InnerText;
            textBox_Safetimer.Text = TMstring;
        }

        void readSafeAlart()
        {
            string constr = "server=127.0.0.1;User Id=UI_setup;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                textBox_SateWetAL.Text = readertrace[18].ToString();
                textBox_SafeTMAL.Text = readertrace[19].ToString();
            }
            readertrace.Close();
            con.Close();
        }

        void readUPSconfig()
        {
            XmlDocument config_xml = new XmlDocument();
            config_xml.Load(@"C:\config\UPS_config.xml");
            XmlNode PN = config_xml.SelectSingleNode("Config/portname");
            string serialportName = PN.InnerText;
            textBox_UPSPN.Text = serialportName;
            //读取采集时间
            XmlNode TM = config_xml.SelectSingleNode("Config/timer");
            string TMstring = TM.InnerText;
            textBox_UPStimer.Text = TMstring;
        }

        void readUPSAlart()
        {

            string constr = "server=127.0.0.1;User Id=UI_setup;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM tianjin.alartvalue";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                textBox_UPSTMAL.Text = readertrace[20].ToString();
                
            }
            readertrace.Close();
            con.Close();
        }

        void readStaionINFO()
        {
            string constr = "server=127.0.0.1;User Id=UI_setup;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "SELECT * FROM tianjin.stationinfo";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            MySqlDataReader readertrace = null;
            con.Open();
            readertrace = cmdtrace.ExecuteReader();
            while (readertrace.Read())
            {
                textBox_name.Text = readertrace[1].ToString();
                textBox_adress.Text = readertrace[2].ToString();
                textBox_people.Text = readertrace[3].ToString();
                textBox_number.Text = readertrace[4].ToString();

            }
            readertrace.Close();
            con.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ReadRSS131config();
            readRSS131ALART();
            ReadWeatherconfig();
        //    readWeatherAlart();
            readSARAconfig();
            readSARAAlart();
            readQRJconfig();
            readDianconfig();
            readSafeConfig();
            readSafeAlart();
            readUPSconfig();
            readUPSAlart();
            readStaionINFO();


        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            ReadRSS131config();
        }

        private void button_Copy7_Click(object sender, RoutedEventArgs e)
        {
            readRSS131ALART();
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            ReadWeatherconfig();
        }

        private void button_Copy8_Click(object sender, RoutedEventArgs e)
        {
           // readWeatherAlart();
        }

        private void button_Copy10_Click(object sender, RoutedEventArgs e)
        {
            readSARAconfig();
        }

        private void button_Copy11_Click(object sender, RoutedEventArgs e)
        {
            readSARAAlart();
        }

        private void button_Copy13_Click(object sender, RoutedEventArgs e)
        {
            readQRJconfig();
        }

        private void button_Copy15_Click(object sender, RoutedEventArgs e)
        {
            readDianconfig();
        }

        private void button_Copy4_Click(object sender, RoutedEventArgs e)
        {
            readSafeConfig();
        }

        private void button_Copy16_Click(object sender, RoutedEventArgs e)
        {
            readSafeAlart();
        }

        private void button_Copy6_Click(object sender, RoutedEventArgs e)
        {
            readUPSconfig();
        }

        private void button_Copy18_Click(object sender, RoutedEventArgs e)
        {
            readUPSAlart();
        }

        private void button_Copy17_Click(object sender, RoutedEventArgs e)
        {
            readStaionINFO();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //保存131配置文件
            XmlDocument run_xml = new XmlDocument();
            run_xml.Load(@"C:\config\rs131_config.xml");
            XmlNode node1 = run_xml.SelectSingleNode("Config/portname");
            node1.InnerText = textBox_131portName.Text;
            XmlNode node2 = run_xml.SelectSingleNode("Config/buardrate");
            node2.InnerText = textBox_131BR.Text;
            XmlNode node3 = run_xml.SelectSingleNode("Config/timer");
            node3.InnerText = textBox_131timer.Text;
            XmlNode node4 = run_xml.SelectSingleNode("Config/unit");
            node4.InnerText = comboBox_rs131.SelectedIndex.ToString ();
            XmlNode node5 = run_xml.SelectSingleNode("Config/factor");
            node5.InnerText = textBox_131factor.Text;
            run_xml.Save(@"C:\config\rs131_config.xml");
            ReadRSS131config();

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //保存131报警记录
            string constr = "server=127.0.0.1;User Id=UI_setup;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "UPDATE `tianjin`.`alartvalue` SET `RSS131doserate_AlartLV1`='"+ textBox_131doseAL1 .Text+ "', `RSS131doserate_AlartLV2`='"+ textBox_131doseAL2.Text + "', `RSS131HV_Low_Alart`='"+ textBox_131HVAL1 .Text+ "', `RSS131HV_High_Alart`='"+ textBox_131HVAL2 .Text+ "', `RSS131BV_Low_Alart`='"+ textBox_131BVAL1.Text + "', `RSS131BV_High_Alart`='"+ textBox_131BVAL2.Text + "', `RSS131TMP_LOW_Alart`='"+ textBox_131TMAL1.Text+ "', `RSS131TMP_High_Alart`='"+ textBox_131TMAL2 .Text+ "' WHERE `id`='1'";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            con.Open();
            cmdtrace.ExecuteNonQuery();
            con.Close();
            readRSS131ALART();
        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            //保存气象站配置
            XmlDocument run_xml = new XmlDocument();
            run_xml.Load(@"C:\config\weather_config.xml");
            XmlNode node1 = run_xml.SelectSingleNode("Config/portname");
            node1.InnerText = textBox_weatherportname.Text;
            XmlNode node2 = run_xml.SelectSingleNode("Config/timer");
            node2.InnerText = textBox_weathertimer.Text;
            run_xml.Save(@"C:\config\weather_config.xml");
            ReadWeatherconfig();
        }

      

        private void button_Copy9_Click(object sender, RoutedEventArgs e)
        {
            //保存SARA配置信息

            XmlDocument run_xml = new XmlDocument();
            run_xml.Load(@"C:\config\labr3_config.xml");
            XmlNode node1 = run_xml.SelectSingleNode("Config/ip");
            node1.InnerText = textBox_saraIP.Text;
            XmlNode node2 = run_xml.SelectSingleNode("Config/sn");
            node2.InnerText = textBox_SARASn.Text;
            XmlNode node3 = run_xml.SelectSingleNode("Config/dir");
            node3.InnerText = textBox_SARAPath.Text;
            run_xml.Save(@"C:\config\labr3_config.xml");
            readSafeConfig();

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            //保存SARA报警值
            string constr = "server=127.0.0.1;User Id=UI_setup;password=nuclover;Database=tianjin";
            MySqlConnection con;
            con = new MySql.Data.MySqlClient.MySqlConnection(constr);
            string command = "UPDATE `tianjin`.`alartvalue` SET `SARAAlartV1`='"+ textBox_SARAAL1 .Text+ "', `SARAAlartV2`='"+ textBox_SARAAL2 .Text+ "' WHERE `id`='1'";
            MySqlCommand cmdtrace = new MySqlCommand(command, con);
            con.Open();
            cmdtrace.ExecuteNonQuery();
            con.Close();
            readSARAAlart();
        }

        private void button_Copy12_Click(object sender, RoutedEventArgs e)
        {
            //读取气溶胶配置
            XmlDocument run_xml = new XmlDocument();
            run_xml.Load(@"C:\config\QRJ_config.xml");
            XmlNode node1 = run_xml.SelectSingleNode("Config/portname");
            node1.InnerText = textBox_QRJPN.Text;
            XmlNode node2 = run_xml.SelectSingleNode("Config/timer");
            node2.InnerText = textBox_QRJtimer.Text;
            run_xml.Save(@"C:\config\QRJ_config.xml");
            readQRJconfig();
        }
    }
}
