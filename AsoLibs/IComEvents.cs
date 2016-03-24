using AsoLibs.VO;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace AsoLibs
{
    [Guid("0729C126-C550-493C-A86E-B4E72CC0520E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    interface IComEvents
    {
        [DispId(1)]
        void OnInit(string arg);

        [DispId(2)]
        void OnApprove(RecvVO vo);

        [DispId(3)]
        void OnPrint(CardVO vo);
    }
}
