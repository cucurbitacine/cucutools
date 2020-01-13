using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cucu.Colors;
using Cucu.Log;
using Cucu.Trigger;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Examples.Scripts
{
    public class CucuExampleSceneController : MonoBehaviour
    {
        [FormerlySerializedAs("_increaseZone")] [SerializeField]
        private GameObject _cubanationZone;

        [FormerlySerializedAs("_discreaseZone")] [SerializeField]
        private GameObject _spheranationZone;

        [SerializeField] private GameObject _cucuMember;
        [SerializeField] private GameObject _cucuCube;
        [SerializeField] private GameObject _cucuSphere;
        [SerializeField] private Transform _cucuRoot;

        [SerializeField, Range(1, 2000)] private int _countMax = 500;
        [SerializeField, Range(0, 2000)] private int _countCurr = 0;
        private List<Transform> _cucus = new List<Transform>();

        private void Awake()
        {
            InitLoggers();
            InitTriggers();
        }

        private void Update()
        {
            CreateCucumber();
        }

        private void InitLoggers()
        {
        }

        private void InitTriggers()
        {
            var cubeTrigger = _cubanationZone.GetComponent<CucuTrigger>();

            cubeTrigger.RegisterComponent<Transform>(CucuTrigger.TriggerState.Enter)
                .AddListener(tr =>
                {
                    var trans = tr as Transform;
                    trans.gameObject.GetComponentInChildren<Renderer>().material
                        .SetColor("_EmissionColor", CucuColor.Palettes.Jet.Get(1f * _countCurr / _countMax));
                });

            cubeTrigger.RegisterComponent<Transform>(CucuTrigger.TriggerState.Exit)
                .AddListener(tr =>
                {
                    var trans = tr as Transform;
                    CreateObject(trans, _cucuCube);
                });

            var sphereTrigger = _spheranationZone.GetComponent<CucuTrigger>();

            sphereTrigger.RegisterComponent<Transform>(CucuTrigger.TriggerState.Enter)
                .AddListener(tr =>
                {
                    var trans = tr as Transform;
                    trans.gameObject.GetComponentInChildren<Renderer>().material
                        .SetColor("_EmissionColor", CucuColor.Palettes.Jet.Get(1f * _countCurr / _countMax));
                });

            sphereTrigger.RegisterComponent<Transform>(CucuTrigger.TriggerState.Exit)
                .AddListener(tr =>
                {
                    if (tr is Transform trans)
                    {
                        CreateObject(trans, _cucuSphere);
                    }
                });
        }

        private void CreateCucumber()
        {
            var cucu = Instantiate(_cucuMember,
                _cucuRoot.position + Random.onUnitSphere * 2, Random.rotation,
                _cucuRoot);

            cucu.transform.localScale *= (Random.value - 0.5f) * 0.333f + 2f;
            cucu.name = $"{_cucus.Count}_{cucu.name}";

            cucu.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * 1000);

            _cucus.Add(cucu.transform);

            ClearCucumbers();

            _countCurr = _cucus.Count;
        }

        private void ClearCucumbers()
        {
            while (_cucus.Count > _countMax)
                DeleteCucumber();
        }

        private void DeleteCucumber()
        {
            if (_cucus.Count > 0)
            {
                var cucu = _cucus[0];
                _cucus.RemoveAt(0);
                if (cucu != null)
                    Destroy(cucu.gameObject);
            }
        }

        private void CreateObject(Transform origin, GameObject target)
        {
            var index = _cucus.IndexOf(origin);

            if (!(0 <= index && index < _cucus.Count))
            {
                Destroy(origin.gameObject);
                return;
            }

            var obj = Instantiate(target, origin.position, origin.rotation);
            _cucus[index] = obj.transform;

            obj.transform.parent = origin.parent;

            var rigOrigin = origin.gameObject.GetComponent<Rigidbody>();
            var rigTarget = obj.GetComponent<Rigidbody>();

            rigTarget.velocity = rigOrigin.velocity;
            rigTarget.angularVelocity = rigOrigin.angularVelocity;

            Destroy(origin.gameObject);

            StartCoroutine(AfterBurn(obj.transform, 1f));
        }

        private IEnumerator AfterBurn(Transform obj, float duration)
        {
            var renderer = obj.gameObject.GetComponent<Renderer>();

            var colorInit = CucuColor.Palettes.Jet.Get(1f * _countCurr / _countMax);

            var time = 0.0f;
            while (time < duration)
            {
                if (renderer == null) yield break;

                var t = time / duration;

                renderer.material.SetColor("_EmissionColor", colorInit.LerpTo(Color.black, t));

                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}