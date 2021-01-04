using UnityEngine.UI;
using EasyUI;

namespace Scripts.UIBase.Story
{
    /// <summary>
    /// ---------------- auto generator -----------------
    /// </summary>
    public class BaseStoryPanel : BasePanel
    {
        
        private Image m_image_bg;
        public Image image_bg
        {
            get
            {
                if (m_image_bg == null)
                {
                    m_image_bg = transform.Find("image_bg").GetComponent<Image>();
                }
                return m_image_bg;
            }
        }
        
        private HorizontalLayoutGroup m_hlGroup_char01;
        public HorizontalLayoutGroup hlGroup_char01
        {
            get
            {
                if (m_hlGroup_char01 == null)
                {
                    m_hlGroup_char01 = transform.Find("hlGroup_char01").GetComponent<HorizontalLayoutGroup>();
                }
                return m_hlGroup_char01;
            }
        }
        
        private HorizontalLayoutGroup m_hlGroup_char02;
        public HorizontalLayoutGroup hlGroup_char02
        {
            get
            {
                if (m_hlGroup_char02 == null)
                {
                    m_hlGroup_char02 = transform.Find("hlGroup_char02").GetComponent<HorizontalLayoutGroup>();
                }
                return m_hlGroup_char02;
            }
        }
        
        private Image m_image_dialogBg;
        public Image image_dialogBg
        {
            get
            {
                if (m_image_dialogBg == null)
                {
                    m_image_dialogBg = transform.Find("image_dialogBg").GetComponent<Image>();
                }
                return m_image_dialogBg;
            }
        }
        
        private Text m_text_char;
        public Text text_char
        {
            get
            {
                if (m_text_char == null)
                {
                    m_text_char = transform.Find("text_char").GetComponent<Text>();
                }
                return m_text_char;
            }
        }
        
        private Text m_text_dialog;
        public Text text_dialog
        {
            get
            {
                if (m_text_dialog == null)
                {
                    m_text_dialog = transform.Find("text_dialog").GetComponent<Text>();
                }
                return m_text_dialog;
            }
        }
        
    }
}
        