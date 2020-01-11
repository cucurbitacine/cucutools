using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cucu.Tag
{
    public class CucuTag : MonoBehaviour
    {
        public readonly static List<CucuTag> Tags = new List<CucuTag>();

        public string Key => _key;
        public IList<TagArg> Args => args;
        public Guid Guid => (Guid) (_guid ?? (_guid = Guid.NewGuid()));

        [SerializeField] private string _key;
        [SerializeField] private List<TagArg> args;
        private Guid? _guid;

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
            return GetTagsByArgs(new TagArg {key = key, value = value}, tags);
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(TagArg arg, IEnumerable<CucuTag> tags = null)
        {
            return GetTagsByArgs(new[] {arg}, tags);
        }

        public static IEnumerable<CucuTag> GetTagsByArgs(IEnumerable<TagArg> args, IEnumerable<CucuTag> tags = null)
        {
            if (tags == null) tags = Tags;

            var tagArgs = args as TagArg[] ?? args.ToArray();

            return from cucuTag in tags let t = cucuTag where tagArgs.All(a => t.Args.Contains(a)) select cucuTag;
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right);
            Gizmos.DrawLine(transform.position, transform.position - transform.right);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);

            Gizmos.color = new Color(57f / 255f, 255f / 255f, 20f / 255f, 0.382f);
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
#endif
    }

    [Serializable]
    public struct TagArg
    {
        public string key;
        public string value;

        public override bool Equals(object obj)
        {
            if (!(obj is TagArg arg)) return false;
            return arg.key.Equals(key) && arg.value.Equals(value);
        }
    }
}