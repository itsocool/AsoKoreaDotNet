using AsoLibs.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsoLibs.Message
{
    public interface IMessage
    {
        string SendString { get; set; }
        string RecvString { get; set; }

        void SetSendVO(SendVO vo);

        byte[] ToByteArray();

    }

    public class MessageConst
    {
        //A.STX : Start of text(0x02)
        //B.ETX : End of text(0x03)
        //C.EOT : End of transmission(0x04)
        //D.ENQ : Enquiry(0x05)
        //E.ACK : Ackknowledge(0x06)
        //F.NAK : Negative Acknowledge(0x15)
        //G.FS: file Separator(0x1C)

        public const char STX = (char)0x02; 
        public const char ETX = (char)0x03; 
        public const char EOT = (char)0x04; 
        public const char ENQ = (char)0x05; 
        public const char ACK = (char)0x06; 
        public const char NAK = (char)0x15;
        public const char FS = (char)0x1C;
    }
    
}
