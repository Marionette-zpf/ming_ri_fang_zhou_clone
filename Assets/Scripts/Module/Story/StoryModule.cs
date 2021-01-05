using EasyWork.Extend.Utilities;
using EController.Story;
using Extend.System;
using Key;
using LitJson;
using Manager;
using Module.Story.Cache;
using System;
using System.IO;
using UnityEngine;

namespace Module.Story
{
    public partial class StoryModule : BaseModule
    {
        public void EnterStory(string story)
        {

        }

        protected override void OnInit()
        {
            Register(GameKey.DATA_DIALOG_PLOT, () => DataManager.Get<string>(GameKey.DATA_DIALOG_PLOT));

            ESceneManager.SceneLoaded += LoadSceneHandle;
        }

        private void LoadSceneHandle(string scene)
        {
            if(scene != "Story")
            {
                return;
            }

            CommandManager.ExcuteCommand("EnterPlotCommand");
        }
    }

    public class EnterPlotCommand : BaseCommand, IModuleBinder<StoryModule>
    {
        public override void Excute(params object[] param)
        {
            this.GetData(GameKey.DATA_DIALOG_PLOT, out string plot);

            if(string.IsNullOrEmpty(plot))
            {
                plot = "教程-行动前";
            }

            var path = Application.dataPath + "/Config/Dialog/" + plot + ".json";
            var jsonData = File.ReadAllText(path);

            var dialogFragment = JsonMapper.ToObject<DialogFragment>(jsonData);
            if(dialogFragment == null)
            {
                ELogUtil.LogError($"dialogFragment is null, path:{path}");
                return;
            }

            PanelManager.Open("StoryPanel", dialogFragment);
        }
    }
}

