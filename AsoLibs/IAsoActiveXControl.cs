using AsoLibs.config;
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

        #endregion

        #region Methods

        ConfigVO GetPOSInfo();

        void Init(ConfigVO vo); 

        string Echo(string msg);

        RecvVO Send(SendVO vo);

        object Test(object obj);

        void Print(string msg, int printMode);

        void PrintString(string str);

        #endregion

    }
}
