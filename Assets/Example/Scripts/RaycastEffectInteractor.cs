using CucuTools.Raycasts.Effects;

namespace Example.Scripts
{
    public class RaycastEffectInteractor : RaycastEffectBase
    {
        public InteracableBehavior interacable;

        public void Click()
        {
            interacable?.Click();
        }
        
        public override void UpdateEffect()
        {
            if (Raycaster.Raycast(out var hit))
            {
                var inter = hit.transform.GetComponent<InteracableBehavior>();

                if (interacable != null)
                {
                    if (inter == null) Change(inter);
                    else if (interacable != inter) Change(inter);
                }
                else
                {
                    if (inter != null) Change(inter);
                }
            }
            else
            {
                if (interacable != null) Change(null);
            }
        }

        private void Change(InteracableBehavior inter)
        {
            interacable?.Normal();
        
            interacable = inter;

            interacable?.Hover();
        }
    }
}