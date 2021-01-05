using Helper;

namespace Config
{
    public static class CharacterDecode
    {
        public static void Decode(CharacterConfig cfg, ConfigTableReader tableReader)
        {
            
            tableReader.ReadUint(0, out cfg.Id);
            tableReader.ReadString(1, out cfg.Name);
        }
    }
}
