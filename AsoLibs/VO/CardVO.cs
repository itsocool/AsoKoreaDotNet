using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AsoLibs.VO
{
    public class CardVO
    {
		public string billNo; //영수증번호
		public string businessNO; // 사업자번호
		public string ownerName; // 대표자
		public string companyName; // 상호
		public string greetings; // 인사말

        public List<CardItemVO> cardItemVOList; // 판매내역 리스트
        public List<CardApprovalVO> approvalVOList; //승인관련 리스트
    }
}
