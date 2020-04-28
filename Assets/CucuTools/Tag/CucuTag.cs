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
        public IList<CucuTagArg> Args => args;
        public Guid Guid => (Guid) (_guid ?? (_guid = Guid.NewGuid()));

        [SerializeField] private string _key;
        [SerializeField] private List<CucuTagArg> args;
        private Guid? _guid;

        [SerializeField] private bool _drawGizmos;

        public void SetKey(string key)
        {
            _key = key ?? "";
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

        public static IEnumerable<CucuTag> GetTags(string key)
        {
            return Tags.Where(t => t.Key.Equals(key)).ToList();
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(string key, string value, IEnumerable<CucuTag> tags = null)
        {
            return GetTagsByArgs(new CucuTagArg {key = key, value = value}, tags);
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(CucuTagArg arg, IEnumerable<CucuTag> tags = null)
        {
            return GetTagsByArgs(new[] {arg}, tags);
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(IEnumerable<CucuTagArg> args, IEnumerable<CucuTag> tags = null)
        {
            if (tags == null) tags = Tags;

            var tagArgs = args as CucuTagArg[] ?? args.ToArray();

            return from cucuTag in tags let t = cucuTag where tagArgs.All(a => t.Args.Contains(a)) select cucuTag;
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
        public static CucuTag AddCucuTag(this GameObject gameObject, string tag)
        {
            var cucuTag = gameObject.AddComponent<CucuTag>();
            cucuTag.SetKey(tag);
            return cucuTag;
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(this IEnumerable<CucuTag> tags, IEnumerable<CucuTagArg> args)
        {
            return CucuTag.GetTagsByArgs(args, tags);
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(this IEnumerable<CucuTag> tags, CucuTagArg arg)
        {
            return CucuTag.GetTagsByArgs(arg, tags);
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(this IEnumerable<CucuTag> tags, string key, string value)
        {
            return CucuTag.GetTagsByArgs(key, value, tags);
        }
    }
}