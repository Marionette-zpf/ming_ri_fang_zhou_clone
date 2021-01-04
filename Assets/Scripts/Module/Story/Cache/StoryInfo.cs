using System.Collections.Generic;

namespace Module.Story
{
    /// <summary>
    /// Date    2020/12/20 22:01:53
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class StoryInfo
    {
        public string Title;
        public string BgInfo;
        public List<DialogInfo> DialogInfos = new List<DialogInfo>();
    }

    public class DialogInfo
    {
        public string Character;
        public string Context;
        public string CharIconInfo;
        public List<OptionInfo> Options;
    }

    public class OptionInfo
    {
        public string Option;
        public DialogInfo NextDialog = new DialogInfo();
    }
}