using AsoLibs.POS;
using AsoLibs.VO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS.DEMO
{
    public partial class MainForm : Form
    {

        //[DllImport(@"extlibs\PosToCatReqL.dll", CharSet = CharSet.Unicode)]
        //public static extern Int32 ReqToCat(string CatIP, Int32 CatPort, string SendData, StringBuilder RecvData);

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var recvData = new StringBuilder();
                SendVO sendVo = new SendVO("NICE", "192.168.0.111", 5000, 1000, "00", 1, null, null);
                IPOS pos = new NicePOS();
                RecvVO recvVo = pos.CreditCardApprove(sendVo);
                string msg = recvVo.ReturnValue.ToString();
                MessageBox.Show(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
