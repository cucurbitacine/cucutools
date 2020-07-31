using UnityEngine;

namespace CucuTools.Example
{
    public class CucuMarker : CucuButtonByVision
    {
        [Header("Marker settings")]
        [SerializeField] private Transform marker = null;
        [SerializeField, Range(0f,10f)] private float speedRotation = 2f;
        [SerializeField, Range(0f,10f)] private float speedFloat = 2.5f;
        [SerializeField, Range(0f,2f)] private float amplitudeFloat = 0.2f;

        private Animation animation;
        private ParticleSystem particleSystem;
        private Color color;
        private float t =0.0f;
        
        private void Awake()
        {
            animation = GetComponent<Animation>();
            particleSystem = GetComponent<ParticleSystem>();
            color = marker.GetComponent<Renderer>().material.color;
            animation.Play();
            ExecuteOnFocusChange(Focus);
        }

        protected override void ExecuteOnClick()
        {
            animation.Stop();
            marker.gameObject.SetActive(false);
            particleSystem.Play();
            Active = false;
        }

        protected override void ExecuteOnFocusChange(bool value)
        {
            marker.GetComponent<Renderer>().material.color = value ? Color.green : color;
        }

        
    }
}