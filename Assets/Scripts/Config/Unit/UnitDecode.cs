using Helper;

namespace Config
{
    public static class UnitDecode
    {
        public static void Decode(UnitConfig cfg, ConfigTableReader tableReader)
        {
            
            tableReader.ReadInt(0, out cfg.Key);
            tableReader.ReadString(1, out cfg.ResUrl);
            tableReader.ReadUint(2, out cfg.PropertiesKey);
        }
    }
}
