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

namespace NewProject
{
    public partial class Form3 : Form
    {

        public static int sel; // combobox 인덱스 확인 변수
        public static int tt; // thread while문 제어변수
        public ArrayList nbus = new ArrayList();
        public ArrayList snlog = new ArrayList();
        Boolean checkAll = true;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            tt = 0;
            
            for(int i = 0; i < nbus.Count; i++)
            {
                checkedListBox1.Items.Add(nbus[i]);
            }
            
            comboBox1.Items.Add("기본 전송");
            comboBox1.SelectedIndex = 0;
            comboBox1.Items.Add("시간별 전송");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sel = comboBox1.SelectedIndex;
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {

            comboBox1.Items.Clear();
            checkedListBox1.Items.Clear();
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if(checkedListBox1.GetItemChecked(i))
                {
                    // checkedListBox1.SelectedItem.ToString();
                    // checkedListBox1.Items[i].ToString();
                    
                    snlog.Add(i);
                    continue;
                }
            }
            comboBox1.Items.Clear();
            checkedListBox1.Items.Clear();
            this.Close();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkAll)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, true);
                checkAll = false;
            }
            else
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, false);
                checkAll = true;
            }

        }
    }
}
