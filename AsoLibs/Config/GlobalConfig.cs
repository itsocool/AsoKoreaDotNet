using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AsoLibs.config
{
    public sealed class GlobalConfig
    {
        private static readonly GlobalConfig instance = new GlobalConfig();
        private GlobalConfig()
        {
            LoadXml();
        }

        #region Properties

        public XmlDocument XmlDoc { get; set; } = new XmlDocument();

        public int PrinterWidth { get; set; } = 0;

        public string PrinterPort { get; set; }

        public Printers Printer { get; set; }

        public string Desc { get; set; }

        public string TestPrintMessage { get; set; }

        public VANs VanProvider { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        #endregion

        #region Methods

        public void LoadXml()
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configXmlPath = Path.Combine(dllPath, "config.xml");

            XmlDoc.Load(configXmlPath);

            string van = XmlDoc.SelectSingleNode("/config/global/van").InnerText;
            VanProvider = (VANs)Enum.Parse(typeof(VANs), XmlDoc.SelectSingleNode("/config/global/van").InnerText);
            Ip = XmlDoc.SelectSingleNode("/config/global/ip").InnerText;
            Port = Int32.Parse(XmlDoc.SelectSingleNode("/config/global/port").InnerText);
            Desc = XmlDoc.SelectSingleNode("/config/global/desc").InnerText;
            TestPrintMessage = XmlDoc.SelectSingleNode("/config/global/testPrintMessage").InnerText;
            Printer = (Printers)Enum.Parse(typeof(Printers), XmlDoc.SelectSingleNode("/config/global/printer").InnerText);
            PrinterPort = XmlDoc.SelectSingleNode("/config/global/printerPort").InnerText;
            PrinterWidth = Int32.Parse(XmlDoc.SelectSingleNode("/config/global/printerWidth").InnerText);
        }

        private void SaveXml()
        {
            string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string configXmlPath = Path.Combine(dllPath, "config.xml");
            XmlDocument doc = new XmlDocument();

            doc.Load(configXmlPath);

            doc.SelectSingleNode("/config/global/van").InnerText = instance.VanProvider.ToString();
            doc.SelectSingleNode("/config/global/ip").InnerText = instance.Ip;
            doc.SelectSingleNode("/config/global/port").InnerText = instance.Port.ToString();
            doc.SelectSingleNode("/config/global/desc").InnerText = instance.Desc;
            doc.SelectSingleNode("/config/global/testPrintMessage").InnerText = instance.TestPrintMessage;
            doc.SelectSingleNode("/config/global/printer").InnerText = instance.Printer.ToString();
            doc.SelectSingleNode("/config/global/printerPort").InnerText = instance.PrinterPort;
            doc.SelectSingleNode("/config/global/printerWidth").InnerText = instance.PrinterWidth.ToString();

            doc.Save(configXmlPath);
        }

        public static GlobalConfig Instance
        {
            get
            {
                return instance;
            }
        }

        public void ReLoadXml()
        {
            VanProvider = VANs.DEFAULT;
            Ip = null;
            Port = 0;
            LoadXml();
        }

        public void SetValues(string van, string ip, string port, int width)
        {
            VanProvider = (van != null) ? (VANs)Enum.Parse(typeof(VANs), van) : instance.VanProvider;
            Ip = ip;
            Port = Convert.ToInt32(port);
            PrinterWidth = width;
        } 

        #endregion

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
