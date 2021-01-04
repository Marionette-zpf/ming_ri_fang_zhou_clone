using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
public partial class CodeGenerator
{
    [MenuItem("Assets/CreateCode/Mono Class", false, 1)]
    public static void CreateMono()
    {
        CreateCode(g_monoCode);
    }

    [MenuItem("Assets/CreateCode/New Class", false, 1)]
    public static void CreateClass()
    {
        CreateCode(g_defaultCode);
    }

    [MenuItem("Assets/CreateCode/New Enum", false, 1)]
    public static void CreateEnum()
    {
        CreateCode(g_enumCodeType);
    }

    [MenuItem("Assets/CreateCode/New Module", false, 1)]
    public static void CreateModule()
    {
        CreateCode(g_moduleCode);
    }


    private static void CreateCode(string code, string className = g_emptyStr)
    {
        var guids = Selection.assetGUIDs;
        if (guids == null || guids.Length < 1)
        {
            return;
        }

        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        if (File.Exists(path))
        {
            path = Path.GetDirectoryName(path)?.Replace('\\', '/');
        }

        var curConfigUrl = string.Empty;
        foreach (var configUrl in g_configUrls)
        {
            if (path.StartsWith(configUrl))
            {
                curConfigUrl = configUrl;
                break;
            }
        }

        if (string.IsNullOrEmpty(curConfigUrl))
        {
            Debug.LogError("非法路径:" + path);
            return;
        }

        var codeType = g_defaultCodeType;
        var template = g_classTemplate;

        if (code == g_monoCode)
        {
            template = g_monoTemplate;
        }
        else if (code == g_enumCodeType)
        {
            codeType = g_enumCodeType;
        }
        else if (code == g_moduleCode)
        {
            template = g_moduleTemplate;
        }

        var callback = ScriptableObject.CreateInstance<DoCreateScriptAsset>();
        callback.Namespace = path.Replace(curConfigUrl, string.Empty).Replace('/', '.');
        callback.ClassType = codeType;
        callback.ClassName = className;
        callback.CodeType = code;
        var csIcon = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, callback, path + "/NewCode.cs", csIcon, template);
    }

    private class DoCreateScriptAsset : EndNameEditAction
    {
        public string Namespace;
        public string ClassType;
        public string ClassName;
        public string CodeType;

        public override void Action(int instanceId, string pathName, string template)
        {
            var className = ClassName == g_emptyStr ? Path.GetFileNameWithoutExtension(pathName) : ClassName;

            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            string curComputeUser = windowsIdentity.Name;

            template = template.Replace("#namespace#", Namespace);
            template = template.Replace("#type#", ClassType);
            template = template.Replace("#className#", className);
            template = template.Replace("#date#", DateTime.Now.ToString());
            template = template.Replace("#name#", curComputeUser);

            string fullPath = Path.GetFullPath(pathName);
            try
            {
                File.WriteAllText(fullPath, template, Encoding.UTF8);
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("创建代码错误", e.Message, "确定");
                return;
            }

            AssetDatabase.ImportAsset(pathName);
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(obj);

            if (CodeType == g_moduleCode)
            {
                var assetPath = AssetDatabase.GetAssetPath(obj);
                var systemPath = Application.dataPath.Replace("Assets", string.Empty) + pathName.Replace(Path.GetFileName(pathName), string.Empty);

                var moduleEvet = g_moduleEventTemplate.Replace("#className#", className.Replace("Module", string.Empty));
                moduleEvet = moduleEvet.Replace("#namespace#", Namespace);
                moduleEvet = moduleEvet.Replace("#date#", DateTime.Now.ToString());
                moduleEvet = moduleEvet.Replace("#name#", curComputeUser);

                File.WriteAllText(systemPath + className.Replace("Module", string.Empty) + "Event.cs", moduleEvet);

                var moduelNotify = g_moduleNotifyTemplate.Replace("#className#", className.Replace("Module", string.Empty));
                moduelNotify = moduelNotify.Replace("#namespace#", Namespace);
                moduelNotify = moduelNotify.Replace("#date#", DateTime.Now.ToString());
                moduelNotify = moduelNotify.Replace("#name#", curComputeUser);

                File.WriteAllText(systemPath + className.Replace("Module", string.Empty) + "Notify.cs", moduelNotify);

                AssetDatabase.CopyAsset(assetPath, assetPath.Replace(className, className + "_API"));
                AssetDatabase.CopyAsset(assetPath, assetPath.Replace(className, className + "_Cache"));

                if (!Directory.Exists(systemPath + "/Views"))
                {
                    Directory.CreateDirectory(systemPath + "/Views");
                }


                AssetDatabase.Refresh();
            }
        }
    }
}


public partial class CodeGenerator
{
    private const string g_emptyStr = "";

    private const string g_defaultCode = "Class";
    private const string g_monoCode = "MonoBehaviour";
    private const string g_moduleCode = "Module";

    private const string g_defaultCodeType = "class";
    private const string g_enumCodeType = "enum";

    private static readonly List<string> g_configUrls = new List<string>()
    {
        "Assets/Scripts/","Assets/EasyWork/"
    };

    private const string g_classTemplate = @"namespace #namespace#
{
    /// <summary>
    /// Date    #date#
    /// Name    #name#
    /// Desc    desc
    /// </summary>
    public #type# #className#
    {
    }
}";

    private const string g_monoTemplate = @"using UnityEngine;

namespace #namespace#
{
    /// <summary>
    /// Date    #date#
    /// Name    #name#
    /// Desc    desc
    /// </summary>
    public #type# #className# : MonoBehaviour
    {
        void Start()
        {
        }

        void Update()
        {
        }
    }
}";

    private const string g_moduleTemplate = @"using UnityEngine;

namespace #namespace#
{
    /// <summary>
    /// Date    #date#
    /// Name    #name#
    /// Desc    desc
    /// </summary>
    public partial #type# #className# : MonoBehaviour
    {
        
    }
}";

    private const string g_moduleEventTemplate = @"
namespace #namespace#
{
    /// <summary>
    /// Date    #date#
    /// Name    #name#
    /// Desc    desc
    /// </summary>
    public static class #className#Event
    {
        
    }
}";
    private const string g_moduleNotifyTemplate = @"
namespace #namespace#
{
    /// <summary>
    /// Date    #date#
    /// Name    #name#
    /// Desc    desc
    /// </summary>
    public static class #className#Notify
    {
        
    }
}";
}