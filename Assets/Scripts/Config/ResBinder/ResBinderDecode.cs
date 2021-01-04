using Helper;

namespace Config
{
    public static class ResBinderDecode
    {
        public static void Decode(ResBinderConfig cfg, ConfigTableReader tableReader)
        {
            
            tableReader.ReadString(0, out cfg.Key);
            tableReader.ReadString(1, out cfg.Url);
        }
    }
}
