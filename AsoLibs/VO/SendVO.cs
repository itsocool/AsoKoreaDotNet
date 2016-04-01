using System;
using System.Runtime.InteropServices;

namespace AsoLibs.VO
{
    [Serializable]
    public class SendVO
    {
        public string Van { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public int Amount { get; set; }
        public string Halbu { get; set; }
        public string ServiceCode { get; set; }
        public string AuthDate { get; set; }
        public string AuthNo { get; set; }
    }
}
