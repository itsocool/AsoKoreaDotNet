using AsoLibs.Message;
using AsoLibs.VO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AsoLibs.POS
{
    public class NicePOS : IPOS
    {
        [DllImport(@"Extlibs\PosToCatReqL.dll", CharSet = CharSet.Unicode)]
        public static extern Int32 ReqToCat(byte[] CatIP, Int32 CatPort, byte[] SendData, byte[] RecvData);

        private RecvVO SendVO(SendVO vo)
        {
            RecvVO result = null;
            NiceMessage message = null;
            byte[] sendBytes = null;
            byte[] recvBytes = new byte[4096];

            try
            {
                IPAddress ip = IPAddress.Parse(vo.Ip);
                IPEndPoint ipep = new IPEndPoint(ip, vo.Port);

                //using (var socket = new Socket(vo.Ip, vo.Port))
                //{
                //    message = new NiceMessage();
                //    message.SetSendVO(vo);
                //    //sendBytes = message.ToByteArray();
                //    //socket.Connect(ipep);
                //    socket
                //    socket.Send(message.SendString);
                //}

                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ipep);
                    //client.SendTimeout = 5 * 1000;
                    //client.SendBufferSize = 4096;

                    using (NetworkStream stream = client.GetStream())
                    {
                        stream.Flush();
                        message = new NiceMessage();
                        message.sendVO = vo;
                        sendBytes = message.ToByteArray();
                        stream.Write(sendBytes, 0, sendBytes.Length);
                        Thread.Sleep(100);
                        stream.Read(recvBytes, 0, recvBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            //}finally
            //{
            //    if(client != null)
            //    {
            //        client.Close();
            //        client = null;
            //    }
            }

            return result;
        } 

        public RecvVO Send(SendVO sendVO)
        {
            RecvVO recvVO = null;
            byte[] recvData = null;
            Int32 returnValue = 0;

            try
            {
                string ip = sendVO.Ip;
                int port = sendVO.Port;
                string amount = sendVO.Amount.ToString();
                string halbu = sendVO.Halbu;
                string gubun = sendVO.ServiceCode;
                string authDate = sendVO.AuthDate;
                string authNo = sendVO.AuthNo;

                string STX = ((char)0x02).ToString();
                string ETX = ((char)0x03).ToString();
                string FS = ((char)0x1C).ToString();

                string sendData = STX + gubun + FS + FS + FS + halbu + FS + FS + FS + amount + FS + FS + FS + FS + FS + ETX;
                //sendData = "***";
                byte[] sendBytes = Encoding.Default.GetBytes(sendData);
                recvData = new byte[2000];
                //returnValue = ReqToCat(Encoding.Default.GetBytes(ip), port, sendBytes, recvData);
                //string result = Encoding.Default.GetString(recvData);
                //MessageBox.Show(result);

                recvVO = SendVO(sendVO);

                if(recvVO != null)
                {
                    recvVO.ReturnValue = Convert.ToString(returnValue);
                    recvVO.ReturnMessage = Encoding.Default.GetString(recvData);
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                MessageBox.Show(ex.StackTrace);
                if (recvVO == null)
                {
                    recvVO = new RecvVO();
                    recvVO.ReturnValue = "-1";
                }
                //else if (recvVO.GetValue(1) == null)

                //    recvVO.ReturnValue = -1;
                //return recvVO;
            }

            return recvVO;
        }

        private RecvVO CreateRecvVO()
        {
            RecvVO result = new RecvVO();
            return result;
        }
    }
}
