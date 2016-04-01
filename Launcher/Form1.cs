using AsoLibs;
using AsoLibs.VO;
using AsoLibs.POS;
using System;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
using System.Xml;
using AsoLibs.config;
using System.IO;

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

        private void asoAXCtrl1_Load(object sender, EventArgs e)
        {

            string testMsg = "";
            testMsg += Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            testMsg += "<br/><br/>";
            testMsg += "<b>이것은 b태그</b><br/>";
            testMsg += "<dh>이것은 dh태그</dh><br/>";
            testMsg += "<barcode>ABCD1234</barcode><br/>";
            testMsg += "<cut/><br/><br/>";

            ConfigVO cvo = new ConfigVO()
            {
                Desc = "가라 설정",
                Ip = "192.168.0.111",
                Port = 5000,
                Printer = Printers.LAN.ToString(),
                PrinterPort = "5000",
                PrinterWidth = 48,
                TestPrintMessage = testMsg,
                Van = VANs.KOVAN.ToString()
            };

            asoAXCtrl1.Init(cvo);
        }
    }
}
