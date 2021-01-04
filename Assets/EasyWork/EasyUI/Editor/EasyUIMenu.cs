using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUI.Editor
{
    /// <summary>
    /// Date    2020/12/21 23:10:05
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class EasyUIMenu
    {
        [MenuItem("GameObject/UI/EasyUI/EUiCom")]
        public static void EUiCom()
        {
            NewGo("com_", typeof(EUiCom));
        }

        [MenuItem("GameObject/UI/EasyUI/Text")]
        public static void Text()
        {
            NewGo("text_", typeof(Text));
        }

        [MenuItem("GameObject/UI/EasyUI/Image")]
        public static void Image()
        {
            NewGo("image_", typeof(Image));
        }

        [MenuItem("GameObject/UI/EasyUI/GridLayoutGroup")]
        public static void GridLayoutGroup()
        {
            NewGo("glGroup_", typeof(GridLayoutGroup));
        }

        [MenuItem("GameObject/UI/EasyUI/HorizontalLayoutGroup")]
        public static void HorizontalLayoutGroup()
        {
            NewGo("hlGroup_", typeof(HorizontalLayoutGroup));
        }

        [MenuItem("GameObject/UI/EasyUI/VerticalLayoutGroup")]
        public static void VerticalLayoutGroup()
        {
            NewGo("vlGroup_", typeof(VerticalLayoutGroup));
        }

        [MenuItem("GameObject/UI/EasyUI/CanvasGroup")]
        public static void CanvasGroup()
        {
            NewGo("cg_", typeof(CanvasGroup));
        }

        private static void NewGo(string name, Type com)
        {
            var selectedGo = Selection.activeGameObject;
            if (selectedGo == null || selectedGo.GetComponent<RectTransform>() == null)
            {
                return;
            }

            var go = new GameObject(name, typeof(RectTransform), com);
            go.transform.SetParent(selectedGo.transform);
            (go.transform as RectTransform).anchoredPosition = Vector2.zero;
        }
    }
}