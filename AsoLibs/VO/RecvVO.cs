using System;

namespace AsoLibs.VO
{
    [Serializable]
    public class RecvVO
    {
        public int Amount { get; set; } // 총승인금액
        public string Halbu { get; set; }
        public string Gubun { get; set; }
        public string Auth_date { get; set; }
        public string Auth_no { get; set; }
        public string CardNumber { get; set; }
        public string Tex { get; set; }
        public string PosMemberNo { get; set; }
        public string CardIssueCompanyCode { get; set; }
        public string CardIssueCompanyName { get; set; }
        public string BuyCompanyCode { get; set; }
        public string BuyCompanyName { get; set; }
        public string ReturnValue { get; set; }
        public string ReturnMessage { get; set; }
        public string SendString { get; set; }

    }
}
