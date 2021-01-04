using UnityEngine.UI;
using EasyUI;

namespace Scripts.UIBase.Story
{
    /// <summary>
    /// ---------------- auto generator -----------------
    /// </summary>
    public class BaseStoryCharCom : EUiCom
    {
        
        private Image m_image_char;
        public Image image_char
        {
            get
            {
                if (m_image_char == null)
                {
                    m_image_char = transform.Find("image_char").GetComponent<Image>();
                }
                return m_image_char;
            }
        }
        
    }
}
        