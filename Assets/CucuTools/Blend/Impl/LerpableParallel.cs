using UnityEngine;

namespace CucuTools
{
    public class LerpableParallel : LerpableEntity
    {
        [Header("Elements")]
        [SerializeField] private LerpableEntity[] _elements;
        
        protected override bool UpdateEntityInternal()
        {
            if (_elements == null) return false;
            if (_elements.Length == 0) return false;

            foreach (var element in _elements)
            {
                element?.Lerp(LerpValue);
            }

            return true;
        }
    }
}