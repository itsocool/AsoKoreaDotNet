using AsoLibs.VO;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AsoLibs.POS
{
    public class NicePOS : IPOS
    {
        [DllImport(@"Extlibs\PosToCatReqL.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 ReqToCat(byte[] CatIP, Int32 CatPort, byte[] SendData, byte[] RecvData);

        public RecvVO Send(SendVO sendVO)
        {
            RecvVO recvVO = null;
            byte[] recvData = new byte[2000];
            Int32 returnValue = 0;

            //try
            //{
            //    string ip = sendVO.Ip;
            //    int port = sendVO.Port;
            //    string amount = Convert.ToString(sendVO.Amount);
            //    string halbu = sendVO.Halbu;
            //    string gubun = sendVO.Gubun;
            //    string orgAuthDate = sendVO.AuthDate;
            //    string orgAuthNo = sendVO.AuthNo;

            //    string STX = ((char)0x02).ToString();
            //    string ETX = ((char)0x03).ToString();
            //    string FS = ((char)0x1C).ToString();

            //    halbu = "00";
            //    amount = "000001004";

            //    string sendData = STX + "D1" + FS + FS + FS + halbu + FS + FS + FS + amount + FS + FS + FS + FS + FS + ETX;
            //    sendData = STX + "D5" + FS + ETX;
            //    sendData = "***";
            //    byte[] sendBytes = Encoding.Default.GetBytes(sendData);

            //    returnValue = ReqToCat(Encoding.Default.GetBytes(ip), 5000, sendBytes, recvData);
            //    string result = Encoding.Default.GetString(recvData);
            //    recvVO = new RecvVO((int)returnValue, result);
            //}
            //catch (Exception e)
            //{
            //    Console.Write(e.Message);
            //    MessageBox.Show(e.Message);
            //    if (recvVO == null)
            //    {
            //        recvVO = new RecvVO((int)returnValue, "    N");
            //    }
            //    else if (recvVO.GetValue(1) == null)

            //        recvVO.ReturnValue = -1;
            //    return recvVO;
            //}

            return recvVO;
        }

        private RecvVO CreateRecvVO()
        {
            RecvVO result = new RecvVO();
            return result;
        }
    }
}
