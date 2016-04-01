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

        private RecvVO SendVO(SendVO vo)
        {
            TcpClient client = null;

            RecvVO result = null;
            KoVanMessage message = null;
            byte[] sendBytes = null;
            byte[] recvBytes = new byte[4096];

            try
            {
                IPAddress ip = IPAddress.Parse(vo.Ip);
                IPEndPoint ipep = new IPEndPoint(ip, vo.Port);
                client = new TcpClient();
                client.Connect(ipep);

                using (NetworkStream stream = client.GetStream())
                {
                    message = new KoVanMessage();
                    message.sendVO = vo;
                    sendBytes = message.ToByteArray();
                    stream.Write(sendBytes, 0, sendBytes.Length);
                    Thread.Sleep(50);
                    stream.Read(recvBytes, 0, recvBytes.Length);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);

            }
            finally
            {
                if(client != null)
                {
                    client.Close();
                    client = null;
                }
            }

            return result;
        }

        public RecvVO Send(SendVO sendVO)
        {
            RecvVO recvVO = null;

            try
            {
                //string ip = sendVO.Ip;
                //int port = sendVO.Port;
                //string amount = sendVO.Amount.ToString();
                //string halbu = sendVO.Halbu;
                //string gubun = sendVO.ServiceCode;
                //string authDate = sendVO.AuthDate;
                //string authNo = sendVO.AuthNo;

                //string STX = ((char)0x02).ToString();
                //string ETX = ((char)0x03).ToString();
                //string FS = ((char)0x1C).ToString();

                //string sendData = STX + gubun + FS + FS + FS + halbu + FS + FS + FS + amount + FS + FS + FS + FS + FS + ETX;
                ////sendData = "***";
                //byte[] sendBytes = Encoding.Default.GetBytes(sendData);
                //recvData = new byte[2000];
                ////returnValue = ReqToCat(Encoding.Default.GetBytes(ip), port, sendBytes, recvData);
                ////string result = Encoding.Default.GetString(recvData);
                //MessageBox.Show(result);

                recvVO = SendVO(sendVO);

                //if (recvVO != null)
                //{
                //    recvVO.ReturnValue = Convert.ToString(returnValue);
                //    recvVO.ReturnMessage = Encoding.Default.GetString(recvData);
                //}

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
