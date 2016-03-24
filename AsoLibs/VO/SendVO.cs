using System;
using System.Runtime.InteropServices;

namespace AsoLibs.VO
{
    public class SendVO
    {
        public string VANProvider { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string Amount { get; set; }
        public string Halbu { get; set; }
        public int Gubun { get; set; }
        public string OrgAuthDate { get; set; }
        public string OrgAuthNo { get; set; }
        public string RecvData { get; set; }

        public SendVO(string vanProvider, string ip, int port, int amount, string halbu, int gubun, string orgAuthDate, string orgAuthNo)
        {
            try
            {
                VANProvider = vanProvider;

                Ip = ip;

                Port = port;

                Amount = amount.ToString("D8");

                Halbu = halbu;

                Gubun = gubun;

                OrgAuthDate = (orgAuthDate == null) ? "" : orgAuthDate;

                OrgAuthNo = (orgAuthNo == null) ? "" : orgAuthNo;

                RecvData = "";
            }
            catch (Exception ex)
            {
                 Console.WriteLine(ex.Message);
            }
        }
    }
}
