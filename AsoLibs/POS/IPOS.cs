using AsoLibs.VO;

namespace AsoLibs.POS
{
    public interface IPOS
    {
        RecvVO Send(SendVO vo);
    }
}
