using Helper;

namespace Config
{
    public static class UnitPropertiesDecode
    {
        public static void Decode(UnitPropertiesConfig cfg, ConfigTableReader tableReader)
        {
            
            tableReader.ReadUint(0, out cfg.Id);
            tableReader.ReadIntArray2(1, out cfg.Range);
            tableReader.ReadFloat(2, out cfg.Rate);
            tableReader.ReadFloat(3, out cfg.Health);
            tableReader.ReadFloat(4, out cfg.Attack);
            tableReader.ReadFloat(5, out cfg.RateGrow);
            tableReader.ReadFloat(6, out cfg.HealthGrow);
            tableReader.ReadFloat(7, out cfg.AttackGrow);
            tableReader.ReadFloat(8, out cfg.Speed);
        }
    }
}
