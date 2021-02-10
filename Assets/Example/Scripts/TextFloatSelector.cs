using UnityEngine;
using UnityEngine.UI;

namespace Example.Scripts
{
    public class TextFloatSelector : MonoBehaviour
    {
        [SerializeField] private Text text;

        public void UpdateText(float t)
        {
            if (text == null) return;

            text.text = t.ToString("F2");
        }

        public void UpdateText(int t)
        {
            if (text == null) return;

            text.text = t.ToString("D3");
        }
        
        private void OnValidate()
        {
            if (text == null) text = GetComponent<Text>();
        }
    }
}