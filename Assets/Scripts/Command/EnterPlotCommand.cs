using EasyWork.Extend.Utilities;
using Key;
using LitJson;
using Manager;
using Module;
using Module.Story;
using Module.Story.Cache;
using System.IO;
using UnityEngine;

namespace Command
{
    /// <summary>
    /// Date    2021/1/6 11:16:27
    /// Name    A12771\Administrator
    /// Desc    进入故事界面
    /// </summary>
    public class EnterPlotCommand : BaseCommand, IModuleBinder<StoryModule>
    {
        public override void Excute(params object[] param)
        {
            this.GetData(GameKey.DATA_DIALOG_PLOT, out string plot);

            if (string.IsNullOrEmpty(plot))
            {
                plot = "教程-行动前";
            }

            var path = Application.dataPath + "/Config/Dialog/" + plot + ".json";
            var jsonData = File.ReadAllText(path);

            var dialogFragment = JsonMapper.ToObject<DialogFragment>(jsonData);
            if (dialogFragment == null)
            {
                ELogUtil.LogError($"dialogFragment is null, path:{path}");
                return;
            }

            PanelManager.Open("StoryPanel", dialogFragment);
        }
    }
}