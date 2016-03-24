using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace AsoLibs.VO
{
    public class ConfigVO
    {
        public string posIp; // POS 단말기 IP
		public int posPort; // POS 단말기 포트 번호
		public string posPrinter; // 프린터 유형
		public int posPrinterWidth; // 프린터 용지 폭
		public string posPrinterPort; // 프린터 시리얼 포트
    }
}
