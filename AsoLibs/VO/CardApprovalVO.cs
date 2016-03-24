using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AsoLibs.VO
{
    public class CardApprovalVO
    {
		public string amount; // 총승인금액
		public string halbu;
		public int gubun;
		public string org_auth_date;
		public string org_auth_no;
		public bool is_confirm;
		public string confirmMSG;
		public string cardNumber;
		public bool is_cancel;
        public string tex;
        public string posMemberNo;
        public string cardIssueCompanyCode;
        public string cardIssueCompanyName;
        public string buyCompanyCode;
        public string buyCompanyName;
		public bool is_save;
        public int returnValue = -1;
    }
}
