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
                sendString = ToString();
            }
        }

        public override string ToString()
        {
            string result = null;
            string serviceCode = null;

            if(innerSendVO != null)
            {
                serviceCode = getServiceCode(innerSendVO.Gubun);
                result = MessageConst.STX.ToString();
                result += serviceCode;
                result += MessageConst.FS.ToString();
                result += MessageConst.FS.ToString();
                result += MessageConst.FS.ToString();
                result += innerSendVO.Halbu;
                result += MessageConst.FS.ToString();
                result += MessageConst.FS.ToString();
                result += MessageConst.FS.ToString();
                result += innerSendVO.Amount.ToString("D9");
                result += MessageConst.FS.ToString();
                result += MessageConst.FS.ToString();
                result += MessageConst.FS.ToString();
                result += MessageConst.FS.ToString();
                result += MessageConst.FS.ToString();
                result += MessageConst.ETX.ToString();
            }

            return result;
        }

        public string getServiceCode(string gubun)
        {
            string result = "";

            if ("CREDIT_APPROVAL".Equals(gubun))
            {
                result = "D1";
            }else if("CREDIT_CANCEL".Equals(gubun))
            {
                result = "D2";
            }
            else if ("CASH_APPROVAL".Equals(gubun))
            {
                result = "D3";
            }
            else if ("CASH_CANCEL".Equals(gubun))
            {
                result = "D4";
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
