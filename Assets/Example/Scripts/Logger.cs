using CucuTools;
using UnityEngine;

namespace Example.Scripts
{
    public class Logger : MonoBehaviour
    {
        public void Log(string msg)
        {
            Debug.Log(msg);
        }
        
        public void Log(Color clr)
        {
            Debug.Log(clr.ToHex());
        }
    }
}