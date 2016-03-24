using AsoLibs.VO;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AsoLibs.POS
{
    public class KoVanPOS : IPOS
    {
        [DllImport(@"extlibs\KovanSocketCat.dll")]
        public static extern Int32 Kovan_Send
            (string IP,
            Int32 PORT,
            string amount,
            string halbu,
            Int32 gubun,
            string org_auth_date,
            string org_auth_no,
            StringBuilder recv_data
            ,string xxx
            );

        public RecvVO CreditCardApprove(SendVO sendVO)
        {
            int count = 0;
            Array.ForEach<int>(RecvVO.bytes, delegate(int e) { count += e; });
            RecvVO recvVO = null;
            StringBuilder recvData = new StringBuilder(count);
            Int32 returnValue = -1;
            
            try
            {
                string ip = sendVO.Ip;
                Int32 port = sendVO.Port;
                string amount = sendVO.Amount;
                string halbu = sendVO.Halbu;
                Int32 gubun = sendVO.Gubun;
                string orgAuthDate = sendVO.OrgAuthDate;
                string orgAuthNo = sendVO.OrgAuthNo;
                returnValue = Kovan_Send(ip, port, amount, halbu, gubun, orgAuthDate, orgAuthNo, recvData, null);
                recvVO = new RecvVO(returnValue, recvData.ToString());
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                if(recvVO == null)
                {
                    recvVO = new RecvVO(returnValue, "    N");
                }else if(recvVO.GetValue(1) == null)

                recvVO.ReturnValue = -1;
                return recvVO;
            }

            return recvVO;
        }
    }

    public enum KOVANFieldNames
    {
        DataLength,
        IsApproved,
        CardNumber,
        Amount,
        Tex,
        ApprovalNo,
        ShopNo,
        CardIssuerCode,
        CardIssuerName,
        CardPurchaseCode,
        CardPurchaseName
    }
}
