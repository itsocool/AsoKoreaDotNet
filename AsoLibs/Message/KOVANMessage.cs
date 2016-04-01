using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsoLibs.VO;

namespace AsoLibs.Message
{
    public class KoVanMessage : IMessage
    {
        private SendVO innerSendVO = null;
        private string sendString = null;

        public SendVO sendVO
        {
            get
            {
                return innerSendVO;
            }

            set
            {
                innerSendVO = value;
                //sendString = ToString();
                sendString = GetTestString();

            }
        }

        public override string ToString()
        {
            string result = null;
            int len = 0;

            if(innerSendVO != null)
            {
                result = getServiceCode(innerSendVO.ServiceCode);
                result += MessageConst.FS.ToString();
                result += innerSendVO.Halbu;
                result += MessageConst.FS.ToString();
                result += innerSendVO.Amount.ToString("D9");
                result += MessageConst.FS.ToString();
                result += innerSendVO.AuthDate;
                result += MessageConst.FS.ToString();
                result += innerSendVO.AuthNo;
                result += MessageConst.FS.ToString();
                result += MessageConst.ETX.ToString();

                len = result.Length + 5;

                result = MessageConst.STX.ToString() + len.ToString("D4") + result;
            }

            return result;
        }

        private string GetTestString()
        {
            string result = null;
            int len = 0;

            if (innerSendVO != null)
            {
                result = "GD";
                result += MessageConst.FS.ToString();
                result += 1.ToString("D12");
                result += MessageConst.FS.ToString();
                result += new String(' ', 80);
                result += MessageConst.FS.ToString();
                result += MessageConst.ETX.ToString();

                len = result.Length + 5;

                result = MessageConst.STX.ToString() + len.ToString("D4") + result;
            }

            return result;
        }

        public string getServiceCode(string gubun)
        {
            string result = "";

            if ("CREDIT_APPROVAL".Equals(gubun))
            {
                result = "S0";
            }else if("CREDIT_CANCEL".Equals(gubun))
            {
                result = "S1";
            }
            else if ("CASH_APPROVAL".Equals(gubun))
            {
                result = "41";
            }
            else if ("CASH_CANCEL".Equals(gubun))
            {
                result = "42";
            }
            else
            {
                result = (string.IsNullOrWhiteSpace(gubun)) ? "  " : gubun;
            }

            return result;
        }

        public byte[] ToByteArray()
        {
            byte[] result = null;

            if(!string.IsNullOrWhiteSpace(sendString))
            {
                result = Encoding.Default.GetBytes(sendString);
            }

            return result;
        }
    }
}
