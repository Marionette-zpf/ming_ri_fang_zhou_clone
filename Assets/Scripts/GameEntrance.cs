using LitJson;
using Manager;
using Module;
using Module.Story.Cache;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEntrance : MonoBehaviour
{
    public Button button;
    public Button Game;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            ESceneManager.LoadSceneAsync("Story");
        });

        Game.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Game");
        });

        InitializeCommand();
        InitializeModule();
        InitializeScene();
    }

    private void InitializeScene()
    {
        ESceneManager.AddPreloadAsset("Story", "prefab_ui_story_StoryCharCom");
    }

    private void InitializeModule()
    {
        var baseModuleType = typeof(BaseModule);
        var assembly = Assembly.GetAssembly(baseModuleType);

        var assemblyTypes = assembly.GetTypes();
        for (int i = 0; i < assemblyTypes.Length; i++)
        {
            var moduleType = assemblyTypes[i];
            if (!baseModuleType.IsAssignableFrom(moduleType) || moduleType == baseModuleType)
            {
                continue;
            }
            var module = Activator.CreateInstance(moduleType) as BaseModule;
            if (module.InitOnGameEntrance)
            {
                ModuleManager.RegisterModule(module);
            }
        }
    }
    private void InitializeCommand()
    {
        var baseCommandType = typeof(BaseCommand);
        var assembly = Assembly.GetAssembly(baseCommandType);

        var assemblyTypes = assembly.GetTypes();
        for (int i = 0; i < assemblyTypes.Length; i++)
        {
            var commandType = assemblyTypes[i];
            if (!baseCommandType.IsAssignableFrom(commandType) || commandType == baseCommandType)
            {
                continue;
            }

            CommandManager.RegisterCommand(Activator.CreateInstance(commandType) as BaseCommand);
        }
    }
}




