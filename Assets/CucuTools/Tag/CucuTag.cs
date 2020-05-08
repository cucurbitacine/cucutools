using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class CucuTag : MonoBehaviour
    {
        public readonly static List<CucuTag> Tags = new List<CucuTag>();
        public string Key => _key;
        public List<CucuTagArg> Args => args ?? (args = new List<CucuTagArg>());
        public Guid Guid => (Guid) (_guid ?? (_guid = Guid.NewGuid()));

        [SerializeField] private string _key;
        [SerializeField] private List<CucuTagArg> args;
        private Guid? _guid;

        [SerializeField] private bool _drawGizmos;

        public CucuTag SetKey(string key)
        {
            _key = key ?? "";
            return this;
        }

        private void OnEnable()
        {
            Tags.Add(this);
        }

        private void OnDisable()
        {
            Tags.Remove(this);
        }

        private void OnDestroy()
        {
            Tags.Remove(this);
        }

        public static CucuTag WithKey(string key)
        {
            return WithKeys(key).Single();
        }
        
        public static IEnumerable<CucuTag> WithKeys(params string[] keys)
        {
            return Tags.SelectWithKeys(keys);
        }

        public static IEnumerable<CucuTag> WithArgKeys(params string[] keys)
        {
            return Tags.SelectWithArgKeys(keys);
        }
        
        public static IEnumerable<CucuTag> WithArg(string key, string value)
        {
            return Tags.SelectWithArg(key, value);
        }

        public static IEnumerable<CucuTag> WithArg(CucuTagArg arg)
        {
            return Tags.SelectWithArg(arg);
        }

        public static IEnumerable<CucuTag> WithArgs(IEnumerable<CucuTagArg> args)
        {
            return Tags.SelectWithArgs(args);
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_drawGizmos) return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right);
            Gizmos.DrawLine(transform.position, transform.position - transform.right);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);

            Gizmos.color = new Color(57f / 255f, 255f / 255f, 20f / 255f, 0.2f);
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
#endif
    }

    public static class CucuTagExt
    {
        #region Base Ext

        public static CucuTag AddCucuTag(this GameObject gameObject, string tag)
        {
            return gameObject.AddComponent<CucuTag>().SetKey(tag);
        }
        
        public static CucuTag AddArgs(this CucuTag tag, params CucuTagArg[] args)
        {
            tag.Args.AddRange(args);
            return tag;
        }
        
        public static IEnumerable<CucuTag> SelectWithKeys(this IEnumerable<CucuTag> tags, params string[] keys)
        {
            return tags.Where(t => keys.Any(a => a == t.Key));
        }
        
        public static IEnumerable<CucuTag> SelectWithArgKeys(this IEnumerable<CucuTag> tags, params string[] keys)
        {
            return tags.Where(t => t.Args.Any(a => keys.All(k => k == a.key)));
        }
        
        public static IEnumerable<CucuTag> SelectWithArgs(this IEnumerable<CucuTag> tags, IEnumerable<CucuTagArg> args)
        {
            return from tag in tags
                let t = tag
                where (args as CucuTagArg[] ?? args.ToArray()).All(a => t.Args.Contains(a))
                select tag;
        }

        #endregion

        #region Additional Ext

        public static CucuTag AddArgs(this CucuTag tag, string key, string value)
        {
            return tag.AddArgs(new CucuTagArg(key, value));
        }
        
        public static CucuTag AddCucuTag(this Transform transform, string tag)
        {
            return transform.gameObject.AddCucuTag(tag);
        }
        
        public static IEnumerable<CucuTag> SelectWithArg(this IEnumerable<CucuTag> tags, CucuTagArg arg)
        {
            return tags.SelectWithArgs(new[] {arg});
        }

        public static IEnumerable<CucuTag> SelectWithArg(this IEnumerable<CucuTag> tags, string key, string value)
        {
            return tags.SelectWithArg(new CucuTagArg(key, value));
        }

        #endregion
    }
}