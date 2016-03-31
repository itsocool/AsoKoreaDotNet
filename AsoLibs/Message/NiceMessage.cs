using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsoLibs.VO;

namespace AsoLibs.Message
{
    public class NiceMessage : IMessage
    {
        public string RecvString { get; set; }

        public string SendString { get; set; }

        public void SetSendVO(SendVO vo)
        {
            string serviceCode = getServiceCode(vo.Gubun);
            SendString = MessageConst.STX.ToString();

            SendString += serviceCode;
            SendString += MessageConst.FS.ToString();
            SendString += MessageConst.FS.ToString();
            SendString += MessageConst.FS.ToString();

            SendString += vo.Halbu;
            SendString += MessageConst.FS.ToString();
            SendString += MessageConst.FS.ToString();
            SendString += MessageConst.FS.ToString();

            SendString += vo.Amount.ToString("D9");

            SendString += MessageConst.FS.ToString();
            SendString += MessageConst.FS.ToString();
            SendString += MessageConst.FS.ToString();

            //SendString += new String(' ', 80);

            SendString += MessageConst.FS.ToString();
            SendString += MessageConst.FS.ToString();

            SendString += MessageConst.ETX.ToString();

        }

        private string getServiceCode(string gubun)
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

            if(!string.IsNullOrWhiteSpace(SendString))
            {
                result = Encoding.Default.GetBytes(SendString);
            }

            return result;
        }
    }
}
