using System;
using System.Linq;
using CucuTools.Serializing.Components;
using UnityEngine;

namespace CucuTools.Serializing.Datas
{
    [Serializable]
    public class SerializedNewGameObject
    {
        public SerializedGameObject gameObject;
        public SerializedNewComponent[] components;

        public SerializedNewGameObject(GameObject gameObject)
        {
            this.gameObject = new SerializedGameObject(gameObject);

            components = gameObject
                .GetComponents<SerializableComponent>()
                .Select(s => new SerializedNewComponent(s))
                .ToArray();
        }
    }
}