using System;
using UnityEngine;
using UnityEngine.UI;

namespace Example.Scripts
{
    public class TextBoolSelector : MonoBehaviour
    {
        public string ifTrue = "+";
        public string ifFalse = "-";

        public Text text;

        public void UpdateText(bool value)
        {
            if (text == null) return;

            text.text = value ? ifTrue : ifFalse;
        }

        private void OnValidate()
        {
            if (text == null) text = GetComponent<Text>();
        }
    }
}