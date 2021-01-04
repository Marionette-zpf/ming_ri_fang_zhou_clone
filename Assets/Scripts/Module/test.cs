using EasyUI.Ext;
using LitJson;
using Module.Story;
using Scripts.UIBase.Story;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Module
{
    /// <summary>
    /// Date    2020/12/23 23:22:49
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class test : MonoBehaviour
    {
        public LayoutGroup layoutGroup;
        public GameObject Prefab;

        private LayoutGroupExt<BaseStoryCharCom, Color> groupExt;

        public TextAsset textAsset;

        private void Start()
        {
            //groupExt = new LayoutGroupExt<BaseStoryCharCom, Color>(layoutGroup, () => Instantiate(Prefab), ItemRenderHandler);
            //groupExt.Data = new[] { Color.red, Color.green, Color.yellow };

            //groupExt.Data = new[] { Color.black, Color.blue };

            var info = JsonMapper.ToObject<StoryInfo>(textAsset.text);
            //StoryModule.Inst.StartStory(info);
        }

        private void ItemRenderHandler(int index, BaseStoryCharCom com, Color data)
        {
            com.image_char.color = data;
        }
    }
}