using System;
using System.Runtime.InteropServices;

namespace AsoLibs
{
    [Guid("0E17FFBC-9F1F-4F5A-B6A9-6FAAEC55A2D4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    interface ControlEvents
    {
        [DispId(0x10000001)]
        void OnTest(string url);
    }
}
