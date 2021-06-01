using CucuTools;
using CucuTools.Colors;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class ExampleColor : CucuBehaviour
    {
        public Color result;
        [Range(0f, 1f)]
        public float blend;
        public CucuColorMap palette;
        public Gradient source;
        public Image image;
        
        private void OnValidate()
        {
            source = CucuColor.GradientSample[palette];
            result = source.Evaluate(blend);

            if (image != null) image.color = result;
        }
    }
}