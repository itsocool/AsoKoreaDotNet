using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AsoLibs.VO;
using AsoLibs.POS;
using AsoLibs.Printer;
using System.Security.Permissions;
using System.Threading;
using AsoLibs.config;
using System.IO;
using System.Reflection;

namespace AsoLibs
{

    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(IComEvents))]
    [Guid("2CC571CE-7B4E-4FB1-A469-0A49E857C351")]
    [ProgId("AsoLibs.AsoAXCtrl")]
    public partial class AsoAXCtrl : UserControl, IAsoActiveXControl, IObjectSafety
    {
        private ConfigVO configVO = null;
        //private IPOS pos = null;
        //private IPrinter printer = null;

        #region Initialization

        public AsoAXCtrl()
        {
            InitializeComponent();

            // For the Click event that is re-defined.
            base.Click += new EventHandler(ActiveXCtrl_Click);

            // These functions are used to handle Tab-stops for the ActiveX 
            // control (including its child controls) when the control is 
            // hosted in a container.
            this.LostFocus += new EventHandler(ActiveXCtrl_LostFocus);
            this.ControlAdded += new ControlEventHandler(ActiveXCtrl_ControlAdded);

            // Raise custom Load event
            this.OnCreateControl();
            GlobalConfig config = GlobalConfig.Instance;
            rtbConfig.Text = config.XmlDoc.ToString();
        }

        // Ensures that tabbing across the container and the .NET controls
        // works as expected
        void ActiveXCtrl_LostFocus(object sender, EventArgs e)
        {
            ActiveXCtrlHelper.HandleFocus(this);
        }

        // This event will hook up the necessary handlers
        void ActiveXCtrl_ControlAdded(object sender, ControlEventArgs e)
        {
            // Register tab handler and focus-related event handlers for 
            // the control and its child controls.
            ActiveXCtrlHelper.WireUpHandlers(e.Control, ValidationHandler);
        }

        // Ensures that the Validating and Validated events fire properly
        internal void ValidationHandler(object sender, System.EventArgs e)
        {
            if (this.ContainsFocus) return;

            this.OnLeave(e); // Raise Leave event

            if (this.CausesValidation)
            {
                CancelEventArgs validationArgs = new CancelEventArgs();
                this.OnValidating(validationArgs);

                if (validationArgs.Cancel && this.ActiveControl != null)
                    this.ActiveControl.Focus();
                else
                    this.OnValidated(e); // Raise Validated event
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_SETFOCUS = 0x7;
            const int WM_PARENTNOTIFY = 0x210;
            const int WM_DESTROY = 0x2;
            const int WM_LBUTTONDOWN = 0x201;
            const int WM_RBUTTONDOWN = 0x204;

            if (m.Msg == WM_SETFOCUS)
            {
                // Raise Enter event
                this.OnEnter(System.EventArgs.Empty);
            }
            else if (m.Msg == WM_PARENTNOTIFY && (
                m.WParam.ToInt32() == WM_LBUTTONDOWN ||
                m.WParam.ToInt32() == WM_RBUTTONDOWN))
            {
                if (!this.ContainsFocus)
                {
                    // Raise Enter event
                    this.OnEnter(System.EventArgs.Empty);
                }
            }
            else if (m.Msg == WM_DESTROY &&
                !this.IsDisposed && !this.Disposing)
            {
                // Used to ensure the cleanup of the control
                this.Dispose();
            }

            base.WndProc(ref m);
        }

        #endregion

        #region ActiveX Control Registration

        // These routines perform the additional COM registration needed by 
        // ActiveX controls
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            try
            {
                ActiveXCtrlHelper.RegasmRegisterControl(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            try
            {
                ActiveXCtrlHelper.RegasmUnregisterControl(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw; // Re-throw the exception
            }
        }

        #endregion

        #region IObjectSafety Members

        public enum ObjectSafetyOptions
        {
            INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001,
            INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002,
            INTERFACE_USES_DISPEX = 0x00000004,
            INTERFACE_USES_SECURITY_MANAGER = 0x00000008
        };

        public int GetInterfaceSafetyOptions(ref Guid riid, out int pdwSupportedOptions, out int pdwEnabledOptions)
        {
            ObjectSafetyOptions m_options = ObjectSafetyOptions.INTERFACESAFE_FOR_UNTRUSTED_CALLER | ObjectSafetyOptions.INTERFACESAFE_FOR_UNTRUSTED_DATA;
            pdwSupportedOptions = (int)m_options;
            pdwEnabledOptions = (int)m_options;
            return 0;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            return 0;
        }

        #endregion

        #region Events

        // This section shows the examples of exposing a control's events.
        // Typically, you just need to
        // 1) Declare the event as you want it.
        // 2) Raise the event in the appropriate control event.

        [ComVisible(false)]
        public delegate void ClickEventHandler();
        public new event ClickEventHandler Click = null;
        void ActiveXCtrl_Click(object sender, EventArgs e)
        {
            if (null != Click) Click(); // Raise the new Click event.
        }

        [ComVisible(false)]
        public delegate void FloatPropertyChangingEventHandler(float NewValue, ref bool Cancel);
        public event FloatPropertyChangingEventHandler FloatPropertyChanging = null;

        [ComVisible(false)]
        public delegate void TestStringChangeEventHandler(string str);
        public event TestStringChangeEventHandler TestStringChange = null;

        #endregion

        #region Properties

        //public GlobalConfig Config { get; set; }

        public new int ForeColor
        {
            get { return ActiveXCtrlHelper.GetOleColorFromColor(base.ForeColor); }
            set { base.ForeColor = ActiveXCtrlHelper.GetColorFromOleColor(value); }
        }

        public new int BackColor
        {
            get { return ActiveXCtrlHelper.GetOleColorFromColor(base.BackColor); }
            set { base.BackColor = ActiveXCtrlHelper.GetColorFromOleColor(value); }
        }

        #endregion

        #region Methods

        public string Echo(string msg)
        {
            msg = "ax echode : " + msg;
            MessageBox.Show(msg);

            return msg;
        }

        public object Test(object obj)
        {
            string msg = null;
            ConfigVO cvo = null;

            try
            {
                string testMsg = "";
                testMsg += Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                testMsg += "<br/><br/>";
                testMsg += "<b>이것은 b태그</b><br/>";
                testMsg += "<dh>이것은 dh태그</dh><br/>";
                testMsg += "<barcode>ABCD1234</barcode><br/>";
                testMsg += "<cut/><br/><br/>";

                cvo = obj as ConfigVO;
                //cvo = new ConfigVO()
                //{
                //    Desc = "가라 설정",
                //    Ip = "192.168.0.111",
                //    Port = 5000,
                //    Printer = Printers.LAN.ToString(),
                //    PrinterPort = "5000",
                //    PrinterWidth = 48,
                //    TestPrintMessage = testMsg,
                //    Van = VANs.KOVAN.ToString()
                //};

                Init(cvo);

                SendVO svo = new SendVO();
                svo.Amount = 1234;
                svo.Halbu = "00";
                svo.Ip = configVO.Ip;
                svo.Port = configVO.Port;
                svo.Gubun = ServiceCode.CREDIT_APPROVAL.ToString();
                svo.Van = configVO.Van;

                RecvVO rvo = Send(svo);

                //msg = rvo.ReturnMessage;
                //msg += "<cut/><br/><br/>";
                //Print(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }

            return msg;
        }

        public RecvVO Send(SendVO vo)
        {
            IPOS pos = GetPOS();
            RecvVO result = null;
            result = pos.Send(vo);
            return result;
        }

        public void Print(string msg, int printMode = 1)
        {
            IPrinter printer = GetPrinter();
            printer.Print(msg, printMode);
            printer.Close();
        }

        public SendVO CreateSendVO(int amount, string halbu, string gubun, string org_auth_date, string org_auth_no)
        {
            SendVO vo = new SendVO()
            {
                Van = configVO.Van,
                Ip = configVO.Ip,
                Amount = amount,
                Gubun = gubun,
                Halbu = halbu,
                AuthDate = org_auth_date,
                AuthNo = org_auth_no
            };

            return vo;
        }

        public void PrintString(string str)
        {
            
        }

        public ConfigVO GetPOSInfo()
        {
            return configVO;
        }

        public void Init(ConfigVO vo)
        {
            configVO = vo;
        }

        private IPOS GetPOS()
        {
            IPOS result = null;

            if (configVO != null)
            {
                if (VANs.KOVAN.ToString().Equals(configVO.Van))
                {
                    result = new KoVanPOS();
                }
                else if (VANs.NICE.ToString().Equals(configVO.Van))
                {
                    result = new NicePOS();
                }
            }

            return result;           
        }

        private IPrinter GetPrinter()
        {
            IPrinter result = null;

            if (configVO != null)
            {
                if (Printers.LAN.ToString().Equals(configVO.Printer))
                {
                    int printerPort = Convert.ToInt32(configVO.PrinterPort);
                    result = new LANPrinter(configVO.Ip, printerPort, configVO.PrinterWidth);
                }
                else if (Printers.SERIAL.ToString().Equals(configVO.Printer))
                {
                    result = new SerialPrinter();
                }
            }

            return result;
        }

        #endregion


        #region Event Handler

        private void AsoAXCtrl_Load(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
        }

        private void btnApproval_Click(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
