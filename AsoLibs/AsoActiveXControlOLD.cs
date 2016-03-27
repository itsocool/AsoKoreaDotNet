using AsoLibs.config;
using AsoLibs.POS;
using AsoLibs.printer;
using AsoLibs.VO;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace AsoLibs
{
    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(IComEvents))]
    [Guid("74B59E87-303F-41BF-92E4-8CA6D44477A9")]
    [ProgId("AsoLibs.AsoPOSActivexControl")]
    public class AsoActiveXControlOLD : UserControl, IAsoActiveXControl, IObjectSafety
    {
        public GlobalConfig config = GlobalConfig.Instance;
        private IPOS pos;
        private IPrinter printer;

        public AsoActiveXControlOLD()
        {

        }

        private SendVO CreateSendVO(int amount, string halbu, int gubun, string org_auth_date, string org_auth_no)
        {
            SendVO vo = new SendVO(config.VanProvider.ToString(), config.Ip, config.Port, amount, halbu, gubun, org_auth_date, org_auth_no);
            return vo;
        }

        #region IAsoActiveXControl Members

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
                msg = config.VanProvider + " AX echoed : " + msg;
                MessageBox.Show(msg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return  msg;
        }

        public object Test(object obj)
        {
            CardVO vo = new CardVO();
            RecvVO result = new RecvVO(0, null);

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

            return result;
        }

        public object CreditCardApprove(int amount, string halbu, int gubun, string org_auth_date, string org_auth_no)
        {
            SendVO sendVO = null;
            RecvVO recvVO = null;
            CardApprovalVO result = new CardApprovalVO();

            try
            {
                string vanProvider = config.VanProvider.ToString();
                sendVO = new SendVO(vanProvider, config.Ip, config.Port, amount, halbu, gubun, org_auth_date, org_auth_no);
                pos = POSManager.GetPOS(vanProvider);
                recvVO = pos.CreditCardApprove(sendVO);

                if (recvVO == null)
                {
                    MessageBox.Show("승인결과 없음");
                }else
                {
                    result.returnValue = recvVO.ReturnValue;

                    result.amount = sendVO.Amount;
                    result.org_auth_date = DateTime.Now.ToString("yyMMdd");
                    result.gubun = sendVO.Gubun;
                    result.halbu = sendVO.Halbu;

                    result.confirmMSG = recvVO.GetValue(1);
                    result.is_confirm = (result.confirmMSG != null && result.confirmMSG.Equals("O")) ? true : false;
                    result.cardNumber = recvVO.GetValue(2);
                    result.tex = recvVO.GetValue(4);
                    result.org_auth_no = recvVO.GetValue(5);
                    result.posMemberNo = recvVO.GetValue(6);
                    result.cardIssueCompanyCode = recvVO.GetValue(7);
                    result.cardIssueCompanyName = recvVO.GetValue(8);
                    result.buyCompanyCode = recvVO.GetValue(9);
                    result.buyCompanyName = recvVO.GetValue(10);

                    //MessageBox.Show(gubun.ToString() + " : " + recvVO.ReturnValue.ToString());

                    if (gubun == 2 && recvVO.ReturnValue == 0 && recvVO.GetValue(1) == "O")
                    {
                        result.is_cancel = true;
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return result;
        }

        public void Print(string msg, int printMode)
        {
            //MessageBox.Show(msg);
            string printerType = config.Printer.ToString();

            if (printerType.Equals("EPSON"))
            {
                printer = new EpsonPrinter();
            }
            else
            {
                printer = new LANPrinter(config.Ip, config.Port);
            }

            printer.Init();

            if(printer.IsOpen != true)
            {
                printer.Open();
            }

            printer.Width = config.PrinterWidth;
            printer.Print(msg, printMode);

            if (printMode == 2)
            {
                Thread.Sleep(3000);
            }
            
            printer.Close();
            return;
        }

        public void ConfigPrinter(string portName, int PrinterWidth)
        {
            config.PrinterPort = portName;
            config.PrinterWidth = PrinterWidth;
        }

        public object GetPrinterConfig()
        {
            ConfigVO result = new ConfigVO();
            result.posIp = config.Ip;
            result.posPort = config.Port;
            result.posPrinter = config.Printer.ToString();
            result.posPrinterWidth = config.PrinterWidth;
            result.posPrinterPort = config.PrinterPort;
            return result;
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

    }
}
