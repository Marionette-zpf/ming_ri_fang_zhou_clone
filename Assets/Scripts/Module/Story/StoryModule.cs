using EController.Story;
using Extend.System;
using Manager;
using System;

namespace Module.Story
{
    public partial class StoryModule : BaseModule
    {
        public void EnterStory(string story)
        {

        }

        protected override void OnInit()
        {
            ESceneManager.SceneLoaded += LoadSceneHandle;
        }

        private void LoadSceneHandle(string scene)
        {
            if(scene != "Story")
            {
                return;
            }
        }
    }

    public class EnterPlotCommand : BaseCommand
    {
        public override void Excute(params object[] param)
        {
            var plot = param.Get<string>();

            switch (plot)
            {
                case "0-0":

                    break;
                default:
                    break;
            }
        }
    }
}

