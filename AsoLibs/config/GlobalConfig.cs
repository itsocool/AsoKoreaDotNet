using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AsoLibs.config
{
    public sealed class GlobalConfig
    {
        private static readonly GlobalConfig instance = new GlobalConfig();
        private GlobalConfig() { }
        private static XmlDocument xmlDoc = new XmlDocument();
        private VANs vanProvider;
        private string ip;
        private int port;
        private string desc;
        private Printers printer;
        private string printerPort;
        private int printerWidth = 0;

        public XmlDocument XmlDoc
        {
            get { return xmlDoc; }
            set { xmlDoc = value; }
        }

        public int PrinterWidth
        {
            get { return printerWidth; }
            set
            {
                printerWidth = value;
                SaveXml();
            }
        }

        public string PrinterPort
        {
            get { return printerPort; }
            set
            {
                printerPort = value;
                SaveXml();
            }
        }

        public Printers Printer
        {
            get { return printer; }
            private set { printer = value; }
        }

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private string testPrintMessage;

        public string TestPrintMessage
        {
            get { return testPrintMessage; }
            set { testPrintMessage = value; }
        }

        public VANs VanProvider
        {
            get { return vanProvider; }
            private set { vanProvider = value; }
        }

        public string Ip
        {
            get { return ip; }
        }

        public int Port
        {
            get { return port; }
        }

        private static void LoadXml()
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configXmlPath = Path.Combine(dllPath, "config.xml");
            
            xmlDoc.Load(configXmlPath);

            string van = xmlDoc.SelectSingleNode("/config/global/van").InnerText;

            instance.vanProvider = (VANs)Enum.Parse(typeof(VANs), xmlDoc.SelectSingleNode("/config/global/van").InnerText);
            instance.ip = xmlDoc.SelectSingleNode("/config/global/ip").InnerText;
            instance.port = Int32.Parse(xmlDoc.SelectSingleNode("/config/global/port").InnerText);
            instance.desc = xmlDoc.SelectSingleNode("/config/global/desc").InnerText;
            instance.testPrintMessage = xmlDoc.SelectSingleNode("/config/global/testPrintMessage").InnerText;
            instance.printer = (Printers)Enum.Parse(typeof(Printers), xmlDoc.SelectSingleNode("/config/global/printer").InnerText);
            instance.printerPort = xmlDoc.SelectSingleNode("/config/global/printerPort").InnerText;
            instance.printerWidth = Int32.Parse(xmlDoc.SelectSingleNode("/config/global/printerWidth").InnerText);
        }

        private static void SaveXml()
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configXmlPath = Path.Combine(dllPath, "config.xml");
            XmlDocument doc = new XmlDocument();

            doc.Load(configXmlPath);

            doc.SelectSingleNode("/config/global/van").InnerText = instance.vanProvider.ToString();
            doc.SelectSingleNode("/config/global/ip").InnerText = instance.ip;
            doc.SelectSingleNode("/config/global/port").InnerText = instance.port.ToString();
            doc.SelectSingleNode("/config/global/desc").InnerText = instance.desc;
            doc.SelectSingleNode("/config/global/testPrintMessage").InnerText = instance.testPrintMessage;
            doc.SelectSingleNode("/config/global/printer").InnerText = instance.printer.ToString();
            doc.SelectSingleNode("/config/global/printerPort").InnerText = instance.printerPort;
            doc.SelectSingleNode("/config/global/printerWidth").InnerText = instance.printerWidth.ToString();
        }

        public static GlobalConfig Instance
        {
            get
            {
                LoadXml();
                return instance;
            }
        }

        public void ReLoadXml()
        {
            instance.vanProvider = VANs.DEFAULT;
            instance.ip = null;
            instance.port = 0;
            LoadXml();
        }

        public void SetValues(string van, string ip, string port, int width)
        {
            instance.vanProvider = (van != null) ? (VANs)Enum.Parse(typeof(VANs), van) : instance.vanProvider;
            instance.ip = (ip == null) ? instance.ip : ip;
            instance.port = (port == null) ? instance.port : int.Parse(port);
            instance.printerWidth = width;
        }

    }

    public enum VANs 
    {
        DEFAULT,
        KOVAN,
        NICE 
    };

    public enum Printers
    {
        DEFAULT,
        SERIAL,
        IP,
    };

}
