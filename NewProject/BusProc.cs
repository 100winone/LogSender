using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewProject
{
    public class BusProc
    {

        public ArrayList llog = new ArrayList();
        public ArrayList time = new ArrayList();
        public string busstr;
       // public List<string> llog = new List<string>();

        TcpClient tc;
        NetworkStream ns1;
        private string strIP;

        public int nPort;
        
        

        private bool IsRun = false;
        public BusProc(string ip, int port)
        {
            strIP = ip;
            nPort = port;
          
            Connect();
        }

        private bool Connect()
        {
            bool bResult = false;
            try
            {
                tc = new TcpClient(strIP, nPort);
                bResult = tc.Connected;
                if (tc.Connected)
                {
                    ns1 = tc.GetStream();
                    Form2.isrun = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error");
            
            }

            return bResult;
        }


        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);
            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }
            return DateTime.Now;
        }

        public void ThreadStop()
        {
            Form3.tt = 1;
            Form2.isrun = false;
            tc.Close();
        }

        public void Send()
        {
            if (Form3.sel == 1)
            {
                while (Form3.tt == 0)
                {
                    int t = 0;
                    string ndate = DateTime.Now.ToString("mm:ss");
                    foreach (var item in llog)
                    {
                        try
                        {
                            if (Form2.isrun == false)
                            {
                                while (Form2.isrun == false)
                                {
                                    Delay(100);
                                }
                            }
                            else
                            {
                                if (ndate == (string)time[t])
                                {
                                    SendMsg((string)item);
                                    Form1.timelist.Add("차량 번호 : " + busstr + ", 시간 : " + (string)time[t] + " 의 log파일 출력됨");
                                    time[t] = null;
                                }
                            }
                            t++;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            else
            {
                foreach (var item in llog)
                {
                    SendMsg((string)item);
                    Delay(Convert.ToInt32(Form2.delay));
                }
            }
        }

        public static byte[] ToByteArray(String hex)
        {
            byte[] bytes = null;
            bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }
        public void ThreadStart()
        {

            Form2.isrun = true;

            Thread thread = new Thread(new ThreadStart(Send));
            thread.Start();


        }
        private void SendMsg(string msg)
        {   
                try
                {
                        if (tc.Connected == true)
                        {
                            byte[] sendbuf = ToByteArray(msg);
                            NetworkStream stream = tc.GetStream();
                            stream.Write(sendbuf, 0, sendbuf.Length);
                            // Delay(500);
                            // Delay(Convert.ToInt32(Form2.delay));

                        }
                }

                catch (Exception ex)
                {
                    tc.Close();
                    MessageBox.Show(ex.Message);
                    IsRun = true;
                }
                IsRun = false;
                //ns1.Write();

                //ht.log.Debug("");
            }
    }
}
