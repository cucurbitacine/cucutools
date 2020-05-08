using UnityEngine;

namespace CucuTools
{
    public interface IMonoBehaviour
    {
        string tag { get; set; }
        string name { get; set; }
        
        Transform transform { get; }
        GameObject gameObject { get; }

        bool TryGetComponent(System.Type type, out Component component);
        bool TryGetComponent<T>(out T component);
        
        Component GetComponent(System.Type type);
        Component GetComponent(string type);
        Component GetComponentInChildren(System.Type t, bool includeInactive);
        Component GetComponentInChildren(System.Type t);
        Component GetComponentInParent(System.Type t);
        
        T GetComponent<T>();
        T GetComponentInChildren<T>(bool includeInactive);
        T GetComponentInChildren<T>();
        T GetComponentInParent<T>();
        
        Component[] GetComponents(System.Type type);
        Component[] GetComponentsInChildren(System.Type t, bool includeInactive);
        Component[] GetComponentsInChildren(System.Type t);
        Component[] GetComponentsInParent(System.Type t, bool includeInactive);
        Component[] GetComponentsInParent(System.Type t);
        
        T[] GetComponents<T>();
        T[] GetComponentsInChildren<T>(bool includeInactive);
        T[] GetComponentsInChildren<T>();
        T[] GetComponentsInParent<T>(bool includeInactive);
        T[] GetComponentsInParent<T>();

        int GetInstanceID();
        int GetHashCode();
        string ToString();
    }
}