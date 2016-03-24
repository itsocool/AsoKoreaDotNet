using AsoLibs.VO;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AsoLibs.POS
{
    public class NicePOS : IPOS
    {
        [DllImport(@"extlibs\PosToCatReqL.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 ReqToCat
            (string CatIP,
            Int32 CatPort,
            string SendData,
            StringBuilder RecvData
            );

        public NicePOS()
        {

        }

        public RecvVO CreditCardApprove(SendVO sendVO)
        {
            //int count = 0;
            //Array.ForEach<int>(RecvVO.bytes, delegate (int e) { count += e; });
            RecvVO recvVO = null;
            //StringBuilder recvData = new StringBuilder(count);
            int returnValue = -1;

            try
            {
                string ip = sendVO.Ip;
                int port = sendVO.Port;
                string amount = sendVO.Amount;
                string halbu = sendVO.Halbu;
                Int32 gubun = sendVO.Gubun;
                string orgAuthDate = sendVO.OrgAuthDate;
                string orgAuthNo = sendVO.OrgAuthNo;

                string STX = ((char)0x02).ToString();
                string ETX = ((char)0x03).ToString();
                string FS = ((char)0x1C).ToString();

                string sendData = STX + "D1" + FS + FS + FS + halbu + FS + FS + FS + "1004" + FS + FS + FS + FS + FS + ETX;
                StringBuilder recvByte = new StringBuilder();

                //returnValue = ReqToCat(ip, port, sendData, recvByte);
                returnValue = ReqToCat(ip, port, "***", recvByte);
                string result = recvByte.ToString();
                MessageBox.Show(result);

                recvVO = new RecvVO((int)returnValue, recvByte.ToString());
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                MessageBox.Show(e.Message);
                if (recvVO == null)
                {
                    recvVO = new RecvVO((int)returnValue, "    N");
                }
                else if (recvVO.GetValue(1) == null)

                    recvVO.ReturnValue = -1;
                return recvVO;
            }

            return recvVO;
        }
    }
}
