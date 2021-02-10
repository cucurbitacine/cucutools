using CucuTools;
using UnityEngine;

namespace Example.Scripts
{
    public class ExampleController : MonoBehaviour
    {
        [Header("Observer")]
        public LerpColor lerpColor;

        [Header("Listeners")]
        public ListenerEntity<Color> listenerColor;
        public ListenerEntity listenerVoid;
        public Color color;
        
        private void Start()
        {
            listenerVoid = new ListenerEntity(() => { color = lerpColor.Value; });
            listenerColor = new ListenerEntity<Color>(lerpColor);

            lerpColor.Subscribe(listenerVoid, listenerColor);
        }
    }
}