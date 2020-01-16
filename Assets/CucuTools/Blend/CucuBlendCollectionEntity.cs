using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Blend
{
    public abstract class CucuBlendCollectionEntity : CucuBlendEntity
    {
        /// <summary>
        /// List of blend entities
        /// </summary>
        [Header("List of blend units")] [SerializeField]
        protected List<BlendUnit> _blends;

        [Serializable]
        protected class BlendUnit
        {
            [Header("Key")] public string key;
            [Header("Blend entity")] public CucuBlendEntity cucuBlendEntity;
            [Header("Setting")] public BlendUnitSetting setting;

            public void SetBlend(float blend)
            {
                if (!setting.enable) return;
                if (cucuBlendEntity == null) return;

                if (setting.useCurve && setting.curve != null)
                    blend = setting.curve.Evaluate(blend);

                if (Mathf.Abs(cucuBlendEntity.Blend - blend) <= float.Epsilon) return;
                
                cucuBlendEntity.SetBlend(blend);
            }

            [Serializable]
            public class BlendUnitSetting
            {
                public bool enable = true;
                public bool useCurve = false;
                public AnimationCurve curve;
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (_blends == null || !_blends.Any()) return;

            foreach (var blend in _blends)
            {
                if (blend == null) continue;
                blend.key = blend.cucuBlendEntity != null
                    ? $"[{(blend.setting.enable ? "on" : "off")}] {blend.cucuBlendEntity.Key}"
                    : $"[null]";
            }
        }
    }
}