using AsoLibs.VO;

namespace AsoLibs.POS
{
    public interface IPOS
    {
        RecvVO CreditCardApprove(SendVO vo);
    }
}
