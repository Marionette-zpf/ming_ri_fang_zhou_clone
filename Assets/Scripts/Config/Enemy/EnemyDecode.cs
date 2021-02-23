using Helper;

namespace Config
{
    public static class EnemyDecode
    {
        public static void Decode(EnemyConfig cfg, ConfigTableReader tableReader)
        {
            
            tableReader.ReadInt(0, out cfg.Key);
            tableReader.ReadString(1, out cfg.ResUrl);
        }
    }
}
