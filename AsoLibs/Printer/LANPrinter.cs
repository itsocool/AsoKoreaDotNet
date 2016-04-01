using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AsoLibs.Printer
{
    public class LANPrinter : IPrinter
    {
        #region Init

        private TcpClient client = null;
        private string ip;
        private int port;

        string init = "\x1B\x40";
        string left = "\x1B\x61\x00";
        string center = "\x1B\x61\x01";
        string right = "\x1B\x61\x02";
        string br = "\n";
        string u_start = "\x1B\x2D\x10";
        string u_end = "\x1B\x2D\x00";
        string b_start = "\x1B\x45\x01";
        string b_end = "\x1B\x45\x00";
        string s_start = "\x1B\x47\x01";
        string s_end = "\x1B\x47\x00";
        string dh_start = "\x1B\x21\x10";
        string dh_end = "\x1B\x21\x00";
        string reset = "\x1B\x21\x00";
        string cut = "\x1D\x56\x01";

        public LANPrinter(string ip, int printerPort, int printerWidth)
        {
            this.ip = ip;
            this.port = printerPort;
            this.Width = printerWidth;
        }

        #endregion

        #region Properties

        public string[] Ports
        {
            get { throw new NotImplementedException(); }
        }

        public int Width { get; set; }

        public bool IsOpen { get; private set; }

        public string PortName { get; set; }

        #endregion

        #region IPrinter Member

        public int Open()
        {
            return 0;
        }

        public int Init()
        {
            return 0;
        }

        public int Print(string data, int printMode = 1)
        {
            IPAddress ipaddress = IPAddress.Parse(ip);
            IPEndPoint ipep = new IPEndPoint(ipaddress, port);
            NetworkStream stream = null;

            client = new TcpClient();
            client.Connect(ipep);

            if (printMode == 1)
            {
                data = ParsePrintString(data);      // 태그를 이용한 메세지 파싱
            }

            using (stream = client.GetStream())
            {
                byte[] sendBytes = Encoding.Default.GetBytes(data);
                stream.Write(sendBytes, 0, sendBytes.Length);
            }

            client.Close();
            client = null;

            return 0;
        }

        public int Test()
        {
            return 0;
        }

        public int Cut()
        {
            return 0;
        }

        public int Close()
        {
            if (client != null)
            {
                client.Close();
                client = null;
                IsOpen = false;
            }

            return 0;
        }

        public int Beep()
        {
            return 0;
        }

        #endregion

        #region Method

        private string ParsePrintString(string src)
        {
            string result = string.Empty;

            result = src;
            // 단일 태그
            result = result.Replace("<i/>", init);
            result = result.Replace("<left/>", left);
            result = result.Replace("<center/>", center);
            result = result.Replace("<right/>", right);
            result = result.Replace("<br/>", br);
            result = result.Replace("<reset/>", reset);
            result = result.Replace("<cut/>", cut);

            // 시작태그 종료 태그
            result = result.Replace("<u>", u_start);
            result = result.Replace("</u>", u_end);

            result = result.Replace("<b>", b_start);
            result = result.Replace("</b>", b_end);

            result = result.Replace("<s>", s_start);
            result = result.Replace("</s>", s_end);

            result = result.Replace("<dh>", dh_start);
            result = result.Replace("</dh>", dh_end);

            result = ParseBarcode(result);

            return result;
        }

        private string ParseBarcode(string src)
        {
            string barcode_start = "<barcode>";
            string barcode_end = "</barcode>";

            string result = src;

            while (result.IndexOf(barcode_start, StringComparison.CurrentCulture) >= 0)
            {
                int s = result.IndexOf(barcode_start, StringComparison.CurrentCulture);
                int e = result.IndexOf(barcode_end, StringComparison.CurrentCulture);
                string txt = result.Substring(s + barcode_start.Length, e - s - barcode_end.Length + 1);
                string tmp = br + center + "\x1D\x68\x40\x1D\x6B\x49" + (char)txt.Length + txt + br;

                result = result.Replace(barcode_start + txt + barcode_end, tmp);
            }

            return result;
        }

        #endregion
    }
}
