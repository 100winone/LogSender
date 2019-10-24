using log4net;
using log4net.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Xml;
using System.Xml.XPath;


namespace NewProject
{

    public partial class Form1 : Form
    {
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ArrayList sLogList = new ArrayList();
        public static ArrayList timelist = new ArrayList();
       // bool isBreak;
        Form2 f2 = new Form2();
        Form3 f3 = new Form3();
        
        ArrayList busNum = new ArrayList();
        ArrayList uniqueNum = new ArrayList();
        
        public Dictionary<string, BusProc> dictionary = new Dictionary<string, BusProc>();
       // public Dictionary<string, BusProc> timelist = new Dictionary<string, BusProc>();
        
        
        public Form1()
        {
            InitializeComponent();
            BasicConfigurator.Configure();
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {

            TcpClient tc;
            Stream myStream = null;
            //openFileDialog1.InitialDirectory = "C:\\";
            // openFileDialog 시작위치
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {

                            // byte[] sendBuf = null;
                            string BusStr = null;  //버스 ID저장
                            string ntime = null;
                            String sLog;
                            StreamReader sr = new StreamReader(openFileDialog1.FileName);
                            try
                            {
                                tc = new TcpClient(f2.strip, Convert.ToInt32(f2.strport));
                               
                                while ((sLog = sr.ReadLine()) != null)
                                {
                                    if (!sLog.Contains("] [ReceiveData] #"))
                                        continue;

                                    string text = sLog;
                                    string[] spText = text.Split(new string[] { "] [ReceiveData] #" }, StringSplitOptions.None);
                                    string[] tText = text.Split(new string[] { "] [차량단말기]" }, StringSplitOptions.None);
                                    // listBox1.Items.Add(spText[1].Replace(" ", ""));
                                    BusStr = spText[0].Substring(43);
                                    ntime = tText[0].Substring(15, 5);
                                    //busNum.Add(spText[0].Substring(43));         //버스 번호 컷

                                    //sendBuf = ToByteArray(spText[1].Replace(" ", ""));  //로그 정보 컷



                                    if (!dictionary.ContainsKey(BusStr))
                                    {
                                        BusProc busitem = new BusProc(f2.strip, Convert.ToInt32(f2.strport));
                                        busitem.llog.Add(spText[1].Replace(" ", ""));
                                        busitem.time.Add(ntime);
                                        busitem.busstr = BusStr;
                                        dictionary.Add(BusStr, busitem);
                                        f3.nbus.Add(BusStr);
                                    }
                                    else
                                    {

                                        var busitem = dictionary[BusStr];

                                        busitem.llog.Add(spText[1].Replace(" ", ""));
                                        busitem.time.Add(ntime);
                                    }


                                    sLogList.Add(BusStr);
                                }

                                listBox1.Items.Add("로그파일이 선택되었습니다. ");
                                try
                                {
                                    listBox1.Items.Add("IP 주소 : " + f2.strip);
                                    listBox1.Items.Add("PORT 번호 : " + f2.strport);
                                    listBox1.Items.Add("주기 : " + Form2.delay);
                                    button6.Enabled = true;
                                    button3.Enabled = true;
                                }
                                catch
                                {
                                    button6.Enabled = false;
                                    button3.Enabled = false;
                                    listBox1.Items.Add("서버상태를 확인하고 로그파일을 다시 선택하세요. ");

                                }
                                listBox1.SelectedIndex = listBox1.Items.Count - 1;


                            }
                            catch (Exception)
                            {
                                MessageBox.Show("서버를 확인하세요");
                            }

                           

                        }
                    }
                }
                catch (Exception)
                {


                }
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            
            if (f3.snlog.Count != 0)
            {
                for (int z = 0; z < f3.snlog.Count; z++)
                {
                    int i = 0;
                    foreach (var item in dictionary)
                    {
                        if (i == (int)f3.snlog[z])
                            item.Value.ThreadStart();
                        i++;
                    }
                }
            }
            else
            {
                foreach(var item in dictionary)
                {
                    item.Value.ThreadStart();
                }
            }
            button2.Enabled = true;
            button6.Enabled = false;
        }
       

        private void button4_Click(object sender, EventArgs e)
        {
            f2.ShowDialog();
            //isBreak = true;
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            
            timer1.Interval = 1000;
            timer1.Start();
            string a = "5000";
            Form2.delay = a;
          
            Form2.isrun = false;
         

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
                        f2.strip = xn["IP"].InnerText;
                        f2.strport = xn["PORT"].InnerText;

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("XML 문제발생 \r\n" + ex);
                }
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lock (timelist)
            {
                    for (int i = 0; i < timelist.Count; i++)
                    {
                        if (timelist[i] != null)
                        {
                            string s = timelist[i].ToString();
                            listBox1.Items.Add(s);
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        }
                    }
                timelist.Clear();
                if (listBox1.Items.Count > 100)
                    listBox1.Items.Clear();
            }

            if (Form2.isrun == true)
            {
                picBoxDBState.Image = Properties.Resources.On1;
            }
            else
            {
                picBoxDBState.Image = Properties.Resources.Off1;   
            }


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var item in dictionary)
            {
                item.Value.ThreadStop();
            }
            Form3.tt = 1;
            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //tc.Close();
            foreach (var item in dictionary)
            {
                item.Value.ThreadStop();
            }
            // dictionary = null;
            dictionary.Clear();
            timelist.Clear();
            sLogList.Clear();
            f3.snlog.Clear();
            f3.nbus.Clear();
            button2.Enabled = false;
            button3.Enabled = false;
            button6.Enabled = false;

        }

        private void Button6_Click(object sender, EventArgs e)
        {
            f3.ShowDialog();
            //isBreak = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Form2.isrun == true)
                Form2.isrun = false;
            else
                Form2.isrun = true;
        }
    }
}

