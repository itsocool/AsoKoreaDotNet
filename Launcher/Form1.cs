using AsoLibs;
using AsoLibs.VO;
using AsoLibs.POS;
using System;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
using System.Xml;

namespace Launcher
{
    public partial class Form1 : Form
    {
        public AsoActiveXControl ax = new AsoActiveXControl();
        public XmlDocument xmlDoc;

        public Form1()
        {
            InitializeComponent();
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            this.Text += " " + version.ToString();
            xmlDoc = ax.config.XmlDoc;
            treeBinding(xmlDoc, treeView);
        }

        private void treeBinding(XmlDocument xml, TreeView tree)
        {
            XmlNode node = xml.SelectSingleNode("config");
            tree.Nodes.Clear();
            ConvertXmlNodeToTreeNode(node, tree.Nodes);
            tree.Nodes[0].ExpandAll();
        }

        private void ConvertXmlNodeToTreeNode(XmlNode xmlNode, TreeNodeCollection treeNodes)
        {

            TreeNode newTreeNode = treeNodes.Add(xmlNode.Name);

            switch (xmlNode.NodeType)
            {
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.XmlDeclaration:
                    newTreeNode.Text = "<?" + xmlNode.Name + " " +
                      xmlNode.Value + "?>";
                    break;
                case XmlNodeType.Element:
                    newTreeNode.Text = "<" + xmlNode.Name + ">";
                    break;
                case XmlNodeType.Attribute:
                    newTreeNode.Text = "ATTRIBUTE: " + xmlNode.Name;
                    break;
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                    newTreeNode.Text = xmlNode.Value;
                    break;
                case XmlNodeType.Comment:
                    newTreeNode.Text = "<!--" + xmlNode.Value + "-->";
                    break;
            }

            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    ConvertXmlNodeToTreeNode(attribute, newTreeNode.Nodes);
                }
            }
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                ConvertXmlNodeToTreeNode(childNode, newTreeNode.Nodes);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder recv_data = null;
            int rtn = -1;
            //string[] ports = ax.GetPrinterPorts();
            //listBox1.DataSource = ports;

            try
            {
                CardApprovalVO vo = ax.CreditCardApprove(2000, "00", 1, null, null) as CardApprovalVO;
                recv_data = new StringBuilder();

                if (vo is CardApprovalVO)
                {
                    rtn = vo.returnValue;
                    textBox1.Text = "ReturnValue:" + vo.returnValue.ToString();
                    textBox1.Text += "\n";
                    textBox1.Text += "IsApproved:" + vo.is_confirm.ToString();
                }
                MessageBox.Show(rtn.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string msg = "<br/><br/><s><br/><br/><center/>영수증 입니다</s><br/><br/><br/><br/>";
            msg += "<barcode>barcode1234</barcode>";
            msg += "<br/><br/>";
            msg += "<cut/>";

            ax.Print(msg, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form formSub = new Form();
            formSub.Width = 800;
            formSub.Height = 400;
            formSub.Top = 100;
            formSub.Left = 100;
            formSub.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int returnValue = -1;
            string ip = "192.168.0.111";
            int port = 5000;
            string amount = "1004";
            string halbu = "00";

            string STX = ((char)0x02).ToString();
            string ETX = ((char)0x03).ToString();
            string FS = ((char)0x1C).ToString();

            try
            {
                string sendData = STX + "D1" + FS + FS + FS + halbu + FS + FS + FS + amount + FS + FS + FS + FS + FS + ETX;
                StringBuilder recvByte = new StringBuilder();
                //returnValue = NicePOS.ReqToCat(ip, port, sendData, recvByte);
                //IPOS pos = new KoVanPOS();
                IPOS pos = new NicePOS();
                RecvVO vo = pos.CreditCardApprove(null);
                string result = vo.ReturnValue.ToString();
                MessageBox.Show(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private RecvVO CreditCardApprove(SendVO sendVO)
        {
            RecvVO recvVO = null;
            int returnValue = -1;

            try
            {
                string ip = sendVO.Ip;
                int port = sendVO.Port;
                string amount = sendVO.Amount;
                string halbu = sendVO.Halbu;
                Int32 gubun = sendVO.Gubun;
                string orgAuthDate = sendVO.OrgAuthDate;
                string orgAuthNo = sendVO.OrgAuthNo;

                string STX = ((char)0x02).ToString();
                string ETX = ((char)0x03).ToString();
                string FS = ((char)0x1C).ToString();

                string sendData = STX + "D1" + FS + FS + FS + halbu + FS + FS + FS + "1004" + FS + FS + FS + FS + FS + ETX;
                StringBuilder recvByte = new StringBuilder();

                //returnValue = Form1.ReqToCat(ip, port, sendData, recvByte);
                string result = recvByte.ToString();
                MessageBox.Show(result);

                recvVO = new RecvVO((int)returnValue, recvByte.ToString());
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                MessageBox.Show(e.Message);
                if (recvVO == null)
                {
                    recvVO = new RecvVO((int)returnValue, "    N");
                }
                else if (recvVO.GetValue(1) == null)

                    recvVO.ReturnValue = -1;
                return recvVO;
            }

            return recvVO;
        }

    }
}
