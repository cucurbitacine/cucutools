using System;
using UnityEngine;

namespace CucuTools.Serializing.Datas
{
    [Serializable]
    public class SerializedGameObject
    {
        public string name;
        public string tag;
        public int layer;
        public bool activeSelf;

        public SerializedGameObject(GameObject gameObject)
        {
            name = gameObject.name;
            tag = gameObject.tag;
            layer = gameObject.layer;
            activeSelf = gameObject.activeSelf;
        }
        
        public GameObject Create()
        {
            var res = new GameObject(name) {tag = tag, layer = layer};
            res.SetActive(activeSelf);
            return res;
        }
    }
}