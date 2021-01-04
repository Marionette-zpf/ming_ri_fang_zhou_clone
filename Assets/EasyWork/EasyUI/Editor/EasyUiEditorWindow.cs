using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EasyWork.EasyUi.Editor
{
    public enum SupportUiElement
    {
        None,
        Text,
        Image,
        GridLayoutGroup,
        VerticalLayoutGroup,
        HorizontalLayoutGroup,
        EUiCom,
        CanvasGroup
    }

    public class EasyUiEditorWindow : EditorWindow
    {

        private const string PROPERTY_TEMPLATE = @"
        private #type# m_#name#;
        public #type# #name#
        {
            get
            {
                if (m_#name# == null)
                {
                    m_#name# = transform.Find(""#path#"").GetComponent<#type#>();
                }
                return m_#name#;
            }
        }
        ";

        private const string CLASS_TEMPLATE = @"using UnityEngine.UI;
using EasyUI;

namespace #namespace#
{
    /// <summary>
    /// ---------------- auto generator -----------------
    /// </summary>
    public class Base#class# : BasePanel
    {
        #property#
    }
}
        ";

        private const string COM_TEMPLATE = @"using UnityEngine.UI;
using EasyUI;

namespace #namespace#
{
    /// <summary>
    /// ---------------- auto generator -----------------
    /// </summary>
    public class Base#class# : EUiCom
    {
        #property#
    }
}
        ";



        private static Dictionary<SupportUiElement, string[]> g_filteRule = new Dictionary<SupportUiElement, string[]>()
        {
            { SupportUiElement.Text,  new []{ "txt_", "text_" } },
            { SupportUiElement.Image,  new []{ "image_", "icon_" } },
            { SupportUiElement.GridLayoutGroup,  new []{ "glgroup_" } },
            { SupportUiElement.VerticalLayoutGroup,  new []{ "vlgroup_" } },
            { SupportUiElement.HorizontalLayoutGroup,  new []{ "hlgroup_" } },
            { SupportUiElement.EUiCom,  new []{ "com_" } },
            { SupportUiElement.CanvasGroup,  new []{ "cg_" } }
        };

        private Transform m_root;

        public string m_generationPath;
        private string m_classCode;
        private string m_propertyCode;

        [MenuItem("EasyWork/EasyUi")]
        public static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(EasyUiEditorWindow));
            window.Show();
        }

        private void OnGUI()
        {

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("路径:", GUILayout.Width(30));
            m_generationPath = EditorGUILayout.TextField(m_generationPath, GUILayout.Width(300));

            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(300));
            rect.x = 30;
            if ((Event.current.type == EventType.DragUpdated
              || Event.current.type == EventType.DragExited)
              && rect.Contains(Event.current.mousePosition))
            {
                //改变鼠标的外表  
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                {
                    m_generationPath = DragAndDrop.paths[0];
                }
            }
            EditorGUILayout.EndHorizontal();


            if (GUILayout.Button("Generator"))
            {
                var selectedObjs = Selection.gameObjects;

                for (int i = 0; i < selectedObjs.Length; i++)
                {
                    m_root = selectedObjs[i].transform;
                    Generator();
                }
            }
        }

        public void Generator()
        {

            m_propertyCode = string.Empty;
            m_root = Selection.activeGameObject.transform;

            if (m_root.name.ToLower().Contains("panel"))
            {
                m_classCode = CLASS_TEMPLATE;
            }
            else if (m_root.name.ToLower().Contains("com"))
            {
                m_classCode = COM_TEMPLATE;
            }
            else
            {
                return;
            }

            GenerationPropertyCodeByRoot(m_root);

            m_classCode = m_classCode.Replace("#namespace#", m_generationPath.Replace("Assets/", string.Empty).Replace('/', '.'));
            m_classCode = m_classCode.Replace("#property#", m_propertyCode);
            m_classCode = m_classCode.Replace("#class#", FormatName(m_root.name));

            GeneratorScript();
        }

        private void GeneratorScript()
        {
            string folder = Application.dataPath + "/" + m_generationPath.Replace("Assets/", string.Empty);
            string scriptPath = folder + "/Base" + FormatName(m_root.name) + ".cs";

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            FileStream file = new FileStream(scriptPath, FileMode.OpenOrCreate);
            StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
            fileW.Write(m_classCode);
            fileW.Flush();
            fileW.Close();
            file.Close();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private string FormatName(string name)
        {
            return name.Replace("_", string.Empty).Replace("com", "Com");
        }

        private void GenerationPropertyCodeByRoot(Transform transform)
        {
            foreach (Transform child in transform)
            {
                if (child.childCount != 0)
                {
                    GenerationPropertyCodeByRoot(child);
                }

                if (transform == child)
                {
                    continue;
                }

                var supportUiElement = GetElementType(child.name);
                if (supportUiElement != SupportUiElement.None)
                {
                    GenerationPropertyCode(child.name, GetFullPath(child), supportUiElement);
                }
            }
        }

        private void GenerationPropertyCode(string name, string path, SupportUiElement supportUiElement)
        {
            var tempContext = PROPERTY_TEMPLATE;
            tempContext = tempContext.Replace("#type#", supportUiElement.ToString());
            tempContext = tempContext.Replace("#name#", name);
            tempContext = tempContext.Replace("#path#", path);

            m_propertyCode += tempContext;
        }

        private string GetFullPath(Transform transform)
        {
            if (transform == m_root)
            {
                throw new System.Exception("transform is self");
            }

            var result = transform.name;
            var tempTrans = transform.parent;

            while (tempTrans != m_root)
            {
                result = tempTrans.name + "/" + result;
                tempTrans = tempTrans.parent;
            }

            return result;
        }

        private SupportUiElement GetElementType(string objName)
        {
            var lowerName = objName.ToLower();

            foreach (var rule in g_filteRule)
            {
                for (int i = 0; i < rule.Value.Length; i++)
                {
                    if (lowerName.Contains(rule.Value[i]))
                    {
                        return rule.Key;
                    }
                }
            }

            return SupportUiElement.None;
        }

    }

}
