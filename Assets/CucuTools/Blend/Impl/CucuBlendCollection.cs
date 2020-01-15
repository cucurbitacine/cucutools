using System.Collections.Generic;
using System.Linq;

namespace CucuTools.Blend.Impl
{
    /// <inheritdoc />
    public class CucuBlendCollection : CucuBlendCollectionEntity
    {
        public override string Key => "Collection blend-collection";
        
        /// <summary>
        /// List of blend entities
        /// </summary>
        public IReadOnlyList<CucuBlendEntity> Blends => _blends?.Select(b => b.cucuBlendEntity).ToList();

        /// <summary>
        /// Add blend to list
        /// </summary>
        /// <param name="cucuBlend">Blend entity</param>
        public void AddBlend(CucuBlendEntity cucuBlend)
        {
            if (Equals(cucuBlend)) return;
            _blends.Add(new BlendUnit {cucuBlendEntity = cucuBlend});
        }

        /// <summary>
        /// Remove blend from list
        /// </summary>
        /// <param name="cucuBlend">Blend entity</param>
        public void RemoveBlend(CucuBlendEntity cucuBlend)
        {
            var bl = _blends.FirstOrDefault(b => b.cucuBlendEntity.Equals(cucuBlend));
            if (bl != null) _blends.Remove(bl);
        }

        /// <inheritdoc />
        protected override void UpdateEntity()
        {
            foreach (var blend in _blends)
            {
                blend?.SetBlend(Blend);
            }
        }
    }
}