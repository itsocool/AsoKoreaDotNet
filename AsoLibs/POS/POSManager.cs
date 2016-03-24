
namespace AsoLibs.POS
{
    class POSManager
    {
        public static IPOS GetPOS(string providerName)
        {
            IPOS result = null;

            switch (providerName)
            {
                case "KOVAN":
                    result = new KoVanPOS();
                    break;
                case "NICE":
                    result = new NicePOS();
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
