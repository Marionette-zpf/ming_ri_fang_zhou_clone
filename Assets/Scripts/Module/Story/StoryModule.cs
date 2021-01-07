using Key;
using Manager;

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


}

