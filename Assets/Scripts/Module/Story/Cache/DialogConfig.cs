using System.Collections.Generic;

namespace Module.Story.Cache
{
    /// <summary>
    /// Date    2021/1/1 19:31:47
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class DialogConfig
    {
        public uint Id;
        public uint CharacterId;
        public uint OptionId;
        public string Dialog;
        public string DialogBg;
        public List<string> CharacterPainting;
        public List<bool> GreyPainting;
        public List<DialogEvent> DialogEvents;
    }

    public class DialogFragment
    {
        public uint Id;
        public List<DialogConfig> DialogConfigs;
    }

    public class DialogEvent
    {
        public uint Id;
        public string Parma;
    }
}