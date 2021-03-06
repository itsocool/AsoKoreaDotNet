﻿using AsoLibs.VO;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace AsoLibs.Printer
{
    interface IPrinter
    {
        string[] Ports{get;}
        int Width { get; set; }
        string PortName { get; set; }
        bool IsOpen { get; }
        int Open();
        int Init();
        int Print(String data, int printMode);
        int Test();
        int Cut();
        int Close();
        int Beep();
    }
}
