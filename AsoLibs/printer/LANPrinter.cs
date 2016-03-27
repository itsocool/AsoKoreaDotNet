using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AsoLibs.Printer
{
    public class LANPrinter : IPrinter
    {
        private TcpClient client = null;
        private string ip;
        private int port;
        private int width;
        private bool isOpen;

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

        public LANPrinter()
        {
        }

        public LANPrinter(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public string[] Ports
        {
            get { throw new NotImplementedException(); }
        }

        public int Open()
        {
            return 0;
        }

        public int Init()
        {
            return 0;
        }

        public int Print(string data, int printMode)
        {
            IPAddress ipaddress = IPAddress.Parse(ip);
            IPEndPoint ipep = new IPEndPoint(ipaddress, port);
            NetworkStream writeStream = null;

            try
            {
                client = new TcpClient();
                client.Connect(ipep);

                data = paresPrintString(data);

                byte[] bytes = Encoding.Default.GetBytes(data);

                writeStream = client.GetStream();
                writeStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                writeStream.Close();
                writeStream.Dispose();
                writeStream = null;
                Close();
            }

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
                isOpen = false;
            }

            return 0;
        }

        public int Beep()
        {
            return 0;
        }

        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                return isOpen;
            }
        }

        private string paresPrintString(string src)
        {
            string result = string.Empty;

            result = src;
            result = result.Replace("<i/>", init);
            result = result.Replace("<left/>", left);
            result = result.Replace("<center/>", center);
            result = result.Replace("<right/>", right);
            result = result.Replace("<br/>", br);

            result = result.Replace("<u>", u_start);
            result = result.Replace("</u>", u_end);

            result = result.Replace("<b>", b_start);
            result = result.Replace("</b>", b_end);

            result = result.Replace("<s>", s_start);
            result = result.Replace("</s>", s_end);

            result = result.Replace("<dh>", dh_start);
            result = result.Replace("</dh>", dh_end);

            result = result.Replace("<reset/>", reset);
            result = result.Replace("<cut/>", cut);
            result = parseBarcode(result);

            return result;
        }

        private string parseBarcode(string src)
        {
            string barcode_start = "<barcode>";
            string barcode_end = "</barcode>";

            string result = src;

            while (result.IndexOf(barcode_start) >= 0)
            {
                int s = result.IndexOf(barcode_start);
                int e = result.IndexOf(barcode_end);
                string txt = result.Substring(s + barcode_start.Length, e - s - barcode_end.Length + 1);
                string tmp = br + center + "\x1D\x68\x40\x1D\x6B\x49" + (char)txt.Length + txt + br;

                result = result.Replace(barcode_start + txt + barcode_end, tmp);
            }

            return result;
        }

        public string PortName { get; set; }
    }
}
