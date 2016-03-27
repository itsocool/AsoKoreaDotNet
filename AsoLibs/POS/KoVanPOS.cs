using AsoLibs.VO;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace AsoLibs.POS
{
    public class KoVanPOS : IPOS
    {
        [DllImport(@"Extlibs\KovanSocketCat.dll")]
        public static extern Int32 Kovan_Send
            (string IP,
            Int32 PORT,
            string amount,
            string halbu,
            Int32 gubun,
            string org_auth_date,
            string org_auth_no,
            StringBuilder recv_data
            );

        public RecvVO Send(SendVO sendVO)
        {
            RecvVO recvVO = null;
            Int32 returnValue = -1;
            TcpClient client = null;
            
            try
            {
                string sendString = SendVO2String(sendVO);
                string recvString = null;
                byte[] sendBuff = Encoding.Default.GetBytes(sendString);
                byte[] recvBuff = new byte[4096];

                client = new TcpClient(sendVO.Ip, sendVO.Port);

                using (NetworkStream stream = client.GetStream())
                {

                    stream.Write(sendBuff, 0, sendBuff.Length);
                    stream.Read(recvBuff, 0, recvBuff.Length);

                    if(recvBuff != null && recvBuff.Length > 1)
                    {
                        recvString = Encoding.Default.GetString(recvBuff);
                        recvVO = String2RecvVO(recvString, sendString);
                    }
                }
            }
            catch (Exception ex)
            {
                recvVO = new RecvVO();
                recvVO.ReturnValue = "-2";
                recvVO.ReturnMessage = ex.Message;
                return recvVO;
            }

            return recvVO;
        }

        private string SendVO2String(SendVO vo)
        {
            // 거래구분
            //10 : MSR 신용승인
            //11 : MSR 신용취소
            //S0: IC 승인
            //S1: IC 취소
            //X0: MSR / IC 통한 승인
            //X1: MSR / IC 통한 취소
            //A0: MSR 신용승인 +멤버쉽
            //A1: MSR 신용취소 +멤버쉽
            //S2: IC 신용승인 +멤버쉽(E1 주유소)
            //S3: IC 신용취소 +멤버쉽(E1 주유소)
            //H0: 현금IC카드 승인
            //H1: 현금IC카드 취소

            char STX = (char)0x02;
            char FS = (char)0x1C;
            char ETX = (char)0x03;

            int len = 0;

            string result = null;

            if (vo != null)
            {
                result += vo.Gubun;         // 거래구분
                result += FS;
                result += vo.Halbu;         // 할부개월수
                result += FS;
                result += vo.Amount;        // 승인금액
                result += FS;
                result += vo.AuthDate;      // 원거래일자
                result += FS;
                result += vo.AuthNo;        // 원거래번호
                result += FS;
                result += "";               // POS Special User Field
                result += ETX;

                len = 1 + 4 + result.Length;

                result = STX + len.ToString("D4");    // 4자리 남으면 0으로 채움
            }

            return result;              
        }

        private RecvVO String2RecvVO(string str, string sendString = null)
        {
            RecvVO result = null;
            return result;
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
