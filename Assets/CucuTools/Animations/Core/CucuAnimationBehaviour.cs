using UnityEngine;

namespace CucuTools
{
    public abstract class CucuAnimationBehaviour : CucuAnimationEntity
    {
        private void AnimationFrame(float deltaTime)
        {
            progressDisplay = Blend;
            
            if (CurrentTime > AnimationTime)
            {
                progressDisplay = 1f;

                CurrentTime = AnimationTime;

                Lerp(1f);
                
                StopAnimation();
                return;
            }

            Lerp(Blend);
            CurrentTime += deltaTime;
        }
        
        private void Update()
        {
            if (Playing) AnimationFrame(Time.deltaTime);
        }
    }
}