using System.Collections;
using CucuTools;
using UnityEngine;

public class VisionFollow : MonoBehaviour
{
    public Transform target;
    public GameObject bulletPrefab;

    public CucuVision vision;
    
    public ParticleSystem shootEffectPrefab;
    public ParticleSystem shootEffect;
    public ParticleSystem hitEffectPrefab;
    
    private void Awake()
    {
        shootEffect = Instantiate(shootEffectPrefab, bulletPrefab.transform.position, bulletPrefab.transform.rotation,
            transform);
        bulletPrefab.SetActive(false);
    }
    
    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        
        transform.rotation.SetLookRotation(target.forward, target.up);
    }

    private void Shoot()
    {
        shootEffect.Play();

        if (vision.TryGetTarget(out var info))
        {
            var hitEffect = Instantiate(hitEffectPrefab, info.point, Quaternion.identity);
            hitEffect.transform.forward = info.normal;
            
            hitEffect.Play();
            
            DestroyAfter(hitEffect.gameObject, 5f);
        }
    }

    private void DestroyAfter(GameObject obj, float delay)
    {
        StartCoroutine(_DestroyAfter(obj, delay));
        
        IEnumerator _DestroyAfter(GameObject _obj, float _delay)
        {
            var timer = 0.0f;
            while (timer < _delay)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            Destroy(_obj);
        }
    }
}
