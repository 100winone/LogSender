using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
namespace NewProject
{
    public partial class Form2 : Form
    {
       

        public string strip { get; set; }
        public string strport { get; set; }
        public static string delay;
        public static bool isrun;
        public Form2()
        {
            InitializeComponent();           
            this.button2.Click += button2_Click;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            XmlDocument XD = new XmlDocument();
            XD.Load(Environment.CurrentDirectory + @"..\..\Config.xml");


            XmlNodeList config = XD.SelectNodes("/Config/IP");
            foreach (XmlNode ip in config)
            {
                XmlAttribute ipAttribute = ip.Attributes["IP"];
                if (textBox1.Text != null && textBox1.Text != "")
                    ip.InnerText = strip = textBox1.Text;

            }
            XmlNodeList config1 = XD.SelectNodes("/Config/PORT");
            foreach (XmlNode port in config1)
            {
                XmlAttribute portAttribute = port.Attributes["PORT"];
                if (textBox2.Text != null && textBox2.Text != "")
                    port.InnerText = strport = textBox2.Text;
            }

            XD.Save(Environment.CurrentDirectory + @"..\..\Config.xml");
            IP.Text = textBox1.Text;
            PORT.Text = textBox2.Text;
            if (textBox3.Text != null && textBox3.Text != "")
                label6.Text = delay = textBox3.Text;
            this.Close();
        }

        
        public void Form2_Load_1(object sender, EventArgs e)
        {
            string url = Environment.CurrentDirectory + @"..\..\Config.xml";
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(url);

                //xml문서안의 모든 속성을 가져올수 있는 XmlElement입니다. (끝까지 가져옵니다.)
                //  XmlElement KeyList = xml.DocumentElement;

                //XmlNodeList를 쓰게 되면 해당 노드를 선택합니다. 
                XmlNodeList xnList = xml.SelectNodes("Config");

                //foreach문으로 하나씩 가져와서 xnList에 추가하여줍니다. 
                foreach (XmlNode xn in xnList)
                {
                    IP.Text = xn["IP"].InnerText;
                    PORT.Text = xn["PORT"].InnerText;
                }
                label6.Text = delay;
            }
            catch (Exception ex)
            {
                MessageBox.Show("XML 문제발생 \r\n" + ex);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {        
            this.Close();            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
}
