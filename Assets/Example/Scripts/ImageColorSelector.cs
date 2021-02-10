using UnityEngine;
using UnityEngine.UI;

namespace Example.Scripts
{
    public class ImageColorSelector : MonoBehaviour
    {
        [SerializeField] private Image image;

        public void UpdateColor(Color color)
        {
            if (image == null) return;

            image.color = color;
        }

        private void OnValidate()
        {
            if (image == null) image = GetComponent<Image>();
        }
    }
}