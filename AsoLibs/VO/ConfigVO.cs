using System;

namespace AsoLibs.VO
{
    [Serializable]
    public class ConfigVO
    {
        // 밴사
        public string Van { get; set; }
        // 아이피 주소
        public string Ip { get; set; }
        // 접속 포트 번호
        public int Port { get; set; }
        // 설명
        public string Desc { get; set; }
        // 프린터 (SERIAL, IP)
        public string Printer { get; set; }
        // 프린터 포트 번호
        public string PrinterPort { get; set; }
        // 프린터 출력 최대 컬럼수
        public int PrinterWidth { get; set; }
        // 프린터 테스트에 사용할 메세지
        public string TestPrintMessage { get; set; }
    }
}
