using AsoLibs.VO;
using System;
using System.Runtime.InteropServices;

namespace AsoLibs
{
    [Guid("F8A07883-E2AC-4DE2-A330-4880A72C2B32")]
    public interface IAsoActiveXControl
    {
        string Echo(string msg);

        object CreditCardApprove(int amount, string halbu, int gubun, string org_auth_date, string org_auth_no);

        object Test(object obj);

        void Print(string msg, int printMode);

        string[] GetPrinterPorts();

        string SetPrintPort(string portName);

        void ConfigPrinter(string portName, int width);

        object GetPrinterConfig();
    }
}
