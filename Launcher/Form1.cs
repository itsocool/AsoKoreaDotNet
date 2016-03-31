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
        public XmlDocument xmlDoc;

        public Form1()
        {
            InitializeComponent();
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

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    string msg = "<br/><br/><s><br/><br/><center/>영수증 입니다</s><br/><br/><br/><br/>";
        //    msg += "<barcode>barcode1234</barcode>";
        //    msg += "<br/><br/>";
        //    msg += "<cut/>";

        //    oldAx.Print(msg, 1);
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    asoAXCtrl1.Echo("");
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    int returnValue = -1;
        //    string ip = "192.168.0.111";
        //    int port = 5000;
        //    string amount = "1004";
        //    string halbu = "00";

        //    string STX = ((char)0x02).ToString();
        //    string ETX = ((char)0x03).ToString();
        //    string FS = ((char)0x1C).ToString();

        //    try
        //    {
        //        string sendData = STX + "D1" + FS + FS + FS + halbu + FS + FS + FS + amount + FS + FS + FS + FS + FS + ETX;
        //        StringBuilder recvByte = new StringBuilder();
        //        //returnValue = NicePOS.ReqToCat(ip, port, sendData, recvByte);
        //        //IPOS pos = new KoVanPOS();
        //        IPOS pos = new NicePOS();
        //        RecvVO vo = pos.Send(null);
        //        string result = vo.ReturnValue.ToString();
        //        MessageBox.Show(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
    }
}
