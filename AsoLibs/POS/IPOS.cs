using AsoLibs.VO;

namespace AsoLibs.POS
{
    interface IPOS
    {
        RecvVO CreditCardApprove(SendVO vo);
    }
}
