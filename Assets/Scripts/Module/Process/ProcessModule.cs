using Key;
using Manager;
using Manager.Res;
using Module.Process.Cache;
using Module.Story;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Module.Process
{
    /// <summary>
    /// Date    2021/1/3 15:27:04
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public partial class ProcessModule : BaseModule
    {

    }

    public class EntranceScene : BaseCommand
    {
        public override void Excute(params object[] param)
        {
            var info = DataManager.GetFromPlayerPrefs<ProcessInfo>(GameKey.PLAYER_PREFS_GAME_PROCESS);
            if(info == null)
            {
                info = new ProcessInfo() { CurProcess = "0-0" };
                DataManager.Save2PlayerPrefs(GameKey.PLAYER_PREFS_GAME_PROCESS, info);
                ESceneManager.LoadSceneAsync(GameKey.SCENE_STORY, LoadSceneMode.Single, null, null, loader => PanelManager.Open("StoryPanel"));
            }
            else
            {
                
            }

        }
    }
}