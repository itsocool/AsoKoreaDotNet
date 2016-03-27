using AsoLibs.VO;
using System;
using System.Runtime.InteropServices;

namespace AsoLibs
{
    [Guid("F8A07883-E2AC-4DE2-A330-4880A72C2B32")]
    public interface IAsoActiveXControl
    {
        #region Properties

        int ForeColor { get; set; }         // Typical control property
        int BackColor { get; set; }         // Typical control property
        string Van { get; set; }
        string IP { get; set; }
        int Port { get; set; }
        string PrinterMode { get; set; }
        string PrinterWidth { get; set; }

        #endregion

        #region Methods

        string Echo(string msg);

        [Obsolete("구버전 승인 함수, 추후 삭제예정 사용 금지")]
        object CreditCardApprove(int amount, string halbu, int gubun, string org_auth_date, string org_auth_no);

        RecvVO Send(SendVO vo);

        object Test(object obj);

        void Print(string msg, int printMode);

        void PrintString(string str);

        #endregion

    }
}
