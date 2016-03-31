using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using AsoLibs.config;
using AsoLibs.POS;
using AsoLibs.Printer;
using AsoLibs.VO;
using System.Threading;

namespace AsoLibs
{
    
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(IComEvents))]
    [Guid("74B59E87-303F-41BF-92E4-8CA6D44477A9")]
    [ProgId("AsoLibs.AsoPOSActivexControl")]
    public partial class AsoActiveXControl : UserControl, IAsoActiveXControl, IObjectSafety
    {
        //public GlobalConfig config = null;
        private IPOS pos;
        private IPrinter printer;

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

        #region Initialization

        public AsoActiveXControl()
        {
            InitializeComponent();

            // For the Click event that is re-defined.
            base.Click += new EventHandler(ActiveXCtrl_Click);

            // These functions are used to handle Tab-stops for the ActiveX 
            // control (including its child controls) when the control is 
            // hosted in a container.
            this.LostFocus += new EventHandler(ActiveXCtrl_LostFocus);
            //this.ControlAdded += new ControlEventHandler(ActiveXCtrl_ControlAdded);

            // Raise custom Load event
            this.OnCreateControl();
            //config = GlobalConfig.Instance;
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

        #region Properties

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

        private float fField = 0;
        private string sField = null;

        /// <summary>
        /// A custom property with both get and set accessor methods.
        /// </summary>
        public float FloatProperty
        {
            get { return this.fField; }
            set
            {
                bool cancel = false;
                // Raise the event FloatPropertyChanging
                if (null != FloatPropertyChanging)
                    FloatPropertyChanging(value, ref cancel);
                if (!cancel)
                {
                    this.fField = value;
                    //this.lbFloatProperty.Text = value.ToString();
                }
            }
        }

        public string TestString
        {
            get { return this.sField; }
            set
            {
                bool cancel = false;
                // Raise the event StringPropertyChanging
                if (null != TestStringChange)
                    MessageBox.Show("event fire");
                TestStringChange(value);
                if (!cancel)
                {
                    this.sField = value;
                }
            }
        }

        public string Van { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string PrinterMode { get; set; }
        public int PrinterWidth { get; set; }

        public GlobalConfig Config
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
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

        #region Methods
        
        public string SetPrintPort(string portName)
        {
            return printer.PortName;
        }

        public string[] GetPrinterPorts()
        {
            return printer.Ports;
        }

        public string Echo(string msg)
        {
            try
            {
                msg = " AX echoed : " + msg;
                MessageBox.Show(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return msg;
        }

        public object Test(object obj)
        {
            CardVO vo = new CardVO();
            //RecvVO result = new RecvVO(0, null);

            try
            {
                StringBuilder orgAuthDate = new StringBuilder();
                StringBuilder orgAuthNo = new StringBuilder();
                StringBuilder recvData = new StringBuilder();

                StringBuilder rtn = new StringBuilder();

                MessageBox.Show(rtn.ToString());
                //result = CreditCardApprove(2000, "01", 1, "", "") as RecvVO;
                //vo.billNo = "가라번호";
                //result.rawData = "rawData";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return null;
        }

        //public CardApprovalVO CreditCardApprove(int amount, string halbu, int gubun, string org_auth_date, string org_auth_no)
        //{
        //    SendVO sendVO = null;
        //    RecvVO recvVO = null;
        //    CardApprovalVO result = new CardApprovalVO();

        //    try
        //    {
        //        string vanProvider = config.VanProvider.ToString();
        //        sendVO = CreateSendVO(amount, halbu, gubun, org_auth_date, org_auth_no);
        //        pos = POSManager.GetPOS(vanProvider);
        //        recvVO = pos.Send(sendVO);

        //        if (recvVO == null)
        //        {
        //            MessageBox.Show("승인결과 없음");
        //        }
        //        else
        //        {
        //            //result.ReturnValue = recvVO.ReturnValue;

        //            //result.Amount = Convert.ToString(sendVO.Amount);
        //            //result.Org_auth_date = DateTime.Now.ToString("yyMMdd");
        //            //result.Gubun = sendVO.Gubun;
        //            //result.Halbu = sendVO.Halbu;

        //            //result.confirmMSG = recvVO.GetValue(1);
        //            //result.is_confirm = (result.confirmMSG != null && result.confirmMSG.Equals("O")) ? true : false;
        //            //result.cardNumber = recvVO.GetValue(2);
        //            //result.Tex = recvVO.GetValue(4);
        //            //result.Org_auth_no = recvVO.GetValue(5);
        //            //result.PosMemberNo = recvVO.GetValue(6);
        //            //result.CardIssueCompanyCode = recvVO.GetValue(7);
        //            //result.CardIssueCompanyName = recvVO.GetValue(8);
        //            //result.BuyCompanyCode = recvVO.GetValue(9);
        //            //result.BuyCompanyName = recvVO.GetValue(10);

        //            ////MessageBox.Show(gubun.ToString() + " : " + recvVO.ReturnValue.ToString());

        //            //if (gubun == 2 && recvVO.ReturnValue == 0 && recvVO.GetValue(1) == "O")
        //            //{
        //            //    result.is_cancel = true;
        //            //}

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }

        //    return result;
        //}

        //object IAsoActiveXControl.CreditCardApprove(int amount, string halbu, int gubun, string org_auth_date, string org_auth_no)
        //{
        //    throw new NotImplementedException();
        //}

        public RecvVO Send(SendVO vo)
        {
            RecvVO result = null;
            return result;
        }

        public void Print(string msg, int printMode)
        {
            //string printerType = printer.ToString();

            //if (printerType.Equals("EPSON"))
            //{
            //    printer = new SerialPrinter();
            //}
            //else
            //{
            //    printer = new LANPrinter(IP, Port);
            //}

            printer.Init();

            if (printer.IsOpen != true)
            {
                printer.Open();
            }

            printer.Width = PrinterWidth;
            printer.Print(msg, printMode);

            if (printMode == 2)
            {
                Thread.Sleep(3000);
            }

            printer.Close();
            return;
        }

        public SendVO CreateSendVO(int amount, string halbu, string gubun, string org_auth_date, string org_auth_no)
        {
            SendVO vo = new SendVO()
            {
                Van = Van
                , Ip = IP
                , Amount = amount
                , Gubun = gubun
                , Halbu = halbu
                , AuthDate = org_auth_date
                , AuthNo = org_auth_no
            };

            return vo;
        }

        public void PrintString(string str)
        {
            throw new NotImplementedException();
        }

        public ConfigVO GetPOSInfo()
        {
            ConfigVO result = new ConfigVO()
            {
                Desc = ""
                ,Ip = IP
                ,Port = Port
                ,Printer = ""
                ,PrinterPort = ""
                ,PrinterWidth = PrinterWidth
                ,TestPrintMessage = ""
                ,Van = Van
            };

            return result;
        }

        public void Init(ConfigVO vo)
        {
            if(vo != null)
            {
                //config.Desc = vo.Desc;
                //config.Ip = vo.Ip;
                //config.Port = vo.Port;
                //config.Printer = (Printers)Enum.Parse(typeof(Printers), vo.Printer);
                //config.PrinterPort = vo.PrinterPort;
                //config.PrinterWidth = vo.PrinterWidth;
                //config.TestPrintMessage = vo.TestPrintMessage;
                //config.Van = (VANs)Enum.Parse(typeof(VANs), vo.Van);
            }
        }

        #endregion

    }

}
