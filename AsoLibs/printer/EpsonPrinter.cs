using System;
using System.IO.Ports;
using System.Text;

namespace AsoLibs.printer
{
    class EpsonPrinter : IPrinter
    {
        private SerialPort serialPort = null;
        private string[] ports = null;
        private bool isOpen = false;
        private string portName;
        private int width;

        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

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

        #region IPrinter 멤버

        public string[] Ports
        {
            get { return ports; }
        }

        public EpsonPrinter()
        {
            Init();
        }

        ~EpsonPrinter()
        {
            Close();
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.Write(serialPort.ReadExisting());
        }

        public int Init()
        {
            if (serialPort != null)
            {
                serialPort.Dispose();
                serialPort = null;
            }
            serialPort = new SerialPort();
            ports = SerialPort.GetPortNames();
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            serialPort.WriteBufferSize = 8;
            return 0;
        }

        public int Test()
        {
            return 0;
        }

        public int Cut()
        {
            if(serialPort != null && serialPort.IsOpen)
            {
                serialPort.WriteLine("");
                serialPort.WriteLine("");
                serialPort.Write(cut); 
            }

            return 0;
        }

        public int Open()
        {
            Open(null);
            return 0;
        }

        public int Open(string portName)
        {
            if (serialPort == null)
            {
                Init();
            }

            if (!serialPort.IsOpen)
            {
                if (ports.Length > 1)
                {
                    Array.Sort<string>(ports, new Comparison<string>((i1, i2) => i2.CompareTo(i1)));
                }

                if (portName == null)
                {
                    this.portName = ports[0];
                }else{
                    this.portName = portName;
                }

                serialPort.PortName = this.portName;
                serialPort.Open();
            }

            isOpen = serialPort.IsOpen;

            return 0;
        }

        public int Print(String data, int printMode)
        {
            serialPort.Encoding = Encoding.Default;

            if((printMode & 0x01) == 0x01)
            {
                data = paresPrintString(data);
            }

            serialPort.Write(data);
            return 0;
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

            while(result.IndexOf(barcode_start) >= 0)
            {
                int s = result.IndexOf(barcode_start);
                int e = result.IndexOf(barcode_end);
                string txt = result.Substring(s + barcode_start.Length, e - s - barcode_end.Length + 1);
                string tmp = br + center + "\x1D\x68\x40\x1D\x6B\x49" + (char)txt.Length + txt + br;

                result = result.Replace(barcode_start + txt + barcode_end, tmp);
            }

            return result;
        }

        private void LineBreak(int rownum)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                for (int i = 0; i < rownum; i++ )
                {
                    serialPort.WriteLine("");
                }
            }
        }

        public int Close()
        {
            if (serialPort != null)
            {
                serialPort.Close();
                serialPort.Dispose();
                serialPort = null;
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
        #endregion
    }
}
