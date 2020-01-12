using System.Collections.Generic;
using Cucu.Colors;
using Cucu.Log;
using Cucu.Trigger;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Examples.Scripts
{
    public class CucuExampleSceneController : MonoBehaviour
    {
        [SerializeField] private GameObject _increaseZone;
        [SerializeField] private GameObject _discreaseZone;
        [SerializeField] private GameObject _cucuMember;
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
            _increaseZone.GetComponent<CucuTrigger>()
                .RegisterComponent<Transform>()
                .AddListener(tr =>
                {
                    if (tr is Transform trans)
                    {
                        IncreaseObject(trans);
                    }
                });

            _discreaseZone.GetComponent<CucuTrigger>()
                .RegisterComponent<Transform>()
                .AddListener(tr =>
                {
                    if (tr is Transform trans)
                    {
                        DiscreaseObject(trans);
                    }
                });
        }

        private void CreateCucumber()
        {
            var cucu = Instantiate(_cucuMember,
                _cucuRoot.position + Random.onUnitSphere * 2, Random.rotation,
                _cucuRoot);

            cucu.transform.localScale *= (Random.value - 0.5f) * 0.333f + 1f;
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

        private void IncreaseObject(Transform obj)
        {
            obj.localScale *= 2f;
        }

        private void DiscreaseObject(Transform obj)
        {
            obj.localScale *= 0.5f;
        }
    }
}